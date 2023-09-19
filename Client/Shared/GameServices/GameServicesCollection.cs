using Gejms.Client.Games.Tetris;
using Gejms.Client.Shared.GameServices.Interfaces;

namespace Gejms.Client.Shared.GameServices;

public class GameServicesCollection
{
    private IDictionary<Type, IGameService> gameServices { get; }

    private static readonly Lazy<GameServicesCollection> instance = new Lazy<GameServicesCollection>(new GameServicesCollection());
    public static GameServicesCollection Instance => instance.Value;

    private GameServicesCollection()
    {
        gameServices = new Dictionary<Type, IGameService>();
    }

    public void Add<T>(T gameService) where T : class, IGameService
    {
        var type = typeof(T);
        if (!gameServices.ContainsKey(typeof(T)))
            gameServices.Add(typeof(T), null);
        gameServices[typeof(T)] = gameService;
    }

    public T Get<T>() where T : class, IGameService
    {
        var type = typeof(T);
        if (!gameServices.ContainsKey(type))
            Console.WriteLine("You are trying to get the service that is not in collection: " + type.ToString());
        return gameServices[type] as T;
    }

    public List<IGameService> GetAllServices()
    {
        return gameServices.Values.ToList();
    }

    public void RestartServices()
    {
        var services = GetAllServices();
        foreach (var s in services)
            if (s is IRestartable restartable)
                restartable.Restart();
    }

    public async ValueTask IterateServices(IGame game)
    {
        var services = GetAllServices();
        foreach (var s in services)
            if (s is IIterable iterable)
                await iterable.Iterate(game);
    }
}
