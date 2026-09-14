using Content.IntegrationTests.Fixtures;
using Content.Shared.CombatMode;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Movement.Pulling.Components;
using Content.Shared.Movement.Pulling.Systems;
using Content.Shared.Speech;
using Content.Shared.Standing;
using Content.Trauma.Common.MartialArts;
using Content.Trauma.Shared.MartialArts.Components;
using Robust.Shared.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.Physics;
using Robust.Shared.Physics.Dynamics.Joints;

namespace Content.IntegrationTests.Tests.Ashfall;

public sealed class GrabStateTests : GameTest
{
    private (Entity<PullerComponent> Puller, Entity<PullableComponent> Target) StartPull(EntityCoordinates coords)
    {
        var puller = SSpawnAtPosition("MobHuman", coords);
        var target = SSpawnAtPosition("MobHuman", coords.Offset(new System.Numerics.Vector2(0.75f, 0)));
        Assert.That(SEntMan.System<PullingSystem>().TryStartPull(puller, target), Is.True);
        return ((puller, SComp<PullerComponent>(puller)), (target, SComp<PullableComponent>(target)));
    }

    [Test]
    public async Task StageEffectsDoNotGenerateComboAttacks()
    {
        var map = await Pair.CreateTestMap();
        await Server.WaitAssertion(() =>
        {
            var (puller, target) = StartPull(map.GridCoords);
            var combo = SEntMan.EnsureComponent<CanPerformComboComponent>(puller);
            combo.AllowedCombos.Clear();
            combo.LastAttacks.Clear();
            var pulling = SEntMan.System<PullingSystem>();

            Assert.That(pulling.TrySetGrabStages(puller, target, GrabStage.Suffocate), Is.True);
            Assert.That(combo.LastAttacks, Is.Empty,
                "A combo's grab-stage effect must not feed a new attack back into combo detection.");
            Assert.That(pulling.TrySetGrabStages(puller, target, GrabStage.Suffocate), Is.True);
            Assert.That(combo.LastAttacks, Is.Empty);
        });
    }

    [Test]
    public async Task PlayerGrabStillAdvancesCombos()
    {
        var map = await Pair.CreateTestMap();
        await Server.WaitAssertion(() =>
        {
            var (puller, target) = StartPull(map.GridCoords);
            var combo = SEntMan.EnsureComponent<CanPerformComboComponent>(puller);
            combo.AllowedCombos.Clear();
            combo.LastAttacks.Clear();
            SEntMan.System<SharedCombatModeSystem>().SetInCombatMode(puller, true);

            Assert.That(SEntMan.System<PullingSystem>().TogglePull(target, puller.Owner), Is.True);
            Assert.That(combo.LastAttacks, Is.EqualTo(new[] { ComboAttackType.Grab }));
        });
    }

    [TestCase(false)]
    [TestCase(true)]
    public async Task JudoArmbarCompletesAndReleaseRestoresSpeech(bool queued)
    {
        var map = await Pair.CreateTestMap();
        await Server.WaitAssertion(() =>
        {
            var (puller, target) = StartPull(map.GridCoords);
            Assert.That(SEntMan.System<StandingStateSystem>().Down(target), Is.True);
            var combo = SEntMan.EnsureComponent<CanPerformComboComponent>(puller);
            combo.AllowedCombos.Clear();
            combo.AllowedCombos.Add(Server.ProtoMan.Index<Content.Trauma.Shared.MartialArts.ComboPrototype>("JudoArmbar"));
            combo.LastAttacks.Clear();
            combo.LastAttacks.Add(ComboAttackType.Disarm);
            combo.LastAttacks.Add(ComboAttackType.Disarm);
            if (queued)
                SEntMan.EnsureComponent<ComboActionsComponent>(puller).QueuedPrototype = "JudoArmbar";

            var grab = new ComboAttackPerformedEvent(puller, target, puller, ComboAttackType.Grab);
            SEntMan.EventBus.RaiseLocalEvent(puller, ref grab);
            Assert.That(SEntMan.HasComponent<ArmbarredComponent>(target), Is.True);
            Assert.That(target.Comp.GrabStage, Is.EqualTo(GrabStage.Suffocate));
            Assert.That(combo.LastAttacks, Is.Empty);
            var speech = new SpeakAttemptEvent(target);
            SEntMan.EventBus.RaiseLocalEvent(target, speech);
            Assert.That(speech.Cancelled, Is.True);

            Assert.That(SEntMan.System<PullingSystem>().TryStopPull(target, target.Comp, ignoreGrab: true), Is.True);
            speech = new SpeakAttemptEvent(target);
            SEntMan.EventBus.RaiseLocalEvent(target, speech);
            Assert.That(speech.Cancelled, Is.False);
            if (queued)
                Assert.That(SComp<ComboActionsComponent>(puller).QueuedPrototype, Is.Null);
        });
    }

    [Test]
    public async Task LoweringChokeholdPreservesPull()
    {
        var map = await Pair.CreateTestMap();
        Entity<PullerComponent> puller = default;
        Entity<PullableComponent> target = default;
        await Server.WaitAssertion(() =>
        {
            (puller, target) = StartPull(map.GridCoords);
            var pulling = SEntMan.System<PullingSystem>();
            Assert.That(pulling.TrySetGrabStages(puller, target, GrabStage.Suffocate), Is.True);
            Assert.That(pulling.TryLowerGrabStage(target, puller, ignoreCombatMode: true), Is.True);
            Assert.That(puller.Comp.Pulling, Is.EqualTo(target.Owner));
            Assert.That(target.Comp.Puller, Is.EqualTo(puller.Owner));
            Assert.That(target.Comp.GrabStage, Is.EqualTo(GrabStage.Hard));
        });
        await Pair.RunTicksSync(2);
        await Server.WaitAssertion(() =>
        {
            Assert.That(puller.Comp.Pulling, Is.EqualTo(target.Owner));
            Assert.That(SEntMan.System<SharedHandsSystem>().CountFreeHands(puller.Owner), Is.EqualTo(1));
        });
    }

    [Test]
    public async Task FailedChokeholdDoesNotTightenPullJoint()
    {
        var map = await Pair.CreateTestMap();
        await Server.WaitAssertion(() =>
        {
            var (puller, target) = StartPull(map.GridCoords);
            var pulling = SEntMan.System<PullingSystem>();
            Assert.That(pulling.TrySetGrabStages(puller, target, GrabStage.Hard), Is.True);
            var item = SSpawnAtPosition("Crowbar", map.GridCoords);
            Assert.That(SEntMan.System<SharedHandsSystem>().TryPickupAnyHand(puller.Owner, item), Is.True);
            var joint = (DistanceJoint) SComp<JointComponent>(target).GetJoints[target.Comp.PullJointId!];
            var length = joint.MaxLength;

            Assert.That(pulling.TrySetGrabStages(puller, target, GrabStage.Suffocate), Is.False);
            Assert.That(puller.Comp.GrabStage, Is.EqualTo(GrabStage.Hard));
            Assert.That(target.Comp.GrabStage, Is.EqualTo(GrabStage.Hard));
            Assert.That(joint.MaxLength, Is.EqualTo(length));
        });
    }
}
