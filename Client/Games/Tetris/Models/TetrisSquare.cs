using Blazor.Extensions.Canvas.Canvas2D;
using Gejms.Client.Assets;
using Gejms.Client.Shared.GameServices.Interfaces;
using System.Drawing;

namespace Gejms.Client.Games.Tetris.Models;

public class TetrisSquare : IRenderable
{
    public int xPos { get; set; }
    public int yPos { get; set; }
    public bool Taken { get; set; } = false;
    public Sprite Sprite { get; set; } = null;
    public Size Size { get; } = new Size(35, 35);

    public TetrisSquare(int x, int y)
    {
        xPos = x;
        yPos = y;
    }

    public async ValueTask Render(Canvas2DContext context, IGame game)
    {
        if (Sprite == null)
            await context.FillRectAsync(xPos, yPos, Size.Width, Size.Height);
        else
            await context.DrawImageAsync(Sprite.ElementRef,
                0, 0, Sprite.Size.Width, Sprite.Size.Height,
                xPos * Size.Width, yPos * Size.Height, Size.Width, Size.Height);
    }
}
