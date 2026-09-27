using Robust.Shared.Configuration;

namespace Content.Shared.Ashfall.Fire;

/// <summary>
/// Configuration variables for Ashfall's fire simulation (Reagent fires and Solid Fuel combustion).
/// </summary>
[CVarDefs]
public sealed class AshfallFireCVars
{
    /*
     * Reagent Puddle Fires
     */

    /// <summary>
    /// Multiplier for the structural and heat damage dealt by reagent puddle fires.
    /// </summary>
    public static readonly CVarDef<float> PuddleFireDamageMultiplier =
        CVarDef.Create("ashfall.fire.puddle_damage_multiplier", 1.0f, CVar.SERVERONLY);

    /// <summary>
    /// Multiplier for effectiveness of fire protection from equipment.
    /// </summary>
    public static readonly CVarDef<float> FireProtectionEffectiveness =
        CVarDef.Create("ashfall.fire.fire_protection_effectiveness", 1.0f, CVar.SERVERONLY);

    /// <summary>
    /// Whether puddle volume scales down effective fire intensity for small amounts of liquid.
    /// </summary>
    public static readonly CVarDef<bool> VolumeScalingEnabled =
        CVarDef.Create("ashfall.fire.volume_scaling_enabled", true, CVar.SERVERONLY);

    /// <summary>
    /// The solution volume in units at which fire intensity is considered full strength. Puddles below this volume burn proportionally weaker.
    /// </summary>
    public static readonly CVarDef<float> VolumeScalingReference =
        CVarDef.Create("ashfall.fire.volume_scaling_reference", 20f, CVar.SERVERONLY);

    /// <summary>
    /// Exponent applied to the volume ratio. Higher values punish small puddles harder. 1.0 is linear falloff.
    /// </summary>
    public static readonly CVarDef<float> VolumeScalingCurve =
        CVarDef.Create("ashfall.fire.volume_scaling_curve", 1.5f, CVar.SERVERONLY);

    /// <summary>
    /// Solution volume in units below which puddles burn out rapidly instead of following the normal rate.
    /// </summary>
    public static readonly CVarDef<float> SmallPuddleBurnThreshold =
        CVarDef.Create("ashfall.fire.small_puddle_burn_threshold", 5.0f, CVar.SERVERONLY);

    /// <summary>
    /// Percentage of remaining volume consumed per second once a puddle is below the above threshold.
    /// </summary>
    public static readonly CVarDef<float> SmallPuddleBurnPercent =
        CVarDef.Create("ashfall.fire.small_puddle_burn_percent", 0.5f, CVar.SERVERONLY);

    /// <summary>
    /// Whether footprints created from stepping in flammable puddles can catch fire.
    /// </summary>
    public static readonly CVarDef<bool> FlammableFootprintsEnabled =
        CVarDef.Create("ashfall.fire.flammable_footprints_enabled", true, CVar.SERVERONLY);

    /*
     * Solid Fuel Combustion
     */

    /// <summary>
    /// Whether solid material burning is enabled.
    /// </summary>
    public static readonly CVarDef<bool> SolidFuelEnabled =
        CVarDef.Create("ashfall.fire.solid_fuel_enabled", true, CVar.SERVERONLY);

    /// <summary>
    /// Global multiplier for solid fuel heating/ignition accumulation rate.
    /// </summary>
    public static readonly CVarDef<float> SolidFuelIgnitionMultiplier =
        CVarDef.Create("ashfall.fire.solid_fuel_ignition_multiplier", 1f, CVar.SERVERONLY);

    /// <summary>
    /// Global multiplier for solid fuel burn consumption speed.
    /// </summary>
    public static readonly CVarDef<float> SolidFuelBurnMultiplier =
        CVarDef.Create("ashfall.fire.solid_fuel_burn_multiplier", 1f, CVar.SERVERONLY);

    /// <summary>
    /// Whether solid fuel fires spread heat to nearby flammable objects and floors.
    /// </summary>
    public static readonly CVarDef<bool> SolidFuelSpread =
        CVarDef.Create("ashfall.fire.solid_fuel_spread", true, CVar.SERVERONLY);

    /// <summary>
    /// Heat contact range for non-burning ignition sources.
    /// </summary>
    public static readonly CVarDef<float> SolidFuelContactRange =
        CVarDef.Create("ashfall.fire.solid_fuel_contact_range", 0.6f, CVar.SERVERONLY);

    /// <summary>
    /// Heat radiation range for burning solid fuel objects.
    /// </summary>
    public static readonly CVarDef<float> SolidFuelSpreadRange =
        CVarDef.Create("ashfall.fire.solid_fuel_spread_range", 1.1f, CVar.SERVERONLY);

    /// <summary>
    /// Ignition rate provided by open flames.
    /// </summary>
    public static readonly CVarDef<float> SolidFuelFireRate =
        CVarDef.Create("ashfall.fire.solid_fuel_fire_rate", 20f, CVar.SERVERONLY);

    /// <summary>
    /// Ignition rate provided by lit cigarettes.
    /// </summary>
    public static readonly CVarDef<float> SolidFuelCigaretteRate =
        CVarDef.Create("ashfall.fire.solid_fuel_cigarette_rate", 1f, CVar.SERVERONLY);

    /// <summary>
    /// Whether screenshake effects are disabled for the client.
    /// </summary>
    public static readonly CVarDef<bool> ScreenshakeDisabled =
        CVarDef.Create("ashfall.camera.screenshake_disabled", false, CVar.CLIENTONLY | CVar.ARCHIVE);
}
