using Robust.Shared.GameStates;

namespace Content.Shared.Ashfall.Sparks.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true)]
public sealed partial class ESSparkOnItemToggleComponent : ESBaseSparkConfigurationComponent
{
    [DataField]
    public bool ActivatedSpark = true;
}
