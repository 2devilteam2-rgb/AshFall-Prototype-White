using Robust.Shared.GameStates;

namespace Content.Shared.Ashfall.Carrying;

[RegisterComponent, NetworkedComponent]
public sealed partial class CarriableComponent : Component
{
    /// <summary>
    /// How long picking this entity up takes.
    /// </summary>
    [DataField]
    public TimeSpan CarryTime = TimeSpan.FromSeconds(3f);
}
