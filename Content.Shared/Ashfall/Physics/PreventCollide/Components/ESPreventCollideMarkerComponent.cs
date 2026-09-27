using Robust.Shared.GameStates;

namespace Content.Shared.Ashfall.Physics.PreventCollide.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class ESPreventCollideMarkerComponent : Component
{
    [DataField, AutoNetworkedField]
    public HashSet<EntityUid> Entities = new();
}
