using Content.Client.Resources;
using Content.Shared.Chat;
using Content.Shared.Input;
using Robust.Client.ResourceManagement;
using Robust.Client.UserInterface.Controls;
using Robust.Shared.Maths;

namespace Content.Client.UserInterface.Systems.Chat.Controls;

[Virtual]
public class ChatInputBox : PanelContainer
{
    public const string StyleClassChatPanel = "ChatPanel";
    public const string StyleClassChatLineEdit = "ChatLineEdit";
    public const string StyleClassChatFilterOptionButton = "ChatFilterOptionButton";

    public readonly ChannelSelectorButton ChannelSelector;
    public readonly HistoryLineEdit Input;
    public readonly ChannelFilterButton FilterButton;
    public readonly Button SearchButton;
    protected readonly BoxContainer Container;
    protected ChatChannel ActiveChannel { get; private set; } = ChatChannel.Local;

    public event Action? OnSearchButtonPressed;

    public ChatInputBox()
    {
        Container = new BoxContainer
        {
            Orientation = BoxContainer.LayoutOrientation.Horizontal,
            SeparationOverride = 4
        };
        AddChild(Container);

        ChannelSelector = new ChannelSelectorButton
        {
            Name = "ChannelSelector",
            ToggleMode = true,
            StyleClasses = { ChannelSelectorItemButton.StyleClassChatSelectorOptionButton },
            MinWidth = 75
        };
        Container.AddChild(ChannelSelector);
        Input = new HistoryLineEdit
        {
            Name = "Input",
            PlaceHolder = GetChatboxInfoPlaceholder(),
            HorizontalExpand = true,
            StyleClasses = { StyleClassChatLineEdit }
        };
        Container.AddChild(Input);

        var searchTexture = IoCManager.Resolve<IResourceCache>()
            .GetTexture("/Textures/Interface/VerbIcons/examine.svg.192dpi.png");
        var searchIcon = new TextureRect
        {
            Texture = searchTexture,
            TextureScale = new System.Numerics.Vector2(0.25f, 0.25f),
            HorizontalAlignment = HAlignment.Center,
            VerticalAlignment = VAlignment.Center,
            ModulateSelfOverride = Color.FromHex("#7b7e9e")
        };

        SearchButton = new Button
        {
            Name = "SearchButton",
            ToolTip = Loc.GetString("hud-adt-chat-search-button-tooltip"),
            StyleClasses = { StyleClassChatFilterOptionButton },
            MinSize = new System.Numerics.Vector2(28, 0)
        };
        SearchButton.AddChild(searchIcon);
        SearchButton.OnMouseEntered += _ => searchIcon.ModulateSelfOverride = Color.FromHex("#9699bb");
        SearchButton.OnMouseExited += _ => searchIcon.ModulateSelfOverride = Color.FromHex("#7b7e9e");
        SearchButton.OnPressed += _ => OnSearchButtonPressed?.Invoke();
        Container.AddChild(SearchButton);

        FilterButton = new ChannelFilterButton
        {
            Name = "FilterButton",
            StyleClasses = { StyleClassChatFilterOptionButton }
        };
        Container.AddChild(FilterButton);
        AddStyleClass(StyleClassChatPanel);
        ChannelSelector.OnChannelSelect += UpdateActiveChannel;
    }

    private void UpdateActiveChannel(ChatSelectChannel selectedChannel)
    {
        ActiveChannel = (ChatChannel) selectedChannel;
    }

    private static string GetChatboxInfoPlaceholder()
    {
        return (BoundKeyHelper.IsBound(ContentKeyFunctions.FocusChat),
                BoundKeyHelper.IsBound(ContentKeyFunctions.CycleChatChannelForward)) switch
            {
                (true, true) => Loc.GetString("hud-chatbox-info",
                    ("talk-key", BoundKeyHelper.ShortKeyName(ContentKeyFunctions.FocusChat)),
                    ("cycle-key", BoundKeyHelper.ShortKeyName(ContentKeyFunctions.CycleChatChannelForward))),
                (true, false) => Loc.GetString("hud-chatbox-info-talk",
                    ("talk-key", BoundKeyHelper.ShortKeyName(ContentKeyFunctions.FocusChat))),
                (false, true) => Loc.GetString("hud-chatbox-info-cycle",
                    ("cycle-key", BoundKeyHelper.ShortKeyName(ContentKeyFunctions.CycleChatChannelForward))),
                (false, false) => Loc.GetString("hud-chatbox-info-unbound")
            };
    }
}
