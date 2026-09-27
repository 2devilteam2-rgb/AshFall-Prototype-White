using System.Linq;
using Robust.Shared.GameStates;
using Robust.Shared.Player;
using Robust.Shared.Timing;

namespace Content.Shared.Ashfall.Camera;

public sealed partial class SharedESScreenshakeSystem : EntitySystem
{
    [Dependency] private IGameTiming _timing = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ESScreenshakeComponent, EntityUnpausedEvent>(OnEntityUnpaused);
    }

    public override void Update(float frameTime)
    {
        base.Update(frameTime);

        var shakers = EntityQueryEnumerator<EyeComponent, ESScreenshakeComponent>();
        while (shakers.MoveNext(out var ent, out var eye, out var shake))
        {
            if (shake.Commands.Count == 0)
            {
                RemCompDeferred<ESScreenshakeComponent>(ent);
                continue;
            }

            foreach (var command in shake.Commands.ToList())
            {
                if (_timing.CurTime < command.CalculatedEnd)
                    continue;
                shake.Commands.Remove(command);
                Dirty(ent, shake);
            }
        }
    }

    private void OnEntityUnpaused(Entity<ESScreenshakeComponent> ent, ref EntityUnpausedEvent args)
    {
        var newSet = new HashSet<ESScreenshakeCommand>();
        foreach (var command in ent.Comp.Commands)
        {
            var newCommand = command with
            {
                CalculatedEnd = command.CalculatedEnd + args.PausedTime,
                Start = command.Start + args.PausedTime,
            };
            newSet.Add(newCommand);
        }

        ent.Comp.Commands = newSet;
        Dirty(ent);
    }

    private TimeSpan CalculateEndTimeForCommand(Entity<ESScreenshakeComponent> ent, ESScreenshakeParameters? translation, ESScreenshakeParameters? rotation, TimeSpan start)
    {
        var secsUntilRotationalEnd = rotation != null ? MathF.Sqrt(rotation.Trauma / rotation.DecayRate) : 0f;
        var secsUntilTranslationalEnd = translation != null ? MathF.Sqrt(translation.Trauma / translation.DecayRate) : 0f;
        var larger = MathF.Max(secsUntilTranslationalEnd, secsUntilRotationalEnd);

        return start + TimeSpan.FromSeconds(larger);
    }

    public float CalculateTraumaValueForCurrentTime(ESScreenshakeParameters parameters, TimeSpan start)
    {
        var timeDiff = _timing.CurTime - start;
        if (timeDiff < TimeSpan.Zero)
            return 0f;

        var totalSecsSquared = (float) (timeDiff.TotalSeconds * timeDiff.TotalSeconds);
        return -totalSecsSquared * parameters.DecayRate + parameters.Trauma;
    }

    public void Screenshake(EntityUid uid, ESScreenshakeParameters? translation, ESScreenshakeParameters? rotation)
    {
        if (!HasComp<EyeComponent>(uid))
            return;

        var comp = EnsureComp<ESScreenshakeComponent>(uid);
        var time = _timing.CurTime;
        var end = CalculateEndTimeForCommand((uid, comp), translation, rotation, time);
        var command = new ESScreenshakeCommand(translation, rotation, time, end);

        comp.Commands.Add(command);
        Dirty(uid, comp);
    }

    public void Screenshake(Filter filter, ESScreenshakeParameters? translation, ESScreenshakeParameters? rotation)
    {
        foreach (var player in filter.Recipients)
        {
            if (player.AttachedEntity is { } ent)
                Screenshake(ent, translation, rotation);
        }
    }
}
