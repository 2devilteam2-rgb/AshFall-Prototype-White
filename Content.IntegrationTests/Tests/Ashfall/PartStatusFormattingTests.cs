using System.Collections.Generic;
using Content.Medical.Common.Body;
using Content.Medical.Common.Traumas;
using Content.Medical.Common.Wounds;
using Content.Medical.Server.PartStatus;

namespace Content.IntegrationTests.Tests.Ashfall;

[TestFixture]
public sealed class PartStatusFormattingTests
{
    [Test]
    public void ClassifiesPartSymptoms()
    {
        var healthy = new PartStatus(
            BodyPartType.Head,
            BodyPartSymmetry.None,
            "head",
            WoundableSeverity.Healthy,
            new Dictionary<string, WoundSeverity>(),
            BoneSeverity.Normal,
            false);
        Assert.That(PartStatusSystem.ShouldShowPain(healthy, numb: false), Is.False);
        Assert.That(PartStatusSystem.ShouldShowMangled(healthy), Is.False);

        var injured = new PartStatus(
            BodyPartType.Arm,
            BodyPartSymmetry.Left,
            "left arm",
            WoundableSeverity.Moderate,
            new Dictionary<string, WoundSeverity> { ["Brute"] = WoundSeverity.Moderate },
            BoneSeverity.Broken,
            true);
        Assert.That(PartStatusSystem.ShouldShowPain(injured, numb: false), Is.True);
        Assert.That(PartStatusSystem.ShouldShowPain(injured, numb: true), Is.False);
        Assert.That(PartStatusSystem.ShouldShowMangled(injured), Is.False);

        injured.PartSeverity = WoundableSeverity.Mangled;
        Assert.That(PartStatusSystem.ShouldShowMangled(injured), Is.True);
        Assert.That(PartStatusSystem.IsExtremity(injured.PartType), Is.True);
        Assert.That(PartStatusSystem.IsExtremity(BodyPartType.Torso), Is.False);
    }

    [Test]
    public void UsesTemperatureAlertThresholds()
    {
        Assert.That(PartStatusSystem.HasColdAlertAtTemperature(300f, 273.15f, 360f, 310.15f), Is.False);
        Assert.That(PartStatusSystem.HasColdAlertAtTemperature(290f, 273.15f, 360f, 310.15f), Is.True);

        Assert.That(PartStatusSystem.HasColdAlertAtTemperature(295f, 230f, 360f, 310.15f), Is.False);
        Assert.That(PartStatusSystem.HasColdAlertAtTemperature(270f, 230f, 360f, 310.15f), Is.True);
    }
}
