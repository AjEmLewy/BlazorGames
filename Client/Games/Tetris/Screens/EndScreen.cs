using Blazor.Extensions.Canvas.Canvas2D;
using Gejms.Client.Games.Tetris.Services;
using Gejms.Client.Shared.GameServices;
using Gejms.Shared.Models;

namespace Gejms.Client.Games.Tetris.Screens;

public class EndScreen : ScreenBase
{
    private readonly InputSystem inputSystem;
    private readonly ScoreService scoreService;
    private readonly TetrisScreenManager screenManager;
    private List<TetrisScoreUserDTO> topScores = new();
    private int myBest;

    public EndScreen(TetrisScreenManager screenManager)
    {
        inputSystem = GameServicesCollection.Instance.Get<InputSystem>();
        scoreService = GameServicesCollection.Instance.Get<ScoreService>();
        this.screenManager = screenManager;
    }
    public override ValueTask Iterate(IGame game)
    {
        if (inputSystem.keyboardKeyStates[(int)KeyboardKey.R])
            screenManager.TransitionToScreen<GameScreen>();
        if (inputSystem.keyboardKeyStates[(int)KeyboardKey.Esc])
            screenManager.TransitionToScreen<HomeScreen>();

        return ValueTask.CompletedTask;
    }

    public override async ValueTask Load()
    {
        var audioPlayer = GameServicesCollection.Instance.Get<AudioPlayer>();
        await audioPlayer.PlaySound("game_over.mp3");
        var httpService = GameServicesCollection.Instance.Get<TetrisHttpClient>();
        await httpService.SendScore();
        myBest = await httpService.GetMyBest();
        topScores = await httpService.GetTopFive();
    }

    public override async ValueTask Render(Canvas2DContext context, IGame game)
    {
        await context.SetFillStyleAsync("black");
        await context.FillRectAsync(0, 0, 500, 700);
        await context.SetFillStyleAsync("white");
        await context.SetFontAsync("50px MinecraftEvenings");
        await context.SetTextAlignAsync(TextAlign.Center);
        await context.FillTextAsync("GAME OVER", 250, 50);

        await context.SetFontAsync("32px MinecraftEvenings");
        await context.FillTextAsync("WYNIK: " + scoreService.Score.ToString(), 250, 100);

        await context.FillTextAsync("TWOJ NAJLEPSZY: " + myBest, 250, 150);

        await context.FillTextAsync("TOPKA", 250, 250);
        await context.SetFontAsync("24px Verdana");

        for (var i = 0; i < topScores.Count; ++i)
        {
            await context.FillTextAsync(topScores[i].Username, 120, 300 + i * 50);
            await context.FillTextAsync(topScores[i].Score.ToString(), 380, 300 + i * 50);
        }

        await context.FillTextAsync("Kliknij klawisz R żeby zacząć od nowa", 250, 600);
        await context.FillTextAsync("Lub Esc żeby iść do Menu głównego", 250, 650);
    }
}
