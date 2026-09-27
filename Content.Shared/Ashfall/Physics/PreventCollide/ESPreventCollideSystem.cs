using Content.Shared.Ashfall.Physics.PreventCollide.Components;
using Robust.Shared.Physics.Events;

namespace Content.Shared.Ashfall.Physics.PreventCollide;

public sealed partial class ESPreventCollideSystem : EntitySystem
{
    public override void Initialize()
    {
        base.Initialize();
        SubscribeLocalEvent<ESPreventCollideComponent, ComponentShutdown>(OnShutdown);
        SubscribeLocalEvent<ESPreventCollideMarkerComponent, ComponentShutdown>(OnMarkerShutdown);
        SubscribeLocalEvent<ESPreventCollideComponent, PreventCollideEvent>(OnPreventCollide);
    }

    private void OnShutdown(Entity<ESPreventCollideComponent> ent, ref ComponentShutdown args)
    {
        foreach (var uid in ent.Comp.Entities)
        {
            if (!TryComp<ESPreventCollideMarkerComponent>(uid, out var comp))
                continue;
            comp.Entities.Remove(ent);

            if (comp.Entities.Count == 0)
                RemComp(uid, comp);
            else
                Dirty(uid, comp);
        }
    }

    private void OnMarkerShutdown(Entity<ESPreventCollideMarkerComponent> ent, ref ComponentShutdown args)
    {
        foreach (var uid in ent.Comp.Entities)
        {
            if (!TryComp<ESPreventCollideComponent>(uid, out var comp))
                continue;
            comp.Entities.Remove(ent);

            if (comp.Entities.Count == 0)
                RemComp(uid, comp);
            else
                Dirty(uid, comp);
        }
    }

    private void OnPreventCollide(Entity<ESPreventCollideComponent> ent, ref PreventCollideEvent args)
    {
        if (args.Cancelled)
            return;

        if (ent.Comp.Entities.Contains(args.OtherEntity))
            args.Cancelled = true;
    }

    public void PreventCollide(EntityUid primary, EntityUid? collider)
    {
        if (!collider.HasValue || TerminatingOrDeleted(collider) || EntityManager.IsQueuedForDeletion(collider.Value))
            return;

        var preventComp = EnsureComp<ESPreventCollideComponent>(collider.Value);
        preventComp.Entities.Add(primary);
        Dirty(collider.Value, preventComp);

        var markerComp = EnsureComp<ESPreventCollideMarkerComponent>(primary);
        markerComp.Entities.Add(collider.Value);
        Dirty(primary, markerComp);
    }
}
