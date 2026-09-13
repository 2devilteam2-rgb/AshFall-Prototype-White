using Content.Shared.Ashfall.Restoration;
using Content.Client.IconSmoothing;
using Content.Shared.Hands.EntitySystems;
using Robust.Client.GameObjects;
using Robust.Client.Player;
using Robust.Shared.Map;
using Robust.Shared.Map.Components;
using Robust.Shared.Prototypes;
using Robust.Shared.Serialization.Manager;
using Robust.Shared.Timing;

namespace Content.Client.Ashfall.Restoration;

public sealed partial class RestorationSystem : SharedRestorationSystem
{
    [Dependency] private IGameTiming _timing = default!;
    [Dependency] private IPlayerManager _player = default!;
    [Dependency] private IPrototypeManager _prototype = default!;
    [Dependency] private ISerializationManager _serialization = default!;
    [Dependency] private ITileDefinitionManager _tileDefinitions = default!;
    [Dependency] private MetaDataSystem _metaData = default!;
    [Dependency] private SharedHandsSystem _hands = default!;
    [Dependency] private SharedMapSystem _map = default!;
    [Dependency] private SharedTransformSystem _transform = default!;
    [Dependency] private SpriteSystem _sprite = default!;

    private EntityQuery<RestorationGridComponent> _dataQuery;
    private EntityQuery<RestorationToolComponent> _toolQuery;

    public override void Initialize()
    {
        base.Initialize();
        _dataQuery = GetEntityQuery<RestorationGridComponent>();
        _toolQuery = GetEntityQuery<RestorationToolComponent>();
    }
}
