using Content.IntegrationTests.Fixtures;
using Content.Server.Ashfall.Carrying;
using Content.Server.Ashfall.Interaction.OfferItem;
using Content.Shared.ActionBlocker;
using Content.Shared.Ashfall.Interaction.OfferItem;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Interaction;
using Content.Shared.Timing.Components;
using Content.Shared.Timing.Systems;
using Robust.Shared.GameObjects;

namespace Content.IntegrationTests.Tests.Ashfall;

public sealed class InteractionStateTests : GameTest
{
    [TestCase(false)]
    [TestCase(true)]
    public async Task BusyGiverCannotReceiveAnotherOffer(bool hasRecipient)
    {
        var map = await Pair.CreateTestMap();
        await Server.WaitAssertion(() =>
        {
            var first = SSpawnAtPosition("MobHuman", map.GridCoords);
            var second = SSpawnAtPosition("MobHuman", map.GridCoords);
            var third = SSpawnAtPosition("MobHuman", map.GridCoords);
            var firstItem = SSpawnAtPosition("Crowbar", map.GridCoords);
            var secondItem = SSpawnAtPosition("Crowbar", map.GridCoords);
            var hands = SEntMan.System<SharedHandsSystem>();
            var offers = SEntMan.System<OfferItemSystem>();
            Assert.That(hands.TryPickupAnyHand(first, firstItem), Is.True);
            Assert.That(hands.TryPickupAnyHand(second, secondItem), Is.True);
            Assert.That(offers.TryStartOffer(first), Is.True);

            if (hasRecipient)
            {
                var initial = new InteractUsingEvent(first, firstItem, third, map.GridCoords);
                SEntMan.EventBus.RaiseLocalEvent(third, initial);
                Assert.That(initial.Handled, Is.True);
            }

            Assert.That(offers.TryStartOffer(second), Is.True);
            var incoming = new InteractUsingEvent(second, secondItem, first, map.GridCoords);
            SEntMan.EventBus.RaiseLocalEvent(first, incoming);

            var firstOffer = SComp<OfferItemComponent>(first);
            Assert.That(firstOffer.IsInReceiveMode, Is.False);
            Assert.That(firstOffer.IsInOfferMode, Is.EqualTo(!hasRecipient));
            Assert.That(firstOffer.Item, Is.EqualTo(firstItem));
            Assert.That(firstOffer.Target, Is.EqualTo(hasRecipient ? (EntityUid?) third : null));
            Assert.That(SComp<OfferItemComponent>(second).Target, Is.Null);

            if (hasRecipient)
            {
                offers.Receive((third, SComp<OfferItemComponent>(third)));
                Assert.That(hands.GetActiveItem(third), Is.EqualTo(firstItem));
            }
        });
    }

    [Test]
    public async Task CarryingUpdatesMovementOnPickupAndDrop()
    {
        var map = await Pair.CreateTestMap();
        await Server.WaitAssertion(() =>
        {
            var carrier = SSpawnAtPosition("MobHuman", map.GridCoords);
            var target = SSpawnAtPosition("MobHuman", map.GridCoords);
            var carrying = SEntMan.System<CarryingSystem>();
            var blocker = SEntMan.System<ActionBlockerSystem>();
            Assert.That(blocker.CanMove(target), Is.True);
            Assert.That(carrying.TryCarry(carrier, target), Is.True);
            Assert.That(blocker.CanMove(target), Is.False);
            carrying.DropCarried(carrier);
            Assert.That(blocker.CanMove(target), Is.True);
        });
    }

    [Test]
    public async Task CancelledAndExpiredUseDelaysAreImmediatelyInactive()
    {
        await Server.WaitAssertion(() =>
        {
            var item = SSpawn("Crowbar");
            var delays = SEntMan.System<UseDelaySystem>();
            delays.SetLength(item, TimeSpan.FromSeconds(5));
            var component = SComp<UseDelayComponent>(item);
            Assert.That(delays.TryResetDelay(item), Is.True);
            Assert.That(delays.IsDelayed(item), Is.True);
            Assert.That(delays.CancelDelay((item, component)), Is.True);
            Assert.That(delays.IsDelayed(item), Is.False);
            Assert.That(delays.TryResetDelay(item, checkDelayed: true), Is.True);

            delays.SetLength(item, TimeSpan.Zero);
            Assert.That(delays.TryResetDelay(item), Is.True);
            Assert.That(delays.IsDelayed(item), Is.False);
        });
    }
}
