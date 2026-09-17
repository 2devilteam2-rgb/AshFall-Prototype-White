// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.GameStates;

namespace Content.Shared.Ashfall.Combat;

/// <summary>
/// Attached to entities to track combat suppression from incoming near-miss fire.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class SuppressionComponent : Component
{
    /// <summary>
    /// Current suppression intensity from 0.0 to 1.0.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float Level;

    /// <summary>
    /// Rate at which suppression decays per second.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float DecayRate = 0.50f;
}
