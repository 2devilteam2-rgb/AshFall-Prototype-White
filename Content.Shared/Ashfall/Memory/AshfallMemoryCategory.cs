namespace Content.Shared.Ashfall.Memory;

/// <summary>
///     Categories for inter-character memory templates.
///     Combines tone (positive/negative/neutral/complicated) with situation context
///     (workplace/personal/acquaintance) to drive recognition flash line pool selection.
/// </summary>
public enum AshfallMemoryCategory : byte
{
    PositiveWorkplace,
    PositivePersonal,
    NegativeWorkplace,
    NegativePersonal,
    NeutralAcquaintance,
    ComplicatedHistory,
}
