// SPDX-License-Identifier: AGPL-3.0-or-later

using Robust.Shared.GameStates;

namespace Content.Shared.Ashfall.Audio;

/// <summary>
/// Marks clothing (helmets, earmuffs, headsets) or entities as protected from deafening muzzle blasts and tinnitus.
/// </summary>
[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class EarProtectionComponent : Component
{
    /// <summary>
    /// Protection level from 0.0 to 1.0.
    /// 1.0 means full immunity to gunfire deafening.
    /// </summary>
    [DataField, AutoNetworkedField]
    public float Protection = 1.0f;
}
