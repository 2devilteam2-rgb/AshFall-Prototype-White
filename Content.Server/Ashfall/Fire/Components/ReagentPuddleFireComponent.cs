using Robust.Shared.Audio;

namespace Content.Server.Ashfall.Fire.Components;

/// <summary>
/// Caches ignition and burn state for puddles that contain flammable reagents.
/// </summary>
[RegisterComponent]
public sealed partial class ReagentPuddleFireComponent : Component
{
    [ViewVariables(VVAccess.ReadWrite)]
    public bool OnFire { get; set; }

    [ViewVariables(VVAccess.ReadWrite)]
    public int FireState { get; set; } = 2;

    [ViewVariables(VVAccess.ReadWrite)]
    public int Flammability { get; set; }

    [ViewVariables(VVAccess.ReadWrite)]
    public bool SelfOxidizing { get; set; }

    [ViewVariables(VVAccess.ReadWrite)]
    public bool NeedsSpread { get; set; }

    [ViewVariables]
    public EntityUid? PlayingStream { get; set; }

    [ViewVariables]
    public EntityUid? FireEffectEntity { get; set; }

    [ViewVariables(VVAccess.ReadWrite), DataField("sound")]
    public SoundSpecifier LoopingSound { get; set; } = new SoundPathSpecifier("/Audio/Effects/fire.ogg");

    [ViewVariables(VVAccess.ReadWrite)]
    public float VolumeFactor { get; set; } = 1f;
}
