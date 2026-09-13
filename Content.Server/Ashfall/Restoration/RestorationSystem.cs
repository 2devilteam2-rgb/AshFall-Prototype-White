using Content.Shared.Ashfall.Restoration;
using Content.Shared.DoAfter;
using Content.Shared.Eye;
using Content.Shared.Hands;
using Content.Shared.Interaction;
using Content.Shared.Maps;
using Content.Shared.Popups;
using Content.Shared.Stacks;
using Content.Shared.Storage;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Prototypes;
using Robust.Shared.Physics;
using System.Linq;
using System.Numerics;

namespace Content.Server.Ashfall.Restoration;

public sealed partial class RestorationSystem : SharedRestorationSystem
{
    [Dependency] private EntityLookupSystem _lookup = default!;
    [Dependency] private SharedAudioSystem _audio = default!;
    [Dependency] private SharedDoAfterSystem _doAfter = default!;
    [Dependency] private SharedEyeSystem _eye = default!;
    [Dependency] private SharedMapSystem _map = default!;
    [Dependency] private SharedPopupSystem _popup = default!;
    [Dependency] private SharedStackSystem _stack = default!;
    [Dependency] private SharedTransformSystem _transform = default!;

    private readonly HashSet<EntityUid> _intersecting = new();

    public override void Initialize()
    {
        base.Initialize();

        SubscribeLocalEvent<RestorationToolComponent, AfterInteractEvent>(OnAfterInteract);
        SubscribeLocalEvent<RestorationToolComponent, RestorationDoAfterEvent>(OnRestoreComplete);
        SubscribeLocalEvent<RestorationToolComponent, GotEquippedHandEvent>(OnEquippedHand);
        SubscribeLocalEvent<RestorationToolComponent, GotUnequippedHandEvent>(OnUnequippedHand);
        SubscribeLocalEvent<RestorationEyeComponent, GetVisMaskEvent>(OnGetVisibility);
    }

    public void CapturePristineLayout(EntityUid gridUid)
    {
        if (!TryComp<MapGridComponent>(gridUid, out var grid))
            return;

        var data = EnsureComp<RestorationGridComponent>(gridUid);
        if (data.SnapshotComplete)
            return;

        data.Chunks.Clear();
        data.EntityPalette.Clear();

        var tiles = _map.GetAllTilesEnumerator(gridUid, grid);
        while (tiles.MoveNext(out var maybeTile))
        {
            if (maybeTile is not { } tile || tile.Tile.IsEmpty)
                continue;

            var chunk = GetOrCreateChunk(data, tile.GridIndices);
            var relative = GetRelativeIndex(tile.GridIndices, data.ChunkSize);
            chunk.Tiles[relative.X + relative.Y * data.ChunkSize] = tile.Tile.TypeId;
        }

        var restorable = new HashSet<Entity<RestorableComponent>>();
        _lookup.GetLocalEntitiesIntersecting(gridUid, grid.LocalAABB, restorable);
        foreach (var entity in restorable)
        {
            if (TerminatingOrDeleted(entity))
                continue;

            var xform = Transform(entity);
            if (xform.ParentUid != gridUid || !xform.Anchored)
                continue;

            var prototype = entity.Comp.RestoreAs?.Id ?? MetaData(entity).EntityPrototype?.ID;
            if (prototype == null)
                continue;

            var prototypeId = new EntProtoId(prototype);
            var paletteIndex = data.EntityPalette.IndexOf(prototypeId);
            if (paletteIndex < 0)
            {
                paletteIndex = data.EntityPalette.Count;
                data.EntityPalette.Add(prototypeId);
            }

            var tile = _map.LocalToTile(gridUid, grid, xform.Coordinates);
            var chunk = GetOrCreateChunk(data, tile);
            chunk.Entities[chunk.NextId++] = new RestorationEntitySnapshot
            {
                PrototypeIndex = paletteIndex,
                LocalPosition = xform.LocalPosition,
                Rotation = xform.LocalRotation,
                OriginalEntity = GetNetEntity(entity),
            };
        }

        data.SnapshotComplete = true;
        Dirty(gridUid, data);
    }

    private RestorationChunk GetOrCreateChunk(RestorationGridComponent data, Vector2i tile)
    {
        var index = GetChunkIndex(tile, data.ChunkSize);
        if (data.Chunks.TryGetValue(index, out var chunk))
            return chunk;

        chunk = new RestorationChunk { Tiles = new int[data.ChunkSize * data.ChunkSize] };
        Array.Fill(chunk.Tiles, Tile.Empty.TypeId);
        data.Chunks[index] = chunk;
        return chunk;
    }

    private void OnAfterInteract(Entity<RestorationToolComponent> tool, ref AfterInteractEvent args)
    {
        if (args.Handled || !args.CanReach || !TryGetTargetGrid(args.ClickLocation, out var grid))
            return;

        if (!TryComp<RestorationGridComponent>(grid, out var data) || !data.SnapshotComplete)
        {
            _popup.PopupEntity(Loc.GetString("ashfall-restoration-no-snapshot"), tool, args.User);
            return;
        }

        var tile = _map.CoordinatesToTile(grid, grid.Comp, args.ClickLocation);
        if (!TryGetChunk(data, tile, out var chunk))
            return;

        var relative = GetRelativeIndex(tile, data.ChunkSize);
        var storedTile = chunk.Tiles[relative.X + relative.Y * data.ChunkSize];
        var currentTile = _map.GetTileRef(grid, grid.Comp, tile).Tile;
        if (storedTile != Tile.Empty.TypeId && currentTile.IsEmpty)
        {
            if (IsOccupied(grid, tile, null))
            {
                _popup.PopupEntity(Loc.GetString("ashfall-restoration-occupied"), tool, args.User);
                return;
            }

            TryStartRestoration(tool, args.User, grid, tile, null, tool.Comp.TileRestoreTime, tool.Comp.TileRequirements);
            args.Handled = true;
            return;
        }

        foreach (var (id, snapshot) in chunk.Entities)
        {
            if ((snapshot.LocalPosition - args.ClickLocation.Position).LengthSquared() >
                tool.Comp.EntitySearchRadius * tool.Comp.EntitySearchRadius)
                continue;

            if (OriginalStillExists(snapshot))
            {
                _popup.PopupEntity(Loc.GetString("ashfall-restoration-original-exists"), tool, args.User);
                continue;
            }

            // a click near a tile edge can select a snapshot on the neighboring tile, so check its own position
            var snapshotTile = _map.LocalToTile(grid.Owner, grid.Comp,
                new EntityCoordinates(grid.Owner, snapshot.LocalPosition));

            if (IsOccupied(grid, snapshotTile, null))
            {
                _popup.PopupEntity(Loc.GetString("ashfall-restoration-occupied"), tool, args.User);
                continue;
            }

            var prototype = data.EntityPalette[snapshot.PrototypeIndex];
            if (!ProtoMan.Index<EntityPrototype>(prototype).TryGetComponent<RestorableComponent>(out var restorable, Factory))
                continue;

            TryStartRestoration(tool, args.User, grid, snapshotTile, id, restorable.RestoreTime, restorable.Requirements);
            args.Handled = true;
            return;
        }
    }

    private bool TryGetTargetGrid(EntityCoordinates coordinates, out Entity<MapGridComponent> grid)
    {
        var xform = Transform(coordinates.EntityId);
        var gridUid = HasComp<MapGridComponent>(coordinates.EntityId) ? coordinates.EntityId : xform.GridUid;
        if (gridUid is { } uid && TryComp<MapGridComponent>(uid, out var gridComp))
        {
            grid = (uid, gridComp);
            return true;
        }

        grid = default;
        return false;
    }

    private void TryStartRestoration(
        Entity<RestorationToolComponent> tool,
        EntityUid user,
        Entity<MapGridComponent> grid,
        Vector2i tile,
        int? snapshotId,
        float delay,
        Dictionary<ProtoId<StackPrototype>, int> requirements)
    {
        if (!HasMaterials(tool, requirements))
        {
            _popup.PopupEntity(Loc.GetString("ashfall-restoration-missing-materials"), tool, user);
            return;
        }

        var ev = new RestorationDoAfterEvent { Grid = grid, Tile = tile, SnapshotId = snapshotId };
        var doAfter = new DoAfterArgs(EntityManager, user, delay, ev, tool, used: tool)
        {
            BreakOnMove = true,
            BreakOnDamage = true,
            NeedHand = true,
            MovementThreshold = 0.5f,
            DuplicateCondition = DuplicateConditions.SameEvent,
        };

        if (_doAfter.TryStartDoAfter(doAfter))
            _audio.PlayPredicted(tool.Comp.RestoreSound, tool, user);
    }

    private void OnRestoreComplete(Entity<RestorationToolComponent> tool, ref RestorationDoAfterEvent args)
    {
        if (args.Cancelled || args.Handled || args.Grid is not { } gridUid ||
            !TryComp<MapGridComponent>(gridUid, out var grid) ||
            !TryComp<RestorationGridComponent>(gridUid, out var data) ||
            !TryGetChunk(data, args.Tile, out var chunk))
            return;

        if (args.SnapshotId is { } id)
        {
            if (!chunk.Entities.TryGetValue(id, out var snapshot) || OriginalStillExists(snapshot))
                return;

            // validate at the snapshot's own position, not the tile the doafter was started from
            var snapshotTile = _map.LocalToTile(gridUid, grid,
                new EntityCoordinates(gridUid, snapshot.LocalPosition));

            if (IsOccupied((gridUid, grid), snapshotTile, null))
                return;

            var prototype = data.EntityPalette[snapshot.PrototypeIndex];
            if (!ProtoMan.Index<EntityPrototype>(prototype).TryGetComponent<RestorableComponent>(out var restorable, Factory))
                return;

            if (!HasMaterials(tool, restorable.Requirements))
            {
                _popup.PopupEntity(Loc.GetString("ashfall-restoration-missing-materials"), tool, args.User);
                return;
            }

            if (!ConsumeMaterials(tool, restorable.Requirements))
                return;

            var spawned = Spawn(prototype, new EntityCoordinates(gridUid, snapshot.LocalPosition));
            _transform.SetLocalRotation(spawned, snapshot.Rotation);
            snapshot.OriginalEntity = GetNetEntity(spawned);
        }
        else
        {
            var relative = GetRelativeIndex(args.Tile, data.ChunkSize);
            var storedTile = chunk.Tiles[relative.X + relative.Y * data.ChunkSize];
            var currentTile = _map.GetTileRef(gridUid, grid, args.Tile).Tile;
            if (storedTile == Tile.Empty.TypeId || !currentTile.IsEmpty || IsOccupied((gridUid, grid), args.Tile, null))
                return;

            if (!HasMaterials(tool, tool.Comp.TileRequirements))
            {
                _popup.PopupEntity(Loc.GetString("ashfall-restoration-missing-materials"), tool, args.User);
                return;
            }

            if (!ConsumeMaterials(tool, tool.Comp.TileRequirements))
                return;

            _map.SetTile(gridUid, grid, args.Tile, new Tile(storedTile));
        }

        Dirty(gridUid, data);
        args.Handled = true;
        _popup.PopupEntity(Loc.GetString("ashfall-restoration-complete"), tool, args.User);
    }

    private bool OriginalStillExists(RestorationEntitySnapshot snapshot)
    {
        return snapshot.OriginalEntity is { } net && TryGetEntity(net, out var uid) && !TerminatingOrDeleted(uid);
    }

    private bool IsOccupied(Entity<MapGridComponent> grid, Vector2i tile, EntityUid? ignored)
    {
        _intersecting.Clear();
        _lookup.GetLocalEntitiesIntersecting(grid.Owner, tile, _intersecting, gridComp: grid.Comp,
            flags: LookupFlags.Static | LookupFlags.Dynamic);

        foreach (var entity in _intersecting)
        {
            if (entity == ignored || TerminatingOrDeleted(entity))
                continue;

            var xform = Transform(entity);
            if (xform.Anchored && xform.ParentUid == grid.Owner &&
                TryComp<FixturesComponent>(entity, out var fixtures) &&
                fixtures.Fixtures.Values.Any(fixture => fixture.Hard))
                return true;
        }

        return false;
    }

    private bool HasMaterials(EntityUid tool, Dictionary<ProtoId<StackPrototype>, int> requirements)
    {
        if (requirements.Count == 0)
            return true;
        if (!TryComp<StorageComponent>(tool, out var storage))
            return false;

        foreach (var (type, required) in requirements)
        {
            var count = 0;
            foreach (var entity in storage.Container.ContainedEntities)
            {
                if (TryComp<StackComponent>(entity, out var stack) && stack.StackTypeId == type)
                    count += stack.Count;
            }

            if (count < required)
                return false;
        }

        return true;
    }

    private bool ConsumeMaterials(EntityUid tool, Dictionary<ProtoId<StackPrototype>, int> requirements)
    {
        if (!HasMaterials(tool, requirements) || !TryComp<StorageComponent>(tool, out var storage))
            return requirements.Count == 0;

        foreach (var (type, required) in requirements)
        {
            var remaining = required;
            foreach (var entity in storage.Container.ContainedEntities.ToArray())
            {
                if (!TryComp<StackComponent>(entity, out var stack) || stack.StackTypeId != type)
                    continue;

                var used = Math.Min(stack.Count, remaining);
                _stack.TryUse((entity, stack), used);
                remaining -= used;
                if (remaining == 0)
                    break;
            }
        }

        return true;
    }

    private void AddEye(EntityUid user)
    {
        var eye = EnsureComp<RestorationEyeComponent>(user);
        eye.Count++;
        if (eye.Count == 1)
            _eye.RefreshVisibilityMask(user);
        Dirty(user, eye);
    }

    private void RemoveEye(EntityUid user)
    {
        if (!TryComp<RestorationEyeComponent>(user, out var eye))
            return;
        eye.Count--;
        if (eye.Count <= 0)
        {
            RemComp<RestorationEyeComponent>(user);
            _eye.RefreshVisibilityMask(user);
            return;
        }
        Dirty(user, eye);
    }

    private void OnEquippedHand(Entity<RestorationToolComponent> ent, ref GotEquippedHandEvent args) => AddEye(args.User);
    private void OnUnequippedHand(Entity<RestorationToolComponent> ent, ref GotUnequippedHandEvent args) => RemoveEye(args.User);
    private void OnGetVisibility(Entity<RestorationEyeComponent> ent, ref GetVisMaskEvent args)
    {
        args.VisibilityMask |= (int) VisibilityFlags.Subfloor;
    }
}
