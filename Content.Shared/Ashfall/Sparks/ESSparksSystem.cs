using Content.Shared.Ashfall.Physics.PreventCollide;
using Content.Shared.Ashfall.Sparks.Components;
using Content.Shared.Power.Components;
using Content.Shared.Power.EntitySystems;
using Content.Shared.Throwing;
using JetBrains.Annotations;
using Robust.Shared.Map;
using Robust.Shared.Network;
using Robust.Shared.Prototypes;
using Robust.Shared.Random;
using Robust.Shared.Timing;

namespace Content.Shared.Ashfall.Sparks;

public sealed partial class ESSparksSystem : EntitySystem
{
    [Dependency] private IGameTiming _timing = default!;
    [Dependency] private INetManager _net = default!;
    [Dependency] private IRobustRandom _random = default!;
    [Dependency] private SharedPowerReceiverSystem _powerReceiver = default!;
    [Dependency] private ESPreventCollideSystem _preventCollide = default!;
    [Dependency] private ThrowingSystem _throwing = default!;
    [Dependency] private SharedTransformSystem _transform = default!;

    public static readonly EntProtoId DefaultSparks = "AshfallEffectSparks";

    public void DoSparks<T>(
        Entity<T> ent,
        EntityUid? user = null,
        bool cooldown = true)
        where T : ESBaseSparkConfigurationComponent
    {
        if (!_random.Prob(ent.Comp.Prob))
            return;

        SharedApcPowerReceiverComponent? powerReceiver = null;
        if (_powerReceiver.ResolveApc(ent, ref powerReceiver) &&
            (!_powerReceiver.IsPowered((ent, powerReceiver)) || powerReceiver.Load <= 0))
            return;

        DoSparks(ent,
            number: ent.Comp.Count,
            ent.Comp.SparkPrototype,
            user: user,
            cooldown: cooldown);
    }

    [PublicAPI]
    public void DoSparks(
        EntityUid source,
        int number = 4,
        EntProtoId? sparksPrototype = null,
        EntityUid? user = null,
        bool cooldown = true)
    {
        var comp = EnsureComp<ESSparkCooldownComponent>(source);
        if (cooldown && _timing.CurTime - comp.LastSparkTime < comp.SparkDelay)
            return;
        comp.LastSparkTime = _timing.CurTime;

        var coords = Transform(source).Coordinates;
        DoSparks(coords, number, sparksPrototype, user, source);
    }

    [PublicAPI]
    public void DoSparks(
        EntityCoordinates coordinates,
        int number = 4,
        EntProtoId? sparksPrototype = null,
        EntityUid? user = null,
        EntityUid? ignored = null)
    {
        if (_net.IsClient)
            return;

        sparksPrototype ??= DefaultSparks;

        var angleDelta = (Angle) (MathF.Tau / number);
        var angle = _random.NextAngle();
        for (var i = 0; i < number; i++)
        {
            var sparks = Spawn(sparksPrototype, _transform.ToMapCoordinates(coordinates), rotation: angle);
            angle += angleDelta;
            _throwing.TryThrow(sparks, angle.ToVec(), 2f, animated: false);
            _preventCollide.PreventCollide(sparks, ignored);
        }
    }
}
