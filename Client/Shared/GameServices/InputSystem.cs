using Gejms.Client.Shared.GameServices.Interfaces;
using System.Drawing;

namespace Gejms.Client.Shared.GameServices;
public class InputSystem : IGameService
{
    public IDictionary<int, bool> keyboardKeyStates = new Dictionary<int, bool>();

    public IDictionary<int, MouseKeyState> mouseKeyStates = new Dictionary<int, MouseKeyState>();

    public Point MouseCoords = new Point(0,0);
    public InputSystem()
    {
        var keyboardKeys = Enum.GetValues(typeof(KeyboardKey)).Cast<int>().ToList();
        foreach (var key in keyboardKeys)
        {
            keyboardKeyStates[key] = false;
        }

        var mouseKeys = Enum.GetValues(typeof(MouseKey)).Cast<int>().ToList();
        foreach(var key in mouseKeys)
        {
            mouseKeyStates[key] = new MouseKeyState();
        }
    }

    public void HandleKeyboardKeyUp(int keyCode) => keyboardKeyStates[keyCode] = false;

    public void HandleKeyboardKeyDown(int keyCode) => keyboardKeyStates[keyCode] = true;

    public void HandleMouseKeyDown(int keyCode) => mouseKeyStates[keyCode].IsPressed = true;

    public void HandleMouseKeyUp(int keyCode) => mouseKeyStates[keyCode].IsPressed = false;

    public void HandleMouseMove(int xPos, int yPos)
    {
        MouseCoords.X = xPos;
        MouseCoords.Y = yPos;
    }
}

public class MouseKeyState
{
    public bool IsPressed { get; set; } = false;
}

public enum KeyboardKey
{
    Left = 37,
    Up = 38,
    Right = 39,
    Down = 40,
    Space = 32,
    R = 82,
    Enter = 13,
    Esc = 27
}

public enum MouseKey
{
    Left = 0,
    Right = 2,
}
