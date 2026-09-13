using System.Linq;
using Content.IntegrationTests.Fixtures;
using Content.Shared.Damage;
using Content.Shared.Damage.Components;
using Content.Shared.Damage.Systems;
using Content.Shared.DoAfter;
using Content.Shared.FixedPoint;
using Content.Shared.Hands.EntitySystems;
using Content.Shared.Interaction;
using Content.Shared.Repairable;
using Robust.Shared.GameObjects;
using Robust.Shared.Map;
using Robust.Shared.Maths;

namespace Content.IntegrationTests.Tests.Ashfall;

/// <summary>
/// Checks the damage-scaled repair delays: the do-after delay for repairing someone else is
/// multiplied by the clamped ratio of the target's positive damage, while self-repair keeps
/// its own flat penalty. Undamaged targets are rejected outright.
/// </summary>
[TestFixture]
[TestOf(typeof(RepairableSystem))]
public sealed class AshfallRepairableTests : GameTest
{
    private static DamageSpecifier BluntDamage(float amount)
        => new() { DamageDict = new() { { "Blunt", FixedPoint2.New(amount) } } };

    [TestPrototypes]
    private const string Prototypes = @"
- type: entity
  id: AshfallRepairTestTarget
  name: repair test target
  components:
  - type: Damageable
  - type: Injurable
    damageContainer: Inorganic
  - type: Repairable
    fuelCost: 0
    doAfterDelay: 1

- type: entity
  id: AshfallRepairTestTool
  name: repair test tool
  components:
  - type: Item
  - type: Tool
    qualities:
    - Welding
";

    /// <summary>
    /// Builds a small plating grid on a fresh test map.
    /// </summary>
    private async Task<EntityUid> SetupGrid()
    {
        var testMap = await Pair.CreateTestMap();
        var mapId = testMap.MapId;
        EntityUid grid = default;

        await Server.WaitPost(() =>
        {
            var maps = SEntMan.System<SharedMapSystem>();
            var gridEnt = maps.CreateGridEntity(mapId);
            grid = gridEnt.Owner;
            var plating = new Tile(Server.ResolveDependency<ITileDefinitionManager>()["Plating"].TileId);
            for (var x = 0; x < 4; x++)
            {
                for (var y = 0; y < 4; y++)
                    maps.SetTile(gridEnt, gridEnt, new Vector2i(x, y), plating);
            }
        });

        return grid;
    }

    /// <summary>
    /// Spawns a user with hands, a welding tool and a repairable target on the grid.
    /// </summary>
    private async Task<(EntityUid User, EntityUid Tool, EntityUid Target)> SpawnScene(EntityUid grid, float? damage = null)
    {
        EntityUid user = default;
        EntityUid tool = default;
        EntityUid target = default;

        await Server.WaitPost(() =>
        {
            user = SSpawnAtPosition("MobHuman", new EntityCoordinates(grid, 0.5f, 0.5f));
            tool = SSpawnAtPosition("AshfallRepairTestTool", new EntityCoordinates(grid, 0.5f, 1.5f));
            target = SSpawnAtPosition("AshfallRepairTestTarget", new EntityCoordinates(grid, 1.5f, 0.5f));

            Assert.That(SEntMan.System<SharedHandsSystem>().TryPickupAnyHand(user, tool, animate: false), Is.True,
                "The test welding tool must be held for the tool-use do-after.");
            Assert.That(SEntMan.System<SharedHandsSystem>().GetActiveItem(user), Is.EqualTo(tool),
                "The test welding tool must be in the active hand.");

            if (damage is { } amount)
            {
                SEntMan.System<DamageableSystem>().TryChangeDamage(target, BluntDamage(amount), ignoreResistances: true);
                Assert.That(SEntMan.System<DamageableSystem>().GetTotalDamage(target), Is.EqualTo(FixedPoint2.New(amount)),
                    "The repair target must have the requested test damage.");
            }
        });

        return (user, tool, target);
    }

    private InteractUsingEvent StartRepairInteraction(EntityUid user, EntityUid tool, EntityUid target)
    {
        var interaction = SEntMan.System<SharedInteractionSystem>();
        Assert.That(interaction.InRangeAndAccessible(user, target), Is.True,
            "The repair target must be reachable in the test scene.");
        Assert.That(interaction.InRangeUnobstructed(user, tool), Is.True,
            "The held repair tool must remain reachable from its user.");

        var clickLocation = SEntMan.GetComponent<TransformComponent>(target).Coordinates;
        var ev = new InteractUsingEvent(user, tool, target, clickLocation);
        SEntMan.EventBus.RaiseLocalEvent(target, ev);
        return ev;
    }

    private Content.Shared.DoAfter.DoAfter GetSingleDoAfter(EntityUid user)
    {
        var comp = SComp<DoAfterComponent>(user);
        var system = SEntMan.System<SharedDoAfterSystem>();
        var running = comp.DoAfters.Values.Where(doAfter => system.IsRunning(doAfter.Id)).ToList();
        Assert.That(running, Has.Count.EqualTo(1), "Expected exactly one running repair do-after on the user.");
        return running.Single();
    }

    [Test]
    public async Task RepairDelayScalesWithTargetDamage()
    {
        var grid = await SetupGrid();

        var (slightUser, slightTool, slightTarget) = await SpawnScene(grid, 25f);
        await Server.WaitPost(() => StartRepairInteraction(slightUser, slightTool, slightTarget));
        Assert.That(GetSingleDoAfter(slightUser).Args.Delay, Is.EqualTo(TimeSpan.FromSeconds(0.5)),
            "Slight damage (25 of 100) must hit the 0.5x clamp floor.");

        var (severeUser, severeTool, severeTarget) = await SpawnScene(grid, 300f);
        await Server.WaitPost(() => StartRepairInteraction(severeUser, severeTool, severeTarget));
        Assert.That(GetSingleDoAfter(severeUser).Args.Delay, Is.EqualTo(TimeSpan.FromSeconds(3)),
            "Severe damage (300 of 100) must hit the 3x clamp ceiling.");

        var (moderateUser, moderateTool, moderateTarget) = await SpawnScene(grid, 100f);
        await Server.WaitPost(() => StartRepairInteraction(moderateUser, moderateTool, moderateTarget));
        Assert.That(GetSingleDoAfter(moderateUser).Args.Delay, Is.EqualTo(TimeSpan.FromSeconds(1)),
            "Damage equal to 100 must produce the base delay unmodified.");
    }

    [Test]
    public async Task SelfRepairKeepsFlatPenalty()
    {
        var grid = await SetupGrid();
        var (user, tool, _) = await SpawnScene(grid);

        await Server.WaitPost(() =>
        {
            // The user repairs itself, so it needs its own Repairable component.
            var repairable = SEntMan.AddComponent<RepairableComponent>(user);
            repairable.FuelCost = 0;
            repairable.DoAfterDelay = 1;

            SEntMan.System<DamageableSystem>().TryChangeDamage(user, BluntDamage(25f), ignoreResistances: true);
            StartRepairInteraction(user, tool, user);
        });

        // 25 damage would clamp the delay to 0.5x; the self-repair penalty must not be scaled by it.
        Assert.That(GetSingleDoAfter(user).Args.Delay, Is.EqualTo(TimeSpan.FromSeconds(3)),
            "Self-repair delay must only be multiplied by the flat self-repair penalty.");
    }

    [Test]
    public async Task UndamagedTargetIsRejected()
    {
        var grid = await SetupGrid();
        var (user, tool, target) = await SpawnScene(grid);

        await Server.WaitPost(() =>
        {
            var ev = StartRepairInteraction(user, tool, target);
            Assert.That(ev.Handled, Is.False, "Repairing an undamaged target must not handle the interaction.");
            Assert.That(SComp<DoAfterComponent>(user).DoAfters, Is.Empty,
                "No do-after may start for an undamaged target.");
        });
    }

    [Test]
    public async Task RepairCompletesAndCanBeRepeated()
    {
        var grid = await SetupGrid();
        var (user, tool, target) = await SpawnScene(grid, 50f);
        var doAfterSystem = SEntMan.System<SharedDoAfterSystem>();
        var damageable = SEntMan.System<DamageableSystem>();

        await Server.WaitPost(() => StartRepairInteraction(user, tool, target));
        Assert.That(GetSingleDoAfter(user).Args.Delay, Is.EqualTo(TimeSpan.FromSeconds(0.5)));

        await RunSeconds(1.5f);
        Assert.That(damageable.GetTotalDamage(target), Is.EqualTo(FixedPoint2.Zero), "Repair did not clear the damage.");

        // The same target must accept a fresh repair after being damaged again.
        await Server.WaitPost(() =>
        {
            SEntMan.System<DamageableSystem>().TryChangeDamage(target, BluntDamage(50f), ignoreResistances: true);
            StartRepairInteraction(user, tool, target);
        });
        var second = GetSingleDoAfter(user);
        Assert.That(doAfterSystem.IsRunning(second.Id), Is.True, "A repeated repair after completion must start a new do-after.");

        await RunSeconds(1.5f);
        Assert.That(damageable.GetTotalDamage(target), Is.EqualTo(FixedPoint2.Zero), "Repeated repair did not clear the damage.");
    }

    [Test]
    public async Task CancelledRepairKeepsDamageAndAllowsRestart()
    {
        var grid = await SetupGrid();
        var (user, tool, target) = await SpawnScene(grid, 50f);
        var doAfterSystem = SEntMan.System<SharedDoAfterSystem>();
        var damageable = SEntMan.System<DamageableSystem>();

        await Server.WaitPost(() => StartRepairInteraction(user, tool, target));
        var doAfter = GetSingleDoAfter(user);
        var damageBefore = damageable.GetTotalDamage(target);

        await Server.WaitPost(() => doAfterSystem.Cancel(doAfter.Id));
        await RunTicksSync(5);

        Assert.That(damageable.GetTotalDamage(target), Is.EqualTo(damageBefore),
            "Cancelling the repair must not change the damage.");
        Assert.That(doAfterSystem.IsRunning(doAfter.Id), Is.False);

        await Server.WaitPost(() => StartRepairInteraction(user, tool, target));
        Assert.That(doAfterSystem.IsRunning(GetSingleDoAfter(user).Id), Is.True,
            "A repair must be startable again after a cancellation.");
    }
}
