using Content.Shared.Containers.ItemSlots;
using Content.Shared.Paper;
using Robust.Shared.Audio;
using Robust.Shared.Prototypes;

namespace Content.Shared.Ashfall.Recorder;

[RegisterComponent]
public sealed partial class UniversalRecorderComponent : Component
{
    public const string TapeSlotId = "ashfall_recorder_tape";

    [DataField]
    public ItemSlot TapeSlot = new();

    [DataField]
    public EntProtoId<PaperComponent> PrintoutPrototype = "Paper";

    [DataField]
    public float ListenRange = 10f;

    [DataField]
    public TimeSpan WarningThreshold = TimeSpan.FromMinutes(1);

    [DataField]
    public TimeSpan PrintCooldown = TimeSpan.FromSeconds(30);

    [DataField]
    public TimeSpan PlaybackSilenceThreshold = TimeSpan.FromSeconds(14);

    [DataField]
    public SoundSpecifier PlaySound = new SoundPathSpecifier("/Audio/Machines/button.ogg");

    [DataField]
    public SoundSpecifier StopSound = new SoundPathSpecifier("/Audio/Machines/button.ogg");

    [DataField]
    public SoundSpecifier PrintSound = new SoundPathSpecifier("/Audio/Machines/printer.ogg");

    [DataField]
    public SoundSpecifier HissStartSound = new SoundPathSpecifier("/Audio/Items/hiss.ogg");

    [DataField]
    public SoundSpecifier HissLoopSound = new SoundPathSpecifier("/Audio/Items/hiss.ogg");

    [DataField]
    public TimeSpan HissStartDelay = TimeSpan.FromMilliseconds(250);
}
