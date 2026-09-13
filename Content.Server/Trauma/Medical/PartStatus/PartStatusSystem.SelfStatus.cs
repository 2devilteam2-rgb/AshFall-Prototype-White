// SPDX-License-Identifier: AGPL-3.0-or-later

using System.Linq;
using Content.Medical.Common.Body;
using Content.Medical.Common.Traumas;
using Content.Medical.Common.Wounds;
using Content.Server.Body.Components;
using Content.Server.Temperature.Systems;
using Content.Shared.Rounding;
using Content.Shared.StatusEffectNew;
using Content.Shared.Temperature.Components;
using Content.Shared.Traits.Assorted;
using Robust.Shared.Utility;

namespace Content.Medical.Server.PartStatus;

public sealed partial class PartStatusSystem
{
    [Dependency] private StatusEffectsSystem _statusEffects = default!;

    private FormattedMessage GetSelfStatusText(EntityUid entity, HashSet<PartStatus> partStatusSet)
    {
        var message = new FormattedMessage();
        message.AddMarkupPermissive("[bold][color=#C5CAC5]" +
            Loc.GetString("inspect-part-status-title") +
            "[/color][/bold]");
        message.PushNewline();
        AddLine(message);

        var numb = _statusEffects.TryEffectsWithComp<PainNumbnessStatusEffectComponent>(entity, out _);
        var cold = HasColdAlert(entity);

        var orderedParts = BodyPartOrder
            .SelectMany(partType => partStatusSet.Where(p => p.PartType == partType)
                .OrderBy(p => _symmetryPriority.IndexOf(p.PartSymmetry)))
            .ToList();

        foreach (var partStatus in orderedParts)
        {
            if (partStatus.Missing)
            {
                message.AddMarkupPermissive(Loc.GetString("inspect-part-status-line-missing",
                    ("possessive", Loc.GetString("inspect-part-status-you")),
                    ("part", partStatus.PartName)));
                message.PushNewline();
                continue;
            }

            var condition = BuildSelfConditionDescription(partStatus);
            var tags = BuildSelfStatusTags(partStatus, numb, cold);
            var tagText = tags.Count == 0
                ? string.Empty
                : Loc.GetString("self-inspect-part-status-separator") +
                  string.Join(Loc.GetString("self-inspect-part-status-separator"), tags);
            var lineLoc = IsHealthy(partStatus) && tags.Count == 0
                ? "self-inspect-part-status-line-fine"
                : "self-inspect-part-status-line";

            message.AddMarkupPermissive(Loc.GetString(lineLoc,
                ("part", partStatus.PartName),
                ("condition", condition),
                ("tags", tagText)));
            message.PushNewline();
        }

        return message;
    }

    private string BuildSelfConditionDescription(PartStatus partStatus)
    {
        var descriptions = GetDamageGroupDescriptions(partStatus.DamageSeverities, true);
        if (descriptions.Count > 0)
            return string.Join(Loc.GetString("inspect-part-status-comma"), descriptions);

        if (partStatus.PartSeverity > WoundableSeverity.Healthy)
        {
            var severity = partStatus.PartSeverity switch
            {
                WoundableSeverity.Minor => "minor",
                WoundableSeverity.Moderate => "moderate",
                WoundableSeverity.Severe => "severe",
                WoundableSeverity.Critical => "critical",
                _ => "loss",
            };
            return Loc.GetString($"inspect-wound-{severity}");
        }

        return Loc.GetString("inspect-part-status-fine");
    }

    private List<string> BuildSelfStatusTags(PartStatus partStatus, bool numb, bool cold)
    {
        var tags = new List<string>();
        if (ShouldShowPain(partStatus, numb))
            tags.Add(Loc.GetString("self-inspect-part-status-pain"));

        if (ShouldShowMangled(partStatus))
            tags.Add(Loc.GetString("self-inspect-part-status-mangled"));

        if (partStatus.Bleeding)
            tags.Add(Loc.GetString("self-inspect-part-status-bleeding"));

        if (partStatus.BoneSeverity > BoneSeverity.Normal)
            tags.Add(Loc.GetString("self-inspect-part-status-fracture"));

        if (numb)
            tags.Add(Loc.GetString("self-inspect-part-status-numb"));

        if (cold && IsExtremity(partStatus.PartType))
            tags.Add(Loc.GetString("self-inspect-part-status-cold"));

        return tags;
    }

    internal static bool ShouldShowPain(PartStatus partStatus, bool numb)
    {
        if (numb || partStatus.Missing)
            return false;

        return partStatus.PartSeverity > WoundableSeverity.Healthy ||
               partStatus.DamageSeverities.Values.Any(severity => severity > WoundSeverity.Healed) ||
               partStatus.BoneSeverity > BoneSeverity.Normal;
    }

    internal static bool ShouldShowMangled(PartStatus partStatus)
    {
        return !partStatus.Missing &&
               (partStatus.PartSeverity >= WoundableSeverity.Critical ||
                partStatus.DamageSeverities.Values.Any(severity => severity >= WoundSeverity.Critical));
    }

    private bool HasColdAlert(EntityUid entity)
    {
        if (!TryComp<TemperatureComponent>(entity, out var temperature) ||
            !TryComp<TemperatureDamageComponent>(entity, out var thresholds))
            return false;

        float? normalBodyTemperature = TryComp<ThermalRegulatorComponent>(entity, out var regulator)
            ? regulator.NormalBodyTemperature
            : null;

        return HasColdAlertAtTemperature(
            temperature.Temperature,
            thresholds.ColdDamageThreshold,
            thresholds.HeatDamageThreshold,
            normalBodyTemperature);
    }

    internal static bool HasColdAlertAtTemperature(
        float currentTemperature,
        float coldDamageThreshold,
        float heatDamageThreshold,
        float? normalBodyTemperature)
    {
        var idealTemperature = normalBodyTemperature is {} normal &&
                               normal > coldDamageThreshold &&
                               normal < heatDamageThreshold
            ? normal
            : (coldDamageThreshold + heatDamageThreshold) / 2f;

        if (currentTemperature > idealTemperature ||
            MathF.Abs(coldDamageThreshold - idealTemperature) < 0.001f)
            return false;

        var temperatureScale = (currentTemperature - idealTemperature) /
                               (coldDamageThreshold - idealTemperature);
        var alertLevel = ContentHelpers.RoundToLevels(
            temperatureScale - TemperatureSystem.MinAlertTemperatureScale,
            1f - TemperatureSystem.MinAlertTemperatureScale,
            TemperatureSystem.MaxTemperatureAlertSeverity + 1);

        return alertLevel > 0;
    }

    internal static bool IsExtremity(BodyPartType partType)
    {
        return partType is BodyPartType.Arm or
            BodyPartType.Hand or
            BodyPartType.Leg or
            BodyPartType.Foot or
            BodyPartType.Tail or
            BodyPartType.Wings;
    }
}
