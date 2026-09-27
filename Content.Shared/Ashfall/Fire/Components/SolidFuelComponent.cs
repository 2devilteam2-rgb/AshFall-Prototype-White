using Content.Shared.Maps;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;

namespace Content.Shared.Ashfall.Fire.Components;

/// <summary>
/// Material which accumulates heat from nearby ignition sources and is consumed while burning.
/// Times are seconds; ignition time is measured against a smouldering cigarette (rate 1).
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class SolidFuelComponent : Component
{
    /// <summary>Exposure required to ignite, in cigarette-equivalent seconds.</summary>
    [DataField, AutoNetworkedField]
    public float IgnitionTime = 90f;

    /// <summary>Seconds of burning before the material is consumed at the default fuel rate.</summary>
    [DataField, AutoNetworkedField]
    public float BurnTime = 60f;

    /// <summary>Exposure lost per second when no ignition source is heating the material.</summary>
    [DataField, AutoNetworkedField]
    public float CoolingRate = 2f;

    /// <summary>Entity spawned when the material is fully consumed.</summary>
    [DataField, AutoNetworkedField]
    public EntProtoId AshPrototype = "Ash";

    /// <summary>Accumulated heat exposure, in cigarette-equivalent seconds.</summary>
    [DataField, AutoNetworkedField]
    public float Exposure;

    /// <summary>Accumulated burning time, scaled by the fuel consumption multiplier.</summary>
    [DataField, AutoNetworkedField]
    public float BurnedTime;

    /// <summary>Seconds of wetness remaining, preventing ignition.</summary>
    [DataField, AutoNetworkedField]
    public float WetTime;

    /// <summary>Original tile type for transient floor fuel entities; null for ordinary objects.</summary>
    [DataField, AutoNetworkedField]
    public ProtoId<ContentTileDefinition>? TileType;
}
