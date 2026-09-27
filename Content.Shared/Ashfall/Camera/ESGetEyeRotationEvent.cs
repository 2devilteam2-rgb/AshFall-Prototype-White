using Robust.Shared.Maths;

namespace Content.Shared.Ashfall.Camera;

/// <summary>
/// Raised directed by-ref when eye rotation is updated, allowing systems like screenshake to modify it.
/// </summary>
[ByRefEvent]
public record struct ESGetEyeRotationEvent(Angle Rotation);
