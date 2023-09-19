using Gejms.Client.Games.Tetris.Services;
using Gejms.Client.Shared.GameServices;
using Gejms.Client.Shared.Models.GameObjectComponents;
using System.Drawing;

namespace Gejms.Client.Games.Tetris.Models.Pieces.Components;

public class PiecePositionComponent : IComponent
{
    private readonly PieceBase owner;
    private readonly TetrisBoard board;
    public Point[] landingPoints = new Point[4];

    public PiecePositionComponent(PieceBase owner)
    {
        this.owner = owner;
        board = GameServicesCollection.Instance.Get<TetrisBoard>();

        var initStructure = owner.GetInitialStructure();
        for (int i = 0; i < 4; i++)
            owner.parts[i] = new TetrisSquare(5 + initStructure[i].X, 0 + initStructure[i].Y);

        if (!ValidatePiecePlacement(GetCurrentPosition(), 0, 0)) board.EndGame();
        UpdateLandingPoints();
    }

    public ValueTask Iterate(IGame game)
    {
        return ValueTask.CompletedTask;
    }

    public void Rotate()
    {
        owner.Rotate();
    }


    public async ValueTask Move(int xOffset, int yOffset)
    {
        var currPos = GetCurrentPosition();

        if (ValidatePiecePlacement(currPos, xOffset, yOffset))
            SetNewPosition(currPos, xOffset, yOffset);
        else
        {
            if (yOffset == 1)
                await board.AddPieceToBoard(owner);
        }
    }

    public void UpdateLandingPoints()
    {
        Point[] currPos = GetCurrentPosition();
        int yOffset = 0;
        while (ValidatePiecePlacement(currPos, 0, yOffset + 1))
            ++yOffset;
        landingPoints = currPos.Select(p => new Point(p.X, p.Y + yOffset)).ToArray();
    }

    public async void Land()
    {
        SetNewPosition(landingPoints, 0, 0);
        await board.AddPieceToBoard(owner);
    }

    public Point[] GetCurrentPosition()
    {
        Point[] currPos = new Point[4] {
                new Point(owner.parts[0].xPos, owner.parts[0].yPos),
                new Point(owner.parts[1].xPos, owner.parts[1].yPos),
                new Point(owner.parts[2].xPos, owner.parts[2].yPos),
                new Point(owner.parts[3].xPos, owner.parts[3].yPos)
            };
        return currPos;
    }

    public void SetNewPosition(Point[] newCoords, int xOffset, int yOffset)
    {
        for (int i = 0; i < 4; ++i)
        {
            owner.parts[i].xPos = newCoords[i].X + xOffset;
            owner.parts[i].yPos = newCoords[i].Y + yOffset;
        }
        UpdateLandingPoints();
    }

    public bool ValidatePiecePlacement(Point[] newPositions, int xOffset, int yOffset)
    {
        var xs = newPositions.Select(pos => pos.X);
        var ys = newPositions.Select(pos => pos.Y);

        bool incorrectXs = xs.Any(x => x + xOffset < 0 || x + xOffset >= board.ColAmount);
        bool incorrectYs = ys.Any(y => y + yOffset < 0 || y + yOffset >= board.RowAmount);

        if (incorrectXs || incorrectYs)
            return false;


        bool colliding = newPositions.Where(pos => board.Board[pos.X + xOffset, pos.Y + yOffset].Taken).Any();
        if (colliding)
            return false;

        return true;
    }
}
