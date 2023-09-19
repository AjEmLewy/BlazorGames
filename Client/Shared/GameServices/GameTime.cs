using Gejms.Client.Games.Tetris;
using Gejms.Client.Shared.GameServices.Interfaces;
using System.Diagnostics;

namespace Gejms.Client.Shared.GameServices;

public class GameTime : IGameService, IRestartable, IIterable
{
    private readonly Stopwatch stopwatch;

    private float millisFromLastFrame = 0f;
    private float totalMillis = 0f;
    public float MillisFromLastFrame { get => millisFromLastFrame; }
    public float TotalMillis { get => totalMillis; }

    public GameTime()
    {
        stopwatch = new Stopwatch();
    }

    public ValueTask Iterate(IGame game)
    {
        var currentMillis = stopwatch.ElapsedMilliseconds;
        millisFromLastFrame = currentMillis - TotalMillis;
        totalMillis = currentMillis;
        return ValueTask.CompletedTask;
    }

    public void Restart()
    {
        stopwatch.Restart();
        millisFromLastFrame = 0f;
        totalMillis = 0f;
    }
}
