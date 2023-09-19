using Gejms.Client.Games.Tetris;
using Gejms.Client.Shared.GameServices.Interfaces;
using System.Drawing;

namespace Gejms.Client.Shared.GameServices;

public class CameraComponent : IGameService, IIterable
{
    private Point Position;
    public Size Size { get; private set; }
    public bool HasMoved { get; set; } = true;

    private readonly InputSystem inputSystem;
    private readonly GameTime gameTime;
    private float cameraSpeed = 250;

    public CameraComponent(Point initialPos)
    {
        Position = initialPos;
        Size = new Size(640, 640);

        inputSystem = GameServicesCollection.Instance.Get<InputSystem>();
        gameTime = GameServicesCollection.Instance.Get<GameTime>();
    }

    public ValueTask Iterate(IGame game)
    {
        /**
        var prevPos = Position;

        int xMove = 0;
        int yMove = 0;
        
        if (inputSystem.keyboardKeyStates[(int)KeyboardKey.Left])
            xMove -= 1;
        if (inputSystem.keyboardKeyStates[(int)KeyboardKey.Right])
            xMove += 1;

        if (inputSystem.keyboardKeyStates[(int)KeyboardKey.Up])
            yMove -= 1;
        if (inputSystem.keyboardKeyStates[(int)KeyboardKey.Down])
            yMove += 1;
        if(yMove != 0 || xMove !=0)
        {
            Position.Offset((int)(xMove * gameTime.MillisFromLastFrame * cameraSpeed/1000), (int)(yMove * gameTime.MillisFromLastFrame * cameraSpeed/1000));
            clipPosition();
        }

        if(prevPos.X != Position.X || prevPos.Y != Position.Y)
            HasMoved = true;
        */

        return ValueTask.CompletedTask;
    }

    private void clipPosition()
    {
        if (Position.X > (6400 - Size.Width / 2))
            Position.X = (6400 - Size.Width / 2);
        else if (Position.X < Size.Width / 2)
            Position.X = Size.Width / 2;

        if (Position.Y > (6400 - Size.Width / 2))
            Position.Y = (6400 - Size.Width / 2);
        else if (Position.Y < Size.Width / 2)
            Position.Y = Size.Width / 2;
    }

    public Point GetTopLeftCorner() => new Point(Position.X - Size.Width/2, Position.Y - Size.Height/2);
    public Point GetBottomRightCorner() => new Point(Position.X + Size.Width/2, Position.Y + Size.Height/2);

    public void SetPosition(int x, int y)
    {
        var prevPos = Position;

        this.Position = new Point(x, y);
        clipPosition();
        if (prevPos.X != Position.X || prevPos.Y != Position.Y)
            HasMoved = true;

    }
}
