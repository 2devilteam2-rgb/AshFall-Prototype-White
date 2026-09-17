// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.GameStates;

namespace Content.Shared.Ashfall.Combat.Concussion;

/// <summary>
/// Placed on helmets, hardsuits, masks or other clothing to resist concussive blast trauma.
/// </summary>
[RegisterComponent, NetworkedComponent]
public sealed partial class ConcussionProtectionComponent : Component
{
    /// <summary>
    /// Concussion reduction modifier (0.0 to 1.0). 0.5 means 50% concussion damage reduction.
    /// </summary>
    [DataField]
    public float Protection = 0.5f;
}
