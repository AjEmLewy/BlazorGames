using Blazor.Extensions.Canvas.Canvas2D;
using Gejms.Client.Games.Tetris.Models.SidePanelComponents;
using Gejms.Client.Shared.GameServices.Interfaces;
using System.Drawing;

namespace Gejms.Client.Games.Tetris.Models;

public class SidePanel : IRenderable
{
    private readonly int startXPos;
    private readonly int width;
    private readonly int height;

    private List<IRenderable> sidePanelComponents = new();

    public SidePanel(int startXPos, int width, int height)
    {
        this.startXPos = startXPos;
        this.width = width;
        this.height = height;

        sidePanelComponents.Add(new ScorePanel(new Rectangle(startXPos, 0, width, 150)));
        sidePanelComponents.Add(new NextPieceTooltip(new Rectangle(startXPos, 150, width, 300)));
        sidePanelComponents.Add(new TimePanel(new Rectangle(startXPos, 450, width, 120)));
        sidePanelComponents.Add(new LevelPanel(new Rectangle(startXPos, 570, width, 130)));
    }
    public async ValueTask Render(Canvas2DContext context, IGame game)
    {
        await context.SetFillStyleAsync("black");
        await context.FillRectAsync(startXPos, 0, width, height);

        foreach (var component in sidePanelComponents)
            await component.Render(context, game);

    }
}
