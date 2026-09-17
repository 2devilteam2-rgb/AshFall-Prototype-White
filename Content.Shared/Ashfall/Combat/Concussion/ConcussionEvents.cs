// SPDX-License-Identifier: AGPL-3.0-or-later

using Content.Shared.FixedPoint;
using Content.Shared.Inventory;

namespace Content.Shared.Ashfall.Combat.Concussion;

[ByRefEvent]
public record struct ConcussionStateChangedEvent(
    EntityUid Uid,
    ConcussionThresholdComponent Component,
    ConcussionState OldState,
    ConcussionState NewState
);

[ByRefEvent]
public record struct BeforeConcussionDamageEvent(
    EntityUid Target,
    ConcussionThresholdComponent Component,
    FixedPoint2 Damage
) : IInventoryRelayEvent
{
    public SlotFlags TargetSlots => SlotFlags.HEAD | SlotFlags.MASK | SlotFlags.OUTERCLOTHING;
    public bool Cancelled { get; set; }
}
