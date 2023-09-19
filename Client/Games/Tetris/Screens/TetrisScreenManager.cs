using System.Reflection;

namespace Gejms.Client.Games.Tetris.Screens;

public class TetrisScreenManager
{
    private readonly List<ScreenBase> screens = new();

    public ScreenBase CurrentScreen { get; private set; }

    public TetrisScreenManager()
    {
        var screenTypes = Assembly.GetAssembly(typeof(ScreenBase))?.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(ScreenBase))).ToList();
        foreach (var type in screenTypes)
        {
            screens.Add(Activator.CreateInstance(type, new object[] { this }) as ScreenBase);
        }
    }

    public void TransitionToScreen<T>() where T : ScreenBase
    {
        CurrentScreen = screens.Where(s => s.GetType() == typeof(T)).FirstOrDefault();
        CurrentScreen.Load();
    }
}
