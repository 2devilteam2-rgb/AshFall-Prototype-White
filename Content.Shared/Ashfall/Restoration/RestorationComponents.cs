using Content.Shared.DoAfter;
using Content.Shared.Stacks;
using Robust.Shared.Audio;
using Robust.Shared.GameStates;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization;
using System.Numerics;

namespace Content.Shared.Ashfall.Restoration;

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class RestorationGridComponent : Component
{
    [DataField, AutoNetworkedField]
    public int ChunkSize = 8;

    [DataField, AutoNetworkedField]
    public Dictionary<Vector2i, RestorationChunk> Chunks = new();

    [DataField, AutoNetworkedField]
    public List<EntProtoId> EntityPalette = new();

    [DataField, AutoNetworkedField]
    public bool SnapshotComplete;
}

[DataDefinition, Serializable, NetSerializable]
public sealed partial class RestorationChunk
{
    [DataField]
    public int[] Tiles = [];

    [DataField]
    public Dictionary<int, RestorationEntitySnapshot> Entities = new();

    [DataField]
    public int NextId;
}

[DataDefinition, Serializable, NetSerializable]
public sealed partial class RestorationEntitySnapshot
{
    [DataField]
    public int PrototypeIndex;

    [DataField]
    public Vector2 LocalPosition;

    [DataField]
    public Angle Rotation;

    [DataField]
    public NetEntity? OriginalEntity;
}

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class RestorableComponent : Component
{
    [DataField, AutoNetworkedField]
    public EntProtoId? RestoreAs;

    [DataField, AutoNetworkedField]
    public float RestoreTime = 2f;

    [DataField, AutoNetworkedField]
    public Dictionary<ProtoId<StackPrototype>, int> Requirements = new();
}

[RegisterComponent, NetworkedComponent, AutoGenerateComponentState]
public sealed partial class RestorationToolComponent : Component
{
    [DataField, AutoNetworkedField]
    public float TileRestoreTime = 2f;

    [DataField, AutoNetworkedField]
    public Dictionary<ProtoId<StackPrototype>, int> TileRequirements = new()
    {
        ["Steel"] = 1,
    };

    [DataField, AutoNetworkedField]
    public float EntitySearchRadius = 0.55f;

    [DataField, AutoNetworkedField]
    public float GhostRenderRadius = 8f;

    [DataField, AutoNetworkedField]
    public SoundSpecifier? RestoreSound = new SoundPathSpecifier("/Audio/Items/deconstruct.ogg");
}

[RegisterComponent, NetworkedComponent]
public sealed partial class RestorationEyeComponent : Component
{
    [DataField]
    public int Count;
}

[Serializable, NetSerializable]
public sealed partial class RestorationDoAfterEvent : SimpleDoAfterEvent
{
    public Vector2i Tile;
    public int? SnapshotId;

    [NonSerialized]
    public EntityUid? Grid;

    public override bool IsDuplicate(DoAfterEvent other)
    {
        return other is RestorationDoAfterEvent ev && ev.Tile == Tile && ev.SnapshotId == SnapshotId;
    }
}
