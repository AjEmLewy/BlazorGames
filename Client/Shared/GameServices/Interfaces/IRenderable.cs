using Blazor.Extensions.Canvas.Canvas2D;
using Gejms.Client.Games.Tetris;

namespace Gejms.Client.Shared.GameServices.Interfaces;

public interface IRenderable
{
    ValueTask Render(Canvas2DContext context, IGame game);
}