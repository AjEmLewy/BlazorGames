using Blazor.Extensions.Canvas.Canvas2D;
using Gejms.Client.Shared.GameServices;

namespace Gejms.Client.Games.Tetris.Screens;

public class SterowanieScreen : ScreenBase
{
    private readonly TetrisScreenManager screenManager;
    InputSystem InputSystem;
    public SterowanieScreen(TetrisScreenManager screenManager)
    {
        InputSystem = GameServicesCollection.Instance.Get<InputSystem>();
        this.screenManager = screenManager;
    }
    public override ValueTask Iterate(IGame game)
    {
        if (InputSystem.keyboardKeyStates[(int)KeyboardKey.Space])
            screenManager.TransitionToScreen<GameScreen>();

        if (InputSystem.keyboardKeyStates[(int)KeyboardKey.Esc])
            screenManager.TransitionToScreen<HomeScreen>();

        return ValueTask.CompletedTask;
    }

    public override ValueTask Load()
    {
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

        await context.SetFontAsync("32px MinecraftEvenings");
        await context.FillTextAsync("Sterowanie: ", 250, 100);

        await context.SetFontAsync("16px Verdana");

        await context.FillTextAsync("Strzałki w prawo i lewo to chyba wiesz.", 250, 150);
        await context.FillTextAsync("Strzałka w górę obrót", 250, 200);
        await context.FillTextAsync("Strzałka w dół przyspiesza klocucha", 250, 250);
        await context.FillTextAsync("Spacja osadza klocka w miejscu docelowym", 250, 300);

        await context.SetFontAsync("24px Verdana");
        await context.FillTextAsync("Kliknij spację żeby zacząć grę", 250, 400);
        await context.FillTextAsync("Lub Esc żeby wrócić do Menu", 250, 450);

    }
}
