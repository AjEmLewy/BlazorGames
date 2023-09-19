using System.Drawing;
using Gejms.Client.Games.Tetris;
using Gejms.Client.Shared.GameServices;
using Gejms.Client.Shared.Models;

namespace Gejms.Client.Shared.Models.GameObjectComponents;

public class PositionComponent : IComponent
{
    private readonly GameObject owner;
    public Point Position;

    public PositionComponent(GameObject owner, Point initialPosition)
    {
        this.owner = owner;
        Position = initialPosition;
    }
    public ValueTask Iterate(IGame game)
    {
        return ValueTask.CompletedTask;
    }

    public void Move(int x, int y)
    {
        Position.X += x;
        Position.Y += y;
    }

    public void SetPosition(int x, int y)
    {
        Position.X = x;
        Position.Y = y;
    }
}
