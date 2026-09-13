using System.Linq;
using Content.Client.UserInterface.Controls;
using Content.Shared.Ashfall.Recorder;
using JetBrains.Annotations;
using Robust.Client.UserInterface;
using Robust.Shared.Utility;

namespace Content.Client.Ashfall.Recorder;

[UsedImplicitly]
public sealed class UniversalRecorderRecorderBui(EntityUid owner, Enum uiKey) : BoundUserInterface(owner, uiKey)
{
    private static readonly Dictionary<UniversalRecorderRecorderAction, (string Tooltip, SpriteSpecifier Sprite)> ActionData = new()
    {
        [UniversalRecorderRecorderAction.Record] = ("ashfall-recorder-verb-record", Icon("signal.svg.192dpi.png")),
        [UniversalRecorderRecorderAction.Play] = ("ashfall-recorder-verb-play", Icon("point.svg.192dpi.png")),
        [UniversalRecorderRecorderAction.Stop] = ("ashfall-recorder-verb-stop", Icon("dot.svg.192dpi.png")),
        [UniversalRecorderRecorderAction.PrintTranscript] = ("ashfall-recorder-verb-print", Icon("information.svg.192dpi.png")),
        [UniversalRecorderRecorderAction.Eject] = ("ashfall-recorder-verb-eject", Icon("eject.svg.192dpi.png")),
    };

    private static SpriteSpecifier Icon(string name) =>
        new SpriteSpecifier.Texture(new ResPath($"/Textures/Interface/VerbIcons/{name}"));

    private SimpleRadialMenu? _menu;
    private UniversalRecorderRecorderBuiState? _state;

    protected override void Open()
    {
        base.Open();

        _menu = this.CreateWindow<SimpleRadialMenu>();
        _menu.Track(Owner);

        if (_state != null)
            _menu.SetButtons(ConvertToButtons(_state.Actions));

        _menu.OpenOverMouseScreenPosition();
    }

    protected override void UpdateState(BoundUserInterfaceState state)
    {
        if (state is not UniversalRecorderRecorderBuiState recorderState)
            return;

        _state = recorderState;
        _menu?.SetButtons(ConvertToButtons(recorderState.Actions));
    }

    private IEnumerable<RadialMenuActionOption<UniversalRecorderRecorderAction>> ConvertToButtons(IEnumerable<UniversalRecorderRecorderAction> actions)
    {
        return actions.Select(action =>
        {
            var data = ActionData[action];
            return new RadialMenuActionOption<UniversalRecorderRecorderAction>(OnActionPressed, action)
            {
                IconSpecifier = RadialMenuIconSpecifier.With(data.Sprite),
                ToolTip = Loc.GetString(data.Tooltip),
            };
        });
    }

    private void OnActionPressed(UniversalRecorderRecorderAction action)
    {
        SendPredictedMessage(new UniversalRecorderRecorderActionBuiMsg(action));
    }
}
