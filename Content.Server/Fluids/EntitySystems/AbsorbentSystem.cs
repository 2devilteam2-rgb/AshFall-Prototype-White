using Content.Server.Ashfall.Fire.Components;
using Content.Server.Ashfall.Fire.Systems;
using Content.Server.Atmos.EntitySystems;
using Content.Shared.Ashfall.Fire.Components;
using Content.Shared.Atmos.Components;
using Content.Shared.Chemistry.EntitySystems;
using Content.Shared.Chemistry.Reagent;
using Content.Shared.Fluids;
using Content.Shared.Fluids.Components;
using Content.Shared.Interaction;
using Content.Shared.Popups;
using Content.Shared.Weapons.Melee.Events;
using Robust.Shared.Audio.Systems;
using Robust.Shared.Prototypes;

namespace Content.Server.Fluids.EntitySystems;

/// <inheritdoc/>
public sealed partial class AbsorbentSystem : SharedAbsorbentSystem
{
    [Dependency] private FlammableSystem _flammable = default!;
    [Dependency] private ReagentFireSystem _reagentFire = default!;
    [Dependency] private SharedAudioSystem _audio = default!;
    [Dependency] private SharedPopupSystem _popups = default!;
    [Dependency] private IPrototypeManager _prototype = default!;
    [Dependency] private EntityLookupSystem _lookup = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<AbsorbentComponent, AfterInteractEvent>(OnAbsorbentAfterInteract);
        SubscribeLocalEvent<AbsorbentComponent, MeleeHitEvent>(OnAbsorbentMeleeHit);
    }

    private bool IsMopWetWithExtinguisher(EntityUid mop, AbsorbentComponent comp)
    {
        if (!SolutionContainer.TryGetSolution(mop, comp.SolutionName, out _, out var solution))
            return false;

        if (solution.GetSolutionFlammability(_prototype) > 0)
            return false;

        foreach (var (reagent, quantity) in solution.Contents)
        {
            if (quantity <= 0)
                continue;
            if (_prototype.TryIndex<ReagentPrototype>(reagent.Prototype, out var proto) &&
                proto.ReactiveEffects != null && proto.ReactiveEffects.ContainsKey("Extinguish"))
            {
                return true;
            }
        }
        return false;
    }

    private void OnAbsorbentAfterInteract(Entity<AbsorbentComponent> ent, ref AfterInteractEvent args)
    {
        if (!args.CanReach || args.Handled)
            return;

        var coords = args.ClickLocation;
        var wet = IsMopWetWithExtinguisher(ent.Owner, ent.Comp);
        var extinguished = false;

        var candidates = new HashSet<EntityUid>();
        if (args.Target != null)
            candidates.Add(args.Target.Value);

        candidates.UnionWith(_lookup.GetEntitiesInRange(coords, 0.75f));

        foreach (var candidate in candidates)
        {
            if (TryComp<FlammableComponent>(candidate, out var fire) && fire.OnFire)
            {
                extinguished = true;
                if (wet)
                {
                    _flammable.AdjustFireStacks(candidate, -fire.FireStacks, fire);
                    _flammable.Extinguish(candidate, fire);
                }
                else
                {
                    _flammable.AdjustFireStacks(candidate, -1f, fire);
                    if (fire.FireStacks <= 0)
                        _flammable.Extinguish(candidate, fire);
                }
            }

            if (TryComp<SolidFuelComponent>(candidate, out var fuel))
            {
                extinguished = true;
                if (wet)
                {
                    if (TryComp<FlammableComponent>(candidate, out var fuelFire) && fuelFire.OnFire)
                    {
                        _flammable.AdjustFireStacks(candidate, -fuelFire.FireStacks, fuelFire);
                        _flammable.Extinguish(candidate, fuelFire);
                    }
                    fuel.WetTime = MathF.Max(fuel.WetTime, 8f);
                    fuel.Exposure = 0;
                }
                else
                {
                    if (TryComp<FlammableComponent>(candidate, out var fuelFire) && fuelFire.OnFire)
                    {
                        _flammable.AdjustFireStacks(candidate, -1f, fuelFire);
                        if (fuelFire.FireStacks <= 0)
                            _flammable.Extinguish(candidate, fuelFire);
                    }
                }
            }

            if (wet && TryComp<ReagentPuddleFireComponent>(candidate, out var puddleFire) && puddleFire.OnFire)
            {
                extinguished = true;
                _reagentFire.Extinguish(candidate);
            }
        }

        if (extinguished)
        {
            if (wet)
                _audio.PlayPvs("/Audio/Effects/sizzle.ogg", ent);
            else
                _audio.PlayPvs("/Audio/Effects/thudswoosh.ogg", ent);

            _popups.PopupEntity(Loc.GetString("ashfall-fire-extinguished-mop"), ent, args.User);
            args.Handled = true;
        }
    }

    private void OnAbsorbentMeleeHit(Entity<AbsorbentComponent> ent, ref MeleeHitEvent args)
    {
        var wet = IsMopWetWithExtinguisher(ent.Owner, ent.Comp);
        var extinguished = false;

        var candidates = new HashSet<EntityUid>(args.HitEntities);
        var userCoords = Transform(args.User).Coordinates;
        candidates.UnionWith(_lookup.GetEntitiesInRange(userCoords, 1.25f));

        foreach (var candidate in candidates)
        {
            if (TryComp<FlammableComponent>(candidate, out var fire) && fire.OnFire)
            {
                extinguished = true;
                if (wet)
                {
                    _flammable.AdjustFireStacks(candidate, -2f, fire);
                    if (fire.FireStacks <= 0)
                        _flammable.Extinguish(candidate, fire);
                }
                else
                {
                    _flammable.AdjustFireStacks(candidate, -0.75f, fire);
                    if (fire.FireStacks <= 0)
                        _flammable.Extinguish(candidate, fire);
                }
            }

            if (TryComp<SolidFuelComponent>(candidate, out var fuel))
            {
                extinguished = true;
                if (wet)
                {
                    if (TryComp<FlammableComponent>(candidate, out var fuelFire) && fuelFire.OnFire)
                    {
                        _flammable.AdjustFireStacks(candidate, -fuelFire.FireStacks, fuelFire);
                        _flammable.Extinguish(candidate, fuelFire);
                    }
                    fuel.WetTime = MathF.Max(fuel.WetTime, 8f);
                    fuel.Exposure = 0;
                }
                else
                {
                    if (TryComp<FlammableComponent>(candidate, out var fuelFire) && fuelFire.OnFire)
                    {
                        _flammable.AdjustFireStacks(candidate, -0.75f, fuelFire);
                        if (fuelFire.FireStacks <= 0)
                            _flammable.Extinguish(candidate, fuelFire);
                    }
                }
            }

            if (wet && TryComp<ReagentPuddleFireComponent>(candidate, out var puddleFire) && puddleFire.OnFire)
            {
                extinguished = true;
                _reagentFire.Extinguish(candidate);
            }
        }

        if (extinguished)
        {
            if (wet)
                _audio.PlayPvs("/Audio/Effects/sizzle.ogg", ent);
            else
                _audio.PlayPvs("/Audio/Effects/thudswoosh.ogg", ent);
        }
    }
}
