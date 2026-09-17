namespace Content.Shared.Ashfall.Memory;

/// <summary>
///     Controls which discovery triggers can reveal a memory.
/// </summary>
public enum AshfallMemoryTier : byte
{
    /// <summary>
    ///     Obvious recognition — triggered by proximity or hearing the other person speak.
    /// </summary>
    Passive,

    /// <summary>
    ///     Deeper recall — triggered only by examining (Shift+Click) the other person.
    /// </summary>
    Active,
}
