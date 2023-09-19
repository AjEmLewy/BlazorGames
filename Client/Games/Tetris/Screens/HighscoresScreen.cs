using Blazor.Extensions.Canvas.Canvas2D;
using Gejms.Client.Games.Tetris.Services;
using Gejms.Client.Shared.GameServices;
using Gejms.Shared.Models;

namespace Gejms.Client.Games.Tetris.Screens;

public class HighscoresScreen : ScreenBase
{
    private readonly TetrisScreenManager screenManager;
    private List<TetrisScoreUserDTO> topScores;
    private InputSystem inputSystem;

    public HighscoresScreen(TetrisScreenManager screenManager)
    {
        inputSystem = GameServicesCollection.Instance.Get<InputSystem>();
        this.screenManager = screenManager;
    }
    public override ValueTask Iterate(IGame game)
    {
        if (inputSystem.keyboardKeyStates[(int)KeyboardKey.Space])
            screenManager.TransitionToScreen<GameScreen>();

        if (inputSystem.keyboardKeyStates[(int)KeyboardKey.Esc])
            screenManager.TransitionToScreen<HomeScreen>();

        return ValueTask.CompletedTask;
    }

    public async override ValueTask Load()
    {
        var httpService = GameServicesCollection.Instance.Get<TetrisHttpClient>();
        topScores = await httpService.GetTopFive();
    }

    public async override ValueTask Render(Canvas2DContext context, IGame game)
    {
        await context.SetFillStyleAsync("black");
        await context.FillRectAsync(0, 0, 500, 700);
        await context.SetFillStyleAsync("white");
        await context.SetTextAlignAsync(TextAlign.Center);

        await context.SetFontAsync("32px MinecraftEvenings");

        await context.FillTextAsync("TOPKA", 250, 50);
        await context.SetFontAsync("24px Verdana");

        for (var i = 0; i < topScores.Count; ++i)
        {
            await context.FillTextAsync(topScores[i].Username, 120, 100 + i * 50);
            await context.FillTextAsync(topScores[i].Score.ToString(), 380, 100 + i * 50);
        }

        await context.FillTextAsync("Kliknij spację żeby zacząć grę", 250, 600);
        await context.FillTextAsync("Lub Esc żeby wrócić do menu", 250, 650);
    }
}
