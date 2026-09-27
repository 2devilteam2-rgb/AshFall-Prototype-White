using Robust.Shared.GameStates;

namespace Content.Shared.Ashfall.Sparks.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class ESSparkOnHitComponent : ESBaseSparkConfigurationComponent
{
    [DataField]
    public float Threshold = 5f;
}
