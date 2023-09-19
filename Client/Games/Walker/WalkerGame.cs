using Blazor.Extensions.Canvas.Canvas2D;
using Gejms.Client.Assets;
using Gejms.Client.Games.Tetris;
using Gejms.Client.Games.Walker.GameObjects;
using Gejms.Client.Games.Walker.Maps;
using Gejms.Client.Shared.GameServices;
using Gejms.Client.Shared.Models;
using Microsoft.JSInterop;
using System.Diagnostics;
using System.Drawing;

namespace Gejms.Client.Games.Walker;

public class WalkerGame : IGame
{
    private readonly Canvas2DContext hiddenContext;
    private readonly Canvas2DContext backgroundContext;
    private readonly Canvas2DContext foregroundContext;
    private readonly IAssetsCollection assetsCollection;
    private readonly IJSRuntime jSRuntime;
    private readonly HttpClient httpClient;
    private CameraComponent camera;
    private GameTime gameTime;

    private readonly List<GameObject> objects = new();
    private FirstMap map;

    private Stopwatch stopwatch = new Stopwatch();

    public WalkerGame(Canvas2DContext[] contexts, IAssetsCollection assetsCollection, IJSRuntime jSRuntime, HttpClient httpClient)
    {
        this.hiddenContext = contexts[0];
        this.backgroundContext = contexts[1];
        this.foregroundContext = contexts[2];
        this.assetsCollection = assetsCollection;
        this.jSRuntime = jSRuntime;
        this.httpClient = httpClient;
    }
    

    public async ValueTask InitGame()
    {
        GameServicesCollection.Instance.Add(new InputSystem());
        GameServicesCollection.Instance.Add(new GameTime());
        gameTime = GameServicesCollection.Instance.Get<GameTime>();
        gameTime.Restart();
        camera = new CameraComponent(new Point(320, 320));
        GameServicesCollection.Instance.Add(camera);
        GameServicesCollection.Instance.Add(new AssetsProviderService(assetsCollection));
        
        map = new FirstMap(hiddenContext, this);
        await map.PrepareMap();

        objects.Add(new Player());

        //return ValueTask.CompletedTask;
    }

    public ValueTask EndGame()
    {
        return ValueTask.CompletedTask;
    }

    public async ValueTask Iterate()
    {
        await GameServicesCollection.Instance.IterateServices(this);
        foreach (var obj in objects)
            await obj.Iterate(this);

        await Render();
    }

    public async ValueTask Render()
    {
        stopwatch.Restart();
        await foregroundContext.ClearRectAsync(0, 0, 640, 640);

        await foregroundContext.BeginBatchAsync();

        foreach (var obj in objects)
            await obj.Render(foregroundContext, this);

        await foregroundContext.EndBatchAsync();

        if(camera.HasMoved)
            await map.Render(backgroundContext, this);
    }
}
