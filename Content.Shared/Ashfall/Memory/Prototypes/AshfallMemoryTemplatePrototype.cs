using Robust.Shared.Localization;
using Robust.Shared.Prototypes;

namespace Content.Shared.Ashfall.Memory.Prototypes;

/// <summary>
///     Defines a memory template that can be assigned to a pair of characters.
///     Templates may be symmetric (both sides see the same text) or asymmetric
///     (each side gets a different narrative perspective).
/// </summary>
[Prototype]
public sealed partial class AshfallMemoryTemplatePrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField(required: true)]
    public AshfallMemoryCategory Category { get; private set; }

    [DataField(required: true)]
    public AshfallMemoryTier Tier { get; private set; }

    [DataField]
    public float Weight { get; private set; } = 1f;

    [DataField(required: true)]
    public LocId SideAText { get; private set; } = string.Empty;

    [DataField(required: true)]
    public LocId SideASummary { get; private set; } = string.Empty;

    [DataField]
    public LocId? SideBText { get; private set; }

    [DataField]
    public LocId? SideBSummary { get; private set; }

    [DataField]
    public LocId? RecognitionLineA { get; private set; }

    [DataField]
    public LocId? RecognitionLineB { get; private set; }

    [DataField]
    public HashSet<string> RequiredSharedTags { get; private set; } = new();

    [DataField]
    public HashSet<string> RequiredAnyTags { get; private set; } = new();

    [DataField]
    public HashSet<string> ExcludedTags { get; private set; } = new();

    [DataField]
    public HashSet<string> SideARequiredTags { get; private set; } = new();

    [DataField]
    public HashSet<string> SideBRequiredTags { get; private set; } = new();
}
