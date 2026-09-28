using Content.Shared.Ashfall.Fire;
using Content.Shared.Ashfall.Fire.Components;
using Robust.Client.GameObjects;

namespace Content.Client.Ashfall.Fire;

public sealed partial class ReagentPuddleFireVisualsSystem : EntitySystem
{
    [Dependency] private AppearanceSystem _appearance = default!;

    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ReagentPuddleFireEffectComponent, AppearanceChangeEvent>(OnAppearanceChange);
    }

    private void OnAppearanceChange(EntityUid uid, ReagentPuddleFireEffectComponent component, ref AppearanceChangeEvent args)
    {
        if (args.Sprite == null)
            return;

        if (_appearance.TryGetData<int>(uid, ReagentPuddleFireVisuals.FireState, out var fireState, args.Component))
        {
            var stateStr = fireState switch
            {
                1 => "4",
                2 => "5",
                3 => "6",
                _ => Math.Clamp(fireState, 4, 6).ToString()
            };
            args.Sprite.LayerSetState(0, stateStr);
        }

        if (_appearance.TryGetData<Color>(uid, ReagentPuddleFireVisuals.FireColor, out var color, args.Component))
        {
            args.Sprite.Color = color;
        }
    }
}
