using Blazor.Extensions.Canvas.Canvas2D;
using Gejms.Client.Shared.GameServices.Interfaces;

namespace Gejms.Client.Games.Tetris.Screens;

public abstract class ScreenBase : IIterable, IRenderable
{
    private readonly TetrisScreenManager screenManager;
    public abstract ValueTask Load();
    public abstract ValueTask Iterate(IGame game);

    public abstract ValueTask Render(Canvas2DContext context, IGame game);
}
