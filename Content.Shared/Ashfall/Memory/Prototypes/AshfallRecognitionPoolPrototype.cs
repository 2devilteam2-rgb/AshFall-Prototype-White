using Robust.Shared.Localization;
using Robust.Shared.Prototypes;

namespace Content.Shared.Ashfall.Memory.Prototypes;

/// <summary>
///     A pool of recognition flash lines for a specific memory category.
///     When a memory is discovered and the template does not override the recognition line,
///     a random line is picked from the matching pool.
/// </summary>
[Prototype]
public sealed partial class AshfallRecognitionPoolPrototype : IPrototype
{
    [IdDataField]
    public string ID { get; private set; } = default!;

    [DataField(required: true)]
    public AshfallMemoryCategory Category { get; private set; }

    [DataField(required: true)]
    public List<LocId> Lines { get; private set; } = new();
}
