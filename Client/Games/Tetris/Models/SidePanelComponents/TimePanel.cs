using Blazor.Extensions.Canvas.Canvas2D;
using Gejms.Client.Games.Tetris;
using Gejms.Client.Shared.GameServices;
using Gejms.Client.Shared.GameServices.Interfaces;
using System.Drawing;

namespace Gejms.Client.Games.Tetris.Models.SidePanelComponents;

public class TimePanel : IRenderable
{
    private readonly GameTime gameTime;
    private readonly Rectangle panelRect;

    public TimePanel(Rectangle panelRect)
    {
        gameTime = GameServicesCollection.Instance.Get<GameTime>();
        this.panelRect = panelRect;
    }
    public async ValueTask Render(Canvas2DContext context, IGame game)
    {
        await context.SaveAsync();

        await context.SetFillStyleAsync("white");
        await context.SetFontAsync("24px verdana");
        await context.SetTextAlignAsync(TextAlign.Center);

        await context.FillTextAsync("Time", panelRect.X + panelRect.Width / 2, panelRect.Y + 30);
        var timePlayed = (gameTime.TotalMillis / 1000).ToString("0.0");
        await context.FillTextAsync(timePlayed, panelRect.X + panelRect.Width / 2, panelRect.Y + 80);

        await context.FillRectAsync(panelRect.X, panelRect.Y + panelRect.Height - 5, panelRect.Width, 5);

        await context.RestoreAsync();
    }
}
