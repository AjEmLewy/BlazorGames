using Blazor.Extensions.Canvas.Canvas2D;
using Gejms.Client.Games.Tetris.Services;
using Gejms.Client.Shared.GameServices;
using Gejms.Client.Shared.GameServices.Interfaces;
using System.Drawing;

namespace Gejms.Client.Games.Tetris.Models.SidePanelComponents;

public class ScorePanel : IRenderable
{
    private readonly Rectangle panelRect;
    private readonly ScoreService scoreService;

    public ScorePanel(Rectangle panelRect)
    {
        this.panelRect = panelRect;
        scoreService = GameServicesCollection.Instance.Get<ScoreService>();
    }
    public async ValueTask Render(Canvas2DContext context, IGame game)
    {
        await context.SaveAsync();
        await context.SetFillStyleAsync("white");
        await context.SetFontAsync("24px verdana");
        await context.SetTextAlignAsync(TextAlign.Center);

        await context.FillTextAsync("Score", panelRect.X + panelRect.Width / 2, panelRect.Y + 30);

        await context.FillTextAsync(scoreService.Score.ToString("000000"), panelRect.X + panelRect.Width / 2, panelRect.Y + 70);

        await context.FillRectAsync(panelRect.X, panelRect.Y + panelRect.Height - 5, panelRect.Width, 5);

        await context.RestoreAsync();
    }
}
