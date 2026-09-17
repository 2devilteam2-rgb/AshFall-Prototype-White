// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.GameStates;

namespace Content.Shared.Ashfall.Audio;

/// <summary>
/// Added to entities experiencing acute hearing loss / tinnitus from gunfire or loud explosions.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class DeafenedComponent : Component
{
    /// <summary>
    /// Game time when the deafening effect expires.
    /// </summary>
    [DataField, AutoNetworkedField]
    public TimeSpan EndTime = TimeSpan.Zero;

    /// <summary>
    /// Total duration of this deafness instance.
    /// </summary>
    [DataField, AutoNetworkedField]
    public TimeSpan TotalDuration = TimeSpan.Zero;

    /// <summary>
    /// Whether the client has started the tinnitus audio effect for this instance.
    /// </summary>
    public bool AudioStarted;
}
