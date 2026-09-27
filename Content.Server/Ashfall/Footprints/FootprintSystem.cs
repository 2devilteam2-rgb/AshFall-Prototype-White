using System.Numerics;
using Content.Server.Atmos.EntitySystems;
using Content.Server.Decals;
using Content.Shared.Ashfall.Fire;
using Content.Shared.Ashfall.Footprints;
using Content.Shared.Atmos;
using Content.Shared.Atmos.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Decals;
using Content.Shared.Fluids.Components;
using Content.Shared.IgnitionSource;
using Content.Trauma.Common.Movement;
using Robust.Shared.Configuration;
using Robust.Shared.Map;
using Robust.Shared.Timing;

namespace Content.Server.Ashfall.Footprints;

public sealed partial class FootprintSystem : EntitySystem
{
    [Dependency] private DecalSystem _decalSystem = default!;
    [Dependency] private IGameTiming _timing = default!;
    [Dependency] private EntityLookupSystem _lookup = default!;
    [Dependency] private SharedTransformSystem _transform = default!;
    [Dependency] private SharedSolutionContainerSystem _solutionContainer = default!;
    [Dependency] private AtmosphereSystem _atmos = default!;
    [Dependency] private FlammableSystem _flammable = default!;
    [Dependency] private IConfigurationManager _cfg = default!;

    private readonly List<(EntityUid Grid, DecalIndex Decal, TimeSpan ExpireTime)> _decayingDecals = new();
    private readonly List<FlammableFootprint> _flammableDecals = new();

    private static readonly Color AshColor = Color.FromHex("#3D3C3B");
    private static readonly Color BloodColor = Color.FromHex("#7D0808");
    private static readonly Color OilColor = Color.FromHex("#1C1B1A");
    private static readonly Color WaterColor = Color.FromHex("#9EC4D5");

    private bool _flammableFootprintsEnabled = true;
    private float _fireCheckAccumulator;

    private sealed class FlammableFootprint
    {
        public EntityUid Grid;
        public DecalIndex Decal;
        public EntityCoordinates Coordinates;
        public TimeSpan ExpireTime;
        public TimeSpan? IgniteAt;

        public FlammableFootprint(EntityUid grid, DecalIndex decal, EntityCoordinates coordinates, TimeSpan expireTime)
        {
            Grid = grid;
            Decal = decal;
            Coordinates = coordinates;
            ExpireTime = expireTime;
        }
    }

    public override void Initialize()
    {
        base.Initialize();

        Subs.CVar(_cfg, AshfallFireCVars.FlammableFootprintsEnabled, value => _flammableFootprintsEnabled = value, true);
        SubscribeLocalEvent<FootprintComponent, FootStepEvent>(OnFootStep);
    }

    private void OnFootStep(EntityUid uid, FootprintComponent component, ref FootStepEvent args)
    {
        var xform = Transform(uid);
        if (xform.GridUid is not { } gridUid)
            return;

        // Check if walking over a puddle
        CheckPuddles(uid, xform.Coordinates, component);

        if (component.StepCount <= 0 || component.PrintColor == null)
            return;

        // Calculate alternating foot placement
        var forward = args.WorldAngle.ToWorldVec();
        var right = new Vector2(forward.Y, -forward.X);
        var side = component.RightFoot ? 1f : -1f;
        component.RightFoot = !component.RightFoot;

        var offset = right * (component.FootOffset * side);
        var footCoords = xform.Coordinates.Offset(offset).Offset(new Vector2(-0.5f, -0.5f));

        // Calculate fade based on remaining steps
        var alpha = (float) component.StepCount / component.MaxSteps;
        var decalColor = component.PrintColor.Value.WithAlpha(Math.Clamp(alpha, 0.25f, 0.95f));
        var decalAngle = args.WorldAngle - Math.PI;

        if (_decalSystem.TryAddDecal(
            component.DecalId,
            footCoords,
            out var decalIndex,
            decalColor,
            decalAngle,
            zIndex: -1,
            cleanable: true))
        {
            var expireTime = _timing.CurTime + component.Lifetime;
            _decayingDecals.Add((gridUid, decalIndex, expireTime));

            if (component.IsFlammable)
            {
                _flammableDecals.Add(new FlammableFootprint(gridUid, decalIndex, footCoords, expireTime));
            }
        }

        component.StepCount--;
        if (component.StepCount <= 0)
            component.IsFlammable = false;

        Dirty(uid, component);
    }

    private void CheckPuddles(EntityUid uid, EntityCoordinates coordinates, FootprintComponent component)
    {
        var entities = _lookup.GetEntitiesIntersecting(coordinates);
        foreach (var ent in entities)
        {
            if (!TryComp<PuddleComponent>(ent, out var puddle))
                continue;

            // Only form footprints if the puddle contains enough solution (matches upstream #4762)
            if (!_solutionContainer.TryGetSolution(ent, puddle.SolutionName, out _, out var solution) || solution.Volume < 5)
                continue;

            var color = DeterminePuddleColor(solution);
            component.PrintColor = color;
            component.StepCount = component.MaxSteps;

            var flammability = solution.GetSolutionFlammability(ProtoMan);
            var isFlammable = flammability > 0;
            if (!isFlammable)
            {
                foreach (var quantity in solution.Contents)
                {
                    var id = quantity.Reagent.Prototype.Id.ToLowerInvariant();
                    if (id.Contains("weldingfuel") || id.Contains("oil") || id.Contains("hydrocarbon") || id.Contains("napalm") || id.Contains("ethanol"))
                    {
                        isFlammable = true;
                        break;
                    }
                }
            }

            component.IsFlammable = isFlammable;
            Dirty(uid, component);
            break;
        }
    }

    private Color DeterminePuddleColor(Content.Shared.Chemistry.Components.Solution solution)
    {
        foreach (var quantity in solution.Contents)
        {
            var id = quantity.Reagent.Prototype.Id.ToLowerInvariant();
            if (id.Contains("blood"))
                return BloodColor;
            if (id.Contains("oil") || id.Contains("weldingfuel") || id.Contains("hydrocarbon"))
                return OilColor;
            if (id.Contains("ash") || id.Contains("soot") || id.Contains("carbon"))
                return AshColor;
        }

        return solution.GetColor(ProtoMan);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var curTime = _timing.CurTime;

        // Decal fading
        if (_decayingDecals.Count > 0)
        {
            for (var i = _decayingDecals.Count - 1; i >= 0; i--)
            {
                var (grid, decal, expire) = _decayingDecals[i];
                if (!Exists(grid))
                {
                    _decayingDecals.RemoveAt(i);
                    continue;
                }

                if (curTime >= expire)
                {
                    _decalSystem.RemoveDecal(grid, decal);
                    _decayingDecals.RemoveAt(i);
                }
            }
        }

        // Flammable footprints handling
        if (!_flammableFootprintsEnabled || _flammableDecals.Count == 0)
            return;

        // Remove expired flammable decals
        for (var i = _flammableDecals.Count - 1; i >= 0; i--)
        {
            var fp = _flammableDecals[i];
            if (!Exists(fp.Grid) || curTime >= fp.ExpireTime)
                _flammableDecals.RemoveAt(i);
        }

        _fireCheckAccumulator += frameTime;
        if (_fireCheckAccumulator < 0.25f)
            return;

        _fireCheckAccumulator = 0f;

        for (var i = _flammableDecals.Count - 1; i >= 0; i--)
        {
            var fp = _flammableDecals[i];
            if (!Exists(fp.Grid))
                continue;

            // Check if scheduled to ignite from adjacent footprint chain
            if (fp.IgniteAt != null && curTime >= fp.IgniteAt.Value)
            {
                IgniteFootprint(fp);
                _flammableDecals.RemoveAt(i);
                continue;
            }

            if (fp.IgniteAt != null)
                continue;

            // Check if tile or nearby entities are on fire
            var tilePos = _transform.GetGridTilePositionOrDefault((fp.Grid, Transform(fp.Grid)));
            var coords = fp.Coordinates;
            if (_atmos.GetTileMixture(fp.Grid, null, new Vector2i((int) MathF.Floor(coords.X), (int) MathF.Floor(coords.Y))) is { } mix
                && mix.Temperature > 450f)
            {
                IgniteFootprint(fp);
                _flammableDecals.RemoveAt(i);
                continue;
            }

            var nearby = _lookup.GetEntitiesInRange(coords, 0.4f);
            foreach (var ent in nearby)
            {
                if (TryComp<FlammableComponent>(ent, out var fire) && fire.OnFire)
                {
                    IgniteFootprint(fp);
                    _flammableDecals.RemoveAt(i);
                    break;
                }
                if (TryComp<IgnitionSourceComponent>(ent, out var ignition) && ignition.Ignited)
                {
                    IgniteFootprint(fp);
                    _flammableDecals.RemoveAt(i);
                    break;
                }
            }
        }
    }

    private void IgniteFootprint(FlammableFootprint fp)
    {
        _decalSystem.RemoveDecal(fp.Grid, fp.Decal);

        // Heat tile atmosphere and light a momentary fire
        var tilePos = new Vector2i((int) MathF.Floor(fp.Coordinates.X), (int) MathF.Floor(fp.Coordinates.Y));
        var tileMix = _atmos.GetTileMixture(fp.Grid, null, tilePos, excite: true);
        if (tileMix != null)
        {
            tileMix.Temperature = MathF.Max(tileMix.Temperature, 650f);
        }

        // Damage and ignite any standing mob
        var standing = _lookup.GetEntitiesInRange(fp.Coordinates, 0.35f);
        foreach (var ent in standing)
        {
            if (TryComp<FlammableComponent>(ent, out var flammable))
            {
                _flammable.AdjustFireStacks(ent, 1f, flammable);
                _flammable.Ignite(ent, fp.Grid, flammable);
            }
        }

        // Chain ignite nearby flammable footprints along the trail
        var now = _timing.CurTime;
        foreach (var other in _flammableDecals)
        {
            if (other == fp || other.IgniteAt != null || other.Grid != fp.Grid)
                continue;

            var distSq = Vector2.DistanceSquared(
                new Vector2(fp.Coordinates.X, fp.Coordinates.Y),
                new Vector2(other.Coordinates.X, other.Coordinates.Y));

            if (distSq <= 1.5f * 1.5f)
            {
                // Delay 0.1s so fire travels visibly along the path
                other.IgniteAt = now + TimeSpan.FromSeconds(0.12);
            }
        }
    }
}
