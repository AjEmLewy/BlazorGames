using Blazor.Extensions.Canvas.Canvas2D;
using Gejms.Client.Games.Tetris.Models;
using Gejms.Client.Games.Tetris.Services;
using Gejms.Client.Shared.GameServices;

namespace Gejms.Client.Games.Tetris.Screens;

public class GameScreen : ScreenBase
{
    private readonly SidePanel sidePanel;

    public GameScreen(TetrisScreenManager screenManager)
    {
        sidePanel = new SidePanel(350, 150, 700);
    }


    public override async ValueTask Iterate(IGame game)
    {
        await GameServicesCollection.Instance.IterateServices(game);
    }

    public override ValueTask Load()
    {
        GameServicesCollection.Instance.RestartServices();

        return ValueTask.CompletedTask;
    }

    public override async ValueTask Render(Canvas2DContext context, IGame game)
    {
        await GameServicesCollection.Instance.Get<TetrisBoard>().Render(context, game);
        await sidePanel.Render(context, game);
    }
}
