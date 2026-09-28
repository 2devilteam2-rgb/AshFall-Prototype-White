using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Ashfall.Sparks.Components;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState(true), Virtual]
public partial class ESBaseSparkConfigurationComponent : Component
{
    [DataField, AutoNetworkedField]
    public int Count = 4;

    [DataField, AutoNetworkedField]
    public float Prob = 1f;

    [DataField, AutoNetworkedField]
    public float TileFireChance = 0f;

    [DataField, AutoNetworkedField]
    public EntProtoId? SparkPrototype = null;
}
