using Robust.Shared.Localization;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;

namespace Content.Shared.Ashfall.CharacterGen.Prototypes;

/// <summary>
///     Core psychotype archetype for procedural Ashfall character generation.
///     Biases the selection of demeanor, stress, and quirk trait slots.
/// </summary>
[Prototype]
public sealed partial class AshfallPsychotypePrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField(required: true)]
    public LocId Name { get; private set; } = string.Empty;

    [DataField]
    public string Tag { get; private set; } = string.Empty;

    [DataField]
    public float Weight { get; private set; } = 1f;

    [DataField]
    public List<AshfallTagWeightModifier> WeightModifiers { get; private set; } = new();

    [DataField]
    public HashSet<string> RequiredTags { get; private set; } = new();

    [DataField]
    public HashSet<string> ExcludedTags { get; private set; } = new();
}
