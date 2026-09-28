using Content.Shared.DoAfter;
using Robust.Shared.Serialization;

namespace Content.Shared.Ashfall.Fire.Events;

[Serializable, NetSerializable]
public sealed partial class SolidFuelIgnitionDoAfterEvent : SimpleDoAfterEvent
{
}
