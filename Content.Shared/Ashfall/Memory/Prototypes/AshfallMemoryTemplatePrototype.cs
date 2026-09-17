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

    // ── Side A text ──────────────────────────────────────────────

    /// <summary>Full narrative text shown to the character assigned as side A.</summary>
    [DataField(required: true)]
    public LocId SideAText { get; private set; } = string.Empty;

    /// <summary>Brief one-liner for the examine tooltip (side A).</summary>
    [DataField(required: true)]
    public LocId SideASummary { get; private set; } = string.Empty;

    // ── Side B text (optional — null means symmetric) ────────────

    /// <summary>Full narrative text for side B. If null, falls back to <see cref="SideAText"/>.</summary>
    [DataField]
    public LocId? SideBText { get; private set; }

    /// <summary>Brief tooltip text for side B. If null, falls back to <see cref="SideASummary"/>.</summary>
    [DataField]
    public LocId? SideBSummary { get; private set; }

    // ── Recognition flash overrides ──────────────────────────────

    /// <summary>
    ///     Custom recognition line for side A. If null, a random line is picked from
    ///     the category's <see cref="AshfallRecognitionPoolPrototype"/>.
    /// </summary>
    [DataField]
    public LocId? RecognitionLineA { get; private set; }

    /// <summary>Custom recognition line for side B.</summary>
    [DataField]
    public LocId? RecognitionLineB { get; private set; }

    // ── Matching conditions ──────────────────────────────────────

    /// <summary>Tags that BOTH characters in the pair must have.</summary>
    [DataField]
    public HashSet<string> RequiredSharedTags { get; private set; } = new();

    /// <summary>At least one character in the pair must have one of these tags.</summary>
    [DataField]
    public HashSet<string> RequiredAnyTags { get; private set; } = new();

    /// <summary>Neither character may have any of these tags.</summary>
    [DataField]
    public HashSet<string> ExcludedTags { get; private set; } = new();

    /// <summary>The character assigned as side A must have all of these tags.</summary>
    [DataField]
    public HashSet<string> SideARequiredTags { get; private set; } = new();

    /// <summary>The character assigned as side B must have all of these tags.</summary>
    [DataField]
    public HashSet<string> SideBRequiredTags { get; private set; } = new();
}
