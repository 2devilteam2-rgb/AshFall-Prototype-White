using Content.Shared._Starlight.Zones;
using Robust.Shared.Prototypes;

namespace Ashfall.Server.Degradation.Components;

[RegisterComponent]
public sealed partial class DegradationZoneTargetComponent : Component
{
    [DataField(required: true)]
    public ProtoId<ZonePrototype> Zone;
}
