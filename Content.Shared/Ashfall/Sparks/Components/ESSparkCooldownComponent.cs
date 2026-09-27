using Robust.Shared.GameStates;

namespace Content.Shared.Ashfall.Sparks.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class ESSparkCooldownComponent : Component
{
    [DataField, AutoNetworkedField]
    public TimeSpan SparkDelay = TimeSpan.FromSeconds(0.5);

    [DataField, AutoNetworkedField]
    public TimeSpan? LastSparkTime;
}
