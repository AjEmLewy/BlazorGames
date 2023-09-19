using Blazor.Extensions.Canvas.Canvas2D;
using Gejms.Client.Games.Tetris.Services;
using Gejms.Client.Shared.GameServices;
using Gejms.Client.Shared.GameServices.Interfaces;
using System.Drawing;

namespace Gejms.Client.Games.Tetris.Models.SidePanelComponents
{
    public class LevelPanel : IRenderable
    {
        private readonly Rectangle panelRect;
        private readonly ScoreService scoreService;

        public LevelPanel(Rectangle panelRect)
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

            await context.FillTextAsync("Level", panelRect.X + panelRect.Width / 2, panelRect.Y + 30);
            await context.FillTextAsync(scoreService.Level.ToString(), panelRect.X + panelRect.Width / 2, panelRect.Y + 70);

            await context.RestoreAsync();
        }
    }
}
