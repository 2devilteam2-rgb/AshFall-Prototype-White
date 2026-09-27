using Content.Shared.Chemistry.Components;

namespace Content.Shared.Chemistry.EntitySystems;

public abstract partial class SharedSolutionContainerSystem
{
    /// <summary>
    /// Burns flammable reagents in a solution container and triggers container update.
    /// </summary>
    public void BurnFlammableReagents(Entity<SolutionComponent> soln, float fraction)
    {
        soln.Comp.Solution.BurnFlammableReagents(fraction, ProtoMan);
        UpdateChemicals(soln);
    }
}
