using Blazor.Extensions.Canvas.Canvas2D;
using Gejms.Client.Games.Tetris.GameObjects.Flame;
using Gejms.Client.Shared.GameServices;
using Gejms.Client.Shared.Models;
using Gejms.Client.Shared.Models.GameObjectComponents;
using System.Drawing;

namespace Gejms.Client.Games.Tetris.Screens;

public class HomeScreen : ScreenBase
{
    private readonly GameTime gameTime;
    private readonly InputSystem inputSystem;
    private List<GameObject> flames;

    private int currentIndex = 0;
    private List<Tuple<string, Action<TetrisGame>>> choices;

    private float clickInterval = 250;
    private float lastClickTime = 0;

    public HomeScreen(TetrisScreenManager screenManager)
    {
        choices = new List<Tuple<string, Action<TetrisGame>>>
        {
            new ("START GAME", x => screenManager.TransitionToScreen<GameScreen>()),
            new ("STEROWANIE", x => screenManager.TransitionToScreen<SterowanieScreen>()),
            new ("HIGHSCORES", x => screenManager.TransitionToScreen<HighscoresScreen>())
        };
        gameTime = GameServicesCollection.Instance.Get<GameTime>();
        inputSystem = GameServicesCollection.Instance.Get<InputSystem>();
    }
    public async override ValueTask Iterate(IGame game)
    {
        await gameTime.Iterate(game);
        CheckCursorMove();
        CheckEnterClick(game);
        foreach (var ob in flames)
            await ob.Iterate(game);
    }

    public override ValueTask Load()
    {
        GameServicesCollection.Instance.RestartServices();
        lastClickTime = 0;

        //flames could be created once but we are restarting time so animation freezes till it does not reach the correct moment. easier this way.
        flames = new();

        var yPos = 200 + 100 * currentIndex;
        var firstFlame = new FlameObject(new Point(100, yPos));
        var secondFlame = new FlameObject(new Point(400, yPos));

        flames.Add(firstFlame);
        flames.Add(secondFlame);

        return ValueTask.CompletedTask;
    }

    public override async ValueTask Render(Canvas2DContext context, IGame game)
    {
        await context.SetFillStyleAsync("black");
        await context.FillRectAsync(0, 0, 500, 700);

        await context.SetFillStyleAsync("white");

        await context.SetFontAsync("60px MinecraftEvenings");
        await context.SetTextAlignAsync(TextAlign.Center);
        await context.FillTextAsync("Tetris", 250, 50);

        foreach (var ob in flames)
            await ob.Render(context, game);

        await context.SetFontAsync("30px MinecraftEvenings");

        for (int i = 0; i < choices.Count; ++i)
        {
            var yPos = 200 + 100 * i;
            if (i == currentIndex)
            {
                await context.SetFillStyleAsync("blue");
                await context.FillTextAsync(choices[i].Item1, 250, yPos);
            }
            else
            {
                await context.SetFillStyleAsync("white");
                await context.FillTextAsync(choices[i].Item1, 250, yPos);
            }
        }
    }

    private void CheckCursorMove()
    {
        if (inputSystem.keyboardKeyStates[(int)KeyboardKey.Down] && gameTime.TotalMillis - lastClickTime > clickInterval)
        {
            currentIndex = (currentIndex + 1) % choices.Count;
            lastClickTime = gameTime.TotalMillis;
            MoveFlames();
        }
        else if (inputSystem.keyboardKeyStates[(int)KeyboardKey.Up] && gameTime.TotalMillis - lastClickTime > clickInterval)
        {
            currentIndex = (currentIndex - 1 + choices.Count) % choices.Count;
            lastClickTime = gameTime.TotalMillis;
            MoveFlames();
        }
    }

    private void MoveFlames()
    {
        var yPos = 200 + 100 * currentIndex;
        foreach (var flame in flames)
        {
            var posComp = flame.Components.Get<PositionComponent>();
            posComp.SetPosition(posComp.Position.X, yPos);
        }
    }

    private void CheckEnterClick(IGame game)
    {
        if (inputSystem.keyboardKeyStates[(int)KeyboardKey.Enter])
            choices[currentIndex].Item2(game as TetrisGame);
    }
}
