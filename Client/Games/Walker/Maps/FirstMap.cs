using Blazor.Extensions.Canvas.Canvas2D;
using Gejms.Client.Assets;
using Gejms.Client.Games.Tetris;
using Gejms.Client.Shared.GameServices;
using Gejms.Client.Shared.GameServices.Interfaces;
using System.Diagnostics;
using System.Drawing;

namespace Gejms.Client.Games.Walker.Maps;

public class FirstMap : IRenderable
{
    private readonly int cols = 40;
    private readonly int rows = 40;
    private readonly Canvas2DContext hidden;
    private readonly IGame game;
    private bool firstRender = true;

    private Tile[,] tiles;

    private CameraComponent camera;

    private Stopwatch stopwatch = new Stopwatch();

    public FirstMap(Canvas2DContext hidden, IGame game)
    {
        camera = GameServicesCollection.Instance.Get<CameraComponent>();
        this.hidden = hidden;
        this.game = game;

        tiles = new Tile[cols, rows];
        for (int c = 0; c < cols; c++)
            for (int r = 0; r < rows; r++)
            {
                int type = Random.Shared.Next(1, 6);
                tiles[c, r] = new Tile(type, c, r);
            }
    }

    public async ValueTask PrepareMap()
    {
        await Task.Delay(60);
        await hidden.BeginBatchAsync();
        for (int c = 0; c < cols; c++)
            for (int r = 0; r < rows; r++)
                await tiles[c, r].Render(hidden, game);
        await hidden.EndBatchAsync();
    }

    public async ValueTask Render(Canvas2DContext backgroundContext, IGame game)
    {
        if (firstRender)
        {
            await Task.Delay(600);
            firstRender = false;
        }
        stopwatch.Restart();
        await backgroundContext.BeginBatchAsync();

        //await backgroundContext.SaveAsync();

        var cameraLeftTopCorner = camera.GetTopLeftCorner();

        //await backgroundContext.TranslateAsync(-cameraLeftTopCorner.X, -cameraLeftTopCorner.Y);

        int xTileStart = (int)cameraLeftTopCorner.X;
        int yTileStart = (int)cameraLeftTopCorner.Y;
        await backgroundContext.DrawImageAsync(hidden.Canvas, xTileStart, yTileStart, 640, 640, 0, 0, 640, 640);

        //await backgroundContext.RestoreAsync();

        await backgroundContext.EndBatchAsync();
        //camera.HasMoved = false;
        //Console.WriteLine("copying normal: " + stopwatch.ElapsedMilliseconds);
    }

    /*
    //z hidden wolniej niż directly na background. albo robie cos zle xd
    public async ValueTask Render(Canvas2DContext backgroundContext, IGame game)
    {
        if(firstRender)
        {
            await Task.Delay(60);
            firstRender = false;
        }
        stopwatch.Restart();
        await backgroundContext.BeginBatchAsync();

        await backgroundContext.SaveAsync();

        var cameraLeftTopCorner = camera.GetTopLeftCorner();
        var cameraRightBottom = camera.GetBottomRightCorner();

        await backgroundContext.TranslateAsync(-cameraLeftTopCorner.X, -cameraLeftTopCorner.Y);

        int xTileStart = (int) cameraLeftTopCorner.X / 64;
        int yTileStart = (int) cameraLeftTopCorner.Y / 64;

        int xTileEnd = (int)Math.Ceiling((double)cameraRightBottom.X / 64);
        int yTileEnd = (int)Math.Ceiling((double)cameraRightBottom.Y / 64);
        for (int c = xTileStart; c < xTileEnd; c++)
            for(int r = yTileStart; r < yTileEnd; r++)
                await tiles[c,r].Render(backgroundContext, game);
        await backgroundContext.RestoreAsync();
            
        await backgroundContext.EndBatchAsync();
        //camera.HasMoved = false;

        Console.WriteLine("rendering normal: " + stopwatch.ElapsedMilliseconds);
    }
    */
}


public class Tile : IRenderable
{
    private int type { get; set; }
    private Sprite sprite;
    private Point position;
    private Point drawPoint;

    public Tile(int type, int col, int row)
    {
        var assetResolver = GameServicesCollection.Instance.Get<AssetsProviderService>();
        this.type = type;
        sprite = assetResolver.AssetsCollection.Get<Sprite>("assets/Walker/tiles/"+type+".png");

        position = new Point((int)(sprite.Size.Width * (col + 0.5)), (int)(sprite.Size.Height * (row + 0.5)));
        drawPoint = new Point(position.X - sprite.Size.Width / 2, position.Y - sprite.Size.Height / 2);
    }

    public async ValueTask Render(Canvas2DContext context, IGame game)
    {
        await context.DrawImageAsync(sprite.ElementRef,
            drawPoint.X, drawPoint.Y, sprite.Size.Width, sprite.Size.Height);
    }
}