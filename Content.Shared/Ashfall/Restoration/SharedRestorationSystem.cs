using Robust.Shared.Map;

namespace Content.Shared.Ashfall.Restoration;

public abstract class SharedRestorationSystem : EntitySystem
{
    protected static Vector2i GetChunkIndex(Vector2i tile, int chunkSize)
    {
        var x = tile.X < 0 ? 1 - chunkSize + tile.X : tile.X;
        var y = tile.Y < 0 ? 1 - chunkSize + tile.Y : tile.Y;
        return new Vector2i(x / chunkSize, y / chunkSize);
    }

    protected static Vector2i GetRelativeIndex(Vector2i tile, int chunkSize)
    {
        return new Vector2i(MathHelper.Mod(tile.X, chunkSize), MathHelper.Mod(tile.Y, chunkSize));
    }

    protected static bool TryGetChunk(RestorationGridComponent data, Vector2i tile, out RestorationChunk chunk)
    {
        return data.Chunks.TryGetValue(GetChunkIndex(tile, data.ChunkSize), out chunk!);
    }
}
