using Blazor.Extensions.Canvas.Canvas2D;
using Gejms.Client.Assets;
using Gejms.Client.Games.Tetris.Screens;
using Gejms.Client.Games.Tetris.Services;
using Gejms.Client.Shared.GameServices;
using Microsoft.JSInterop;

namespace Gejms.Client.Games.Tetris;

public class TetrisGame : IGame
{
    private Canvas2DContext context;
    private readonly IAssetsCollection assetsCollection;
    private readonly IJSRuntime JsRuntime;
    private readonly HttpClient httpClient;
    private TetrisScreenManager screenManager;

    public TetrisGame(Canvas2DContext context, IAssetsCollection assetsCollection, IJSRuntime jsRuntime, HttpClient httpClient)
    {
        this.context = context;
        this.assetsCollection = assetsCollection;
        JsRuntime = jsRuntime;
        this.httpClient = httpClient;
    }

    public async ValueTask InitGame()
    {
        GameServicesCollection.Instance.Add(new AssetsProviderService(assetsCollection));
        GameServicesCollection.Instance.Add(new TetrisHttpClient(httpClient));
        GameServicesCollection.Instance.Add(new ScoreService());
        GameServicesCollection.Instance.Add(new GameTime());
        GameServicesCollection.Instance.Add(new AudioPlayer(JsRuntime));
        GameServicesCollection.Instance.Add(new InputSystem());
        GameServicesCollection.Instance.Add(new PieceFactory(assetsCollection));
        GameServicesCollection.Instance.Add(new TetrisBoard(350, 700, 10, 20));

        screenManager = new TetrisScreenManager();
        screenManager.TransitionToScreen<HomeScreen>();
    }


    public async ValueTask EndGame()
    {
        screenManager.TransitionToScreen<EndScreen>();
    }

    public void StartGame()
    {
    }

    public async ValueTask Iterate()
    {
        await screenManager.CurrentScreen.Iterate(this);
        await Render();
    }

    public async ValueTask Render()
    {
        await context.ClearRectAsync(0, 0, 500, 700);

        await context.BeginBatchAsync();
        await screenManager.CurrentScreen.Render(context, this);
        await context.EndBatchAsync();
    }
}
