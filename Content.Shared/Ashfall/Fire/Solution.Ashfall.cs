using Content.Shared.Chemistry.Reagent;
using Content.Shared.FixedPoint;
using Robust.Shared.IoC;
using Robust.Shared.Prototypes;

namespace Content.Shared.Chemistry.Components;

public sealed partial class Solution
{
    /// <summary>
    /// Computes the overall flammability of the solution based on its contained reagents.
    /// </summary>
    public int GetSolutionFlammability(IPrototypeManager? protoMan = null)
    {
        if (Volume <= 0)
            return 0;

        IoCManager.Resolve(ref protoMan);
        var totalFlammability = 0f;
        foreach (var (reagent, quantity) in Contents)
        {
            if (protoMan.TryIndex<ReagentPrototype>(reagent.Prototype, out var proto))
            {
                totalFlammability += proto.Flammability * (quantity.Float() / Volume.Float());
            }
        }
        return (int) MathF.Round(totalFlammability);
    }

    /// <summary>
    /// Checks whether any reagent in the solution is self-oxidizing.
    /// </summary>
    public bool IsSolutionSelfOxidizing(IPrototypeManager? protoMan = null)
    {
        if (Volume <= 0)
            return false;

        IoCManager.Resolve(ref protoMan);
        foreach (var (reagent, _) in Contents)
        {
            if (protoMan.TryIndex<ReagentPrototype>(reagent.Prototype, out var proto) && proto.SelfOxidizing)
            {
                return true;
            }
        }
        return false;
    }

    /// <summary>
    /// Burns flammable reagents in the solution proportional to the burn fraction and each reagent's flammability.
    /// </summary>
    public void BurnFlammableReagents(float fraction, IPrototypeManager? protoMan = null)
    {
        IoCManager.Resolve(ref protoMan);
        var clone = Clone();
        foreach (var (reagent, quantity) in Contents)
        {
            if (!protoMan.TryIndex<ReagentPrototype>(reagent.Prototype, out var proto) || proto.Flammability <= 0)
                continue;

            var rawBurn = MathF.Min(quantity.Float(), quantity.Float() * fraction * proto.Flammability);
            var roundedBurn = MathF.Ceiling(rawBurn * 100f) / 100f;
            if (roundedBurn <= 0f)
                continue;

            clone.RemoveReagent(reagent, FixedPoint2.New(roundedBurn));
        }
        Contents = clone.Contents;
        Volume = clone.Volume;
        _heatCapacityDirty = true;
        ValidateSolution();
    }
}
