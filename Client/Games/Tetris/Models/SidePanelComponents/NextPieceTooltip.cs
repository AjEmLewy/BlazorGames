using Blazor.Extensions.Canvas.Canvas2D;
using Gejms.Client.Games.Tetris.Services;
using Gejms.Client.Shared.GameServices;
using Gejms.Client.Shared.GameServices.Interfaces;
using System.Drawing;

namespace Gejms.Client.Games.Tetris.Models.SidePanelComponents;

public class NextPieceTooltip : IRenderable
{
    private readonly Rectangle panelRect;
    private readonly Point firstSqPos;
    private readonly int sqWidth = 25;
    private Point[] nextShape;

    public NextPieceTooltip(Rectangle panelRect)
    {
        this.panelRect = panelRect;
        firstSqPos = new Point((panelRect.Width - sqWidth) / 2, 100);

        var pieceFactory = GameServicesCollection.Instance.Get<PieceFactory>();
        pieceFactory.OnNextPieceCreated += () =>
        {
            nextShape = pieceFactory.nextPieceInitStructure;
        };
    }
    public async ValueTask Render(Canvas2DContext context, IGame game)
    {
        await context.SaveAsync();

        await context.SetFillStyleAsync("white");
        await context.SetFontAsync("24px verdana");
        await context.SetTextAlignAsync(TextAlign.Center);

        await context.FillTextAsync("Next", panelRect.X + panelRect.Width / 2, panelRect.Y + 30);

        foreach (var sq in nextShape)
            await context.FillRectAsync(panelRect.X + firstSqPos.X + sq.X * sqWidth, panelRect.Y + firstSqPos.Y + sq.Y * sqWidth,
                sqWidth, sqWidth);

        await context.FillRectAsync(panelRect.X, panelRect.Y + panelRect.Height - 5, panelRect.Width, 5);

        await context.RestoreAsync();
    }
}
