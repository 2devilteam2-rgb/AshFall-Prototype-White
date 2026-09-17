using Robust.Shared.GameObjects;

namespace Content.Shared.Ashfall.Memory;

/// <summary>
///     Marks an entity as a participant in the inter-character memory system.
///     Stores matching tags derived from the character's background for template filtering.
///     All memory link state lives in the server-side <c>AshfallMemorySystem</c> registry,
///     not in this component.
/// </summary>
[RegisterComponent]
public sealed partial class CharacterMemoryComponent : Component
{
    /// <summary>
    ///     Tags derived from the character's generated background, used to filter which
    ///     memory templates are eligible for a given pair. Populated at spawn time from
    ///     the character pool's stored structure tags plus derived tags (species, age
    ///     bracket, primary domain, culture).
    /// </summary>
    [ViewVariables]
    public HashSet<string> MatchingTags = new();
}
