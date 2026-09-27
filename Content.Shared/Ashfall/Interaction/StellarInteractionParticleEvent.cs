using Robust.Shared.Serialization;

namespace Content.Shared.Ashfall.Interaction;

/// <summary>
/// Network event raised when an interaction occurs to show stellar interaction particles.
/// </summary>
[Serializable, NetSerializable]
public sealed class StellarInteractionParticleEvent(NetEntity performer, NetEntity? used, NetEntity target, bool isClientEvent, StellarInteractionParticleType type) : EntityEventArgs
{
    public NetEntity Performer = performer;
    public NetEntity? Used = used;
    public NetEntity Target = target;
    public bool IsClientEvent = isClientEvent;
    public StellarInteractionParticleType Type = type;
}

[Serializable, NetSerializable]
public enum StellarInteractionParticleType : byte
{
    Use,
    Pull,
}
