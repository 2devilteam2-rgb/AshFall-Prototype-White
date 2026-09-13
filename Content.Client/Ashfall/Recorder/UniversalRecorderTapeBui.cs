using System.Linq;
using Content.Client.UserInterface.Controls;
using Content.Shared.Ashfall.Recorder;
using JetBrains.Annotations;
using Robust.Client.UserInterface;
using Robust.Shared.Utility;

namespace Content.Client.Ashfall.Recorder;

[UsedImplicitly]
public sealed class UniversalRecorderTapeBui(EntityUid owner, Enum uiKey) : BoundUserInterface(owner, uiKey)
{
    private static readonly Dictionary<UniversalRecorderTapeAction, (string Tooltip, SpriteSpecifier Sprite)> ActionData = new()
    {
        [UniversalRecorderTapeAction.Flip] = ("ashfall-recorder-tape-verb-flip", new SpriteSpecifier.Texture(new ResPath("/Textures/Interface/VerbIcons/rotate_cw.svg.192dpi.png"))),
        [UniversalRecorderTapeAction.Unwind] = ("ashfall-recorder-tape-verb-unwind", new SpriteSpecifier.Texture(new ResPath("/Textures/Interface/VerbIcons/scissors.svg.236dpi.png"))),
    };

    private SimpleRadialMenu? _menu;
    private UniversalRecorderTapeBuiState? _state;

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
        if (state is not UniversalRecorderTapeBuiState tapeState)
            return;

        _state = tapeState;
        _menu?.SetButtons(ConvertToButtons(tapeState.Actions));
    }

    private IEnumerable<RadialMenuActionOption<UniversalRecorderTapeAction>> ConvertToButtons(IEnumerable<UniversalRecorderTapeAction> actions)
    {
        return actions.Select(action =>
        {
            var data = ActionData[action];
            return new RadialMenuActionOption<UniversalRecorderTapeAction>(OnActionPressed, action)
            {
                IconSpecifier = RadialMenuIconSpecifier.With(data.Sprite),
                ToolTip = Loc.GetString(data.Tooltip),
            };
        });
    }

    private void OnActionPressed(UniversalRecorderTapeAction action)
    {
        SendPredictedMessage(new UniversalRecorderTapeActionBuiMsg(action));
    }
}
