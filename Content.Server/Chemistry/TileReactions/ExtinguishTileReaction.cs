using Content.Server.Ashfall.Fire.Components;
using Content.Server.Ashfall.Fire.Systems;
using Content.Server.Atmos.EntitySystems;
using Content.Shared.Ashfall.Fire.Components;
using Content.Shared.Atmos;
using Content.Shared.Atmos.Components;
using Content.Shared.Chemistry.Reaction;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.FixedPoint;
using JetBrains.Annotations;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;

namespace Content.Server.Chemistry.TileReactions
{
    [UsedImplicitly]
    [DataDefinition]
    public sealed partial class ExtinguishTileReaction : ITileReaction
    {
        [DataField("coolingTemperature")] private float _coolingTemperature = 2f;

        public FixedPoint2 TileReact(TileRef tile,
            ReagentPrototype reagent,
            FixedPoint2 reactVolume,
            IEntityManager entityManager,
            List<ReagentData>? data)
        {
            if (reactVolume <= FixedPoint2.Zero || tile.Tile.IsEmpty)
                return FixedPoint2.Zero;

            var extinguishedAnything = false;
            var atmosphereSystem = entityManager.System<AtmosphereSystem>();

            var environment = atmosphereSystem.GetTileMixture(tile.GridUid, null, tile.GridIndices, true);

            if (environment != null && atmosphereSystem.IsHotspotActive(tile.GridUid, tile.GridIndices))
            {
                environment.Temperature =
                    MathF.Max(MathF.Min(environment.Temperature - (_coolingTemperature * 1000f),
                            environment.Temperature / _coolingTemperature), Atmospherics.TCMB);

                atmosphereSystem.ReactTile(tile.GridUid, tile.GridIndices);
                atmosphereSystem.HotspotExtinguish(tile.GridUid, tile.GridIndices);
                extinguishedAnything = true;
            }

            if (entityManager.TryGetComponent<MapGridComponent>(tile.GridUid, out var gridComp))
            {
                var mapSystem = entityManager.System<SharedMapSystem>();
                var lookupSystem = entityManager.System<EntityLookupSystem>();
                var flammableSystem = entityManager.System<FlammableSystem>();
                var audioSystem = entityManager.System<SharedAudioSystem>();
                var coords = mapSystem.GridTileToLocal(tile.GridUid, gridComp, tile.GridIndices);

                // 1. Anchored entities (such as SolidFuelFloorWood)
                var anchored = mapSystem.GetAnchoredEntities(tile.GridUid, gridComp, tile.GridIndices);
                while (anchored.MoveNext(out var ent))
                {
                    if (entityManager.TryGetComponent<SolidFuelComponent>(ent.Value, out var fuel))
                    {
                        if (entityManager.TryGetComponent<FlammableComponent>(ent.Value, out var flam) && flam.OnFire)
                        {
                            flammableSystem.Extinguish(ent.Value, flam);
                            extinguishedAnything = true;
                        }
                        fuel.Exposure = 0;
                        fuel.WetTime = MathF.Max(fuel.WetTime, 6f);
                    }

                    if (entityManager.TryGetComponent<ReagentPuddleFireComponent>(ent.Value, out var puddleFire) && puddleFire.OnFire)
                    {
                        var reagentFire = entityManager.System<ReagentFireSystem>();
                        reagentFire.Extinguish(ent.Value);
                        extinguishedAnything = true;
                    }
                }

                // 2. Entities in tile area (puddles, items, mobs on the tile)
                var entities = lookupSystem.GetEntitiesInRange(coords, 0.6f);
                foreach (var ent in entities)
                {
                    if (entityManager.TryGetComponent<SolidFuelComponent>(ent, out var fuel))
                    {
                        if (entityManager.TryGetComponent<FlammableComponent>(ent, out var flam) && flam.OnFire)
                        {
                            flammableSystem.Extinguish(ent, flam);
                            extinguishedAnything = true;
                        }
                        fuel.Exposure = 0;
                        fuel.WetTime = MathF.Max(fuel.WetTime, 6f);
                    }

                    if (entityManager.TryGetComponent<ReagentPuddleFireComponent>(ent, out var puddleFire) && puddleFire.OnFire)
                    {
                        var reagentFire = entityManager.System<ReagentFireSystem>();
                        reagentFire.Extinguish(ent);
                        extinguishedAnything = true;
                    }

                    if (entityManager.TryGetComponent<FlammableComponent>(ent, out var flammable) && flammable.OnFire)
                    {
                        flammableSystem.AdjustFireStacks(ent, -2.5f, flammable);
                        if (flammable.FireStacks <= 0)
                            flammableSystem.Extinguish(ent, flammable);
                        extinguishedAnything = true;
                    }
                }

                if (extinguishedAnything)
                {
                    audioSystem.PlayPvs("/Audio/Effects/sizzle.ogg", coords);
                }
            }

            return FixedPoint2.Zero;
        }
    }
}
