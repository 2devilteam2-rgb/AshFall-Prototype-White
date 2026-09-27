using System.Numerics;
using Content.Shared.Ashfall.Camera;
using Content.Shared.Ashfall.Fire;
using Content.Shared.Camera;
using Robust.Client.GameObjects;
using Robust.Shared.Configuration;
using Robust.Shared.Noise;
using Robust.Shared.Timing;

namespace Content.Client.Ashfall.Camera;

public sealed partial class ESScreenshakeSystem : EntitySystem
{
    [Dependency] private IConfigurationManager _config = default!;
    [Dependency] private IGameTiming _timing = default!;
    [Dependency] private SharedESScreenshakeSystem _shared = default!;
    [Dependency] private SharedEyeSystem _eye = default!;

    private bool _disabled;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ESScreenshakeComponent, GetEyeOffsetEvent>(OnGetEyeOffset);
        SubscribeLocalEvent<ESScreenshakeComponent, ESGetEyeRotationEvent>(OnGetEyeRotation);
        SubscribeLocalEvent<ESScreenshakeComponent, ComponentShutdown>(OnShutdown);

        _config.OnValueChanged(AshfallFireCVars.ScreenshakeDisabled, OnDisabledChanged, true);
    }

    private void OnShutdown(Entity<ESScreenshakeComponent> ent, ref ComponentShutdown args)
    {
        if (ent.Comp.CurrentShake != Angle.Zero && TryComp<EyeComponent>(ent, out var eye))
        {
            _eye.SetRotation(ent, eye.Rotation - ent.Comp.CurrentShake, eye);
            ent.Comp.CurrentShake = Angle.Zero;
        }
    }

    private void OnDisabledChanged(bool obj)
    {
        _disabled = obj;
    }

    private void OnGetEyeOffset(Entity<ESScreenshakeComponent> ent, ref GetEyeOffsetEvent args)
    {
        if (!TryComp<EyeComponent>(ent, out var eye) || _disabled)
            return;

        var noise = new FastNoiseLite(67);
        noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);

        var accumulatedOffset = Vector2.Zero;
        var maxOffset = new Vector2(0.15f, 0.15f);
        foreach (var command in ent.Comp.Commands)
        {
            if (command.Translational == null)
                continue;

            var trauma = _shared.CalculateTraumaValueForCurrentTime(command.Translational, command.Start);
            if (trauma <= 0)
                continue;

            noise.SetFrequency(command.Translational.Frequency);

            var offsetX = (maxOffset.X * trauma) * noise.GetNoise((float)_timing.RealTime.TotalMilliseconds, (float)command.Start.TotalMilliseconds);
            noise.SetSeed(68);
            var offsetY = (maxOffset.Y * trauma) * noise.GetNoise((float)_timing.RealTime.TotalMilliseconds, (float)command.Start.TotalMilliseconds);
            noise.SetSeed(67);
            accumulatedOffset += new Vector2(offsetX, offsetY);
        }

        args.Offset += accumulatedOffset;
    }

    private void OnGetEyeRotation(Entity<ESScreenshakeComponent> ent, ref ESGetEyeRotationEvent args)
    {
        if (!TryComp<EyeComponent>(ent, out var eye) || _disabled)
            return;

        var noise = new FastNoiseLite(67 + 420);
        noise.SetNoiseType(FastNoiseLite.NoiseType.OpenSimplex2);

        var accumulatedAngle = Angle.Zero;
        var maxAngleDegrees = 20f;
        foreach (var command in ent.Comp.Commands)
        {
            if (command.Rotational == null)
                continue;

            var trauma = _shared.CalculateTraumaValueForCurrentTime(command.Rotational, command.Start);
            if (trauma <= 0)
                continue;

            noise.SetFrequency(command.Rotational.Frequency);

            var angle = (maxAngleDegrees * trauma) * noise.GetNoise((float)_timing.RealTime.TotalMilliseconds, (float)command.Start.TotalMilliseconds);
            accumulatedAngle += Angle.FromDegrees(angle);
        }

        args.Rotation += accumulatedAngle;
    }
}
