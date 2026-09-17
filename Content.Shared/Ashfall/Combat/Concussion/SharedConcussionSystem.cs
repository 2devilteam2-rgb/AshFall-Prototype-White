// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Linq;
using Content.Shared.FixedPoint;
using Content.Shared.Inventory;
using Robust.Shared.Timing;

namespace Content.Shared.Ashfall.Combat.Concussion;

public abstract partial class SharedConcussionSystem : EntitySystem
{
    [Dependency] private InventorySystem _inventory = default!;
    [Dependency] private IGameTiming _timing = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ConcussionProtectionComponent, BeforeConcussionDamageEvent>(OnProtectionConcussionDamage);
    }

    private void OnProtectionConcussionDamage(EntityUid uid, ConcussionProtectionComponent comp, ref BeforeConcussionDamageEvent args)
    {
        if (args.Damage <= FixedPoint2.Zero)
            return;

        var factor = Math.Clamp(1f - comp.Protection, 0f, 1f);
        args.Damage *= factor;
    }

    protected void UpdateConcussionState(EntityUid uid, ConcussionThresholdComponent comp)
    {
        var newState = GetStateForDamage(comp);
        if (newState == comp.CurrentState)
            return;

        var oldState = comp.CurrentState;
        comp.CurrentState = newState;

        var ev = new ConcussionStateChangedEvent(uid, comp, oldState, newState);
        RaiseLocalEvent(uid, ref ev);
    }

    private ConcussionState GetStateForDamage(ConcussionThresholdComponent comp)
    {
        var result = ConcussionState.Sane;
        foreach (var (threshold, state) in comp.Thresholds.OrderBy(x => x.Key))
        {
            if (comp.StoredDamage >= threshold)
                result = state;
        }
        return result;
    }

    public void AddConcussionDamage(EntityUid uid, ConcussionThresholdComponent comp, FixedPoint2 damage)
    {
        if (damage <= FixedPoint2.Zero)
            return;

        var ev = new BeforeConcussionDamageEvent(uid, comp, damage);
        if (TryComp<InventoryComponent>(uid, out var inventoryComp))
            _inventory.RelayEvent((uid, inventoryComp), ref ev);

        if (ev.Cancelled || ev.Damage <= FixedPoint2.Zero)
            return;

        comp.StoredDamage = FixedPoint2.Min(comp.StoredDamage + ev.Damage, comp.AbsoluteCap);
        UpdateConcussionState(uid, comp);
        Dirty(uid, comp);
        comp.NextUpdate = _timing.CurTime + comp.UpdateInterval;
    }
}
