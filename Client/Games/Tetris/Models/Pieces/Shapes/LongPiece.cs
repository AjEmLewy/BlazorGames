using System.Drawing;

namespace Gejms.Client.Games.Tetris.Models.Pieces.Shapes;

public class LongPiece : PieceBase
{
    //state 0 = up-down
    //state 1 == left-right
    // the rotation center in the second from last part when in top-down and the same for left-right
    public LongPiece()
    {
        numOfStates = 2;
    }
    public override Point[] GetInitialStructure()
    {
        return new Point[4]
        {
            new Point(0, 0),
            new Point(0, 1),
            new Point(0, 2),
            new Point(0, 3),
        };
    }

    public override void Rotate()
    {
        var currPos = positionComponent.GetCurrentPosition();
        List<int> possibleXOffsets = new();
        var nextPos = new Point[4];

        if (state % numOfStates == 0)
        {
            // up - down
            nextPos = new Point[4]
            {
                new Point(currPos[2].X -2, currPos[2].Y),
                new Point(currPos[2].X -1, currPos[2].Y),
                new Point(currPos[2].X, currPos[2].Y),
                new Point(currPos[2].X +1, currPos[2].Y)
            };
            possibleXOffsets.AddRange(new int[4] { 0, 1, 2, -1 });
        }
        else
        {
            // left - right
            nextPos = new Point[4]
            {
                new Point(currPos[2].X, currPos[2].Y -2),
                new Point(currPos[2].X, currPos[2].Y -1),
                new Point(currPos[2].X, currPos[2].Y),
                new Point(currPos[2].X, currPos[2].Y +1)
            };
            possibleXOffsets.Add(0);
        }

        foreach (var xOff in possibleXOffsets)
        {
            if (positionComponent.ValidatePiecePlacement(nextPos, xOff, 0))
            {
                positionComponent.SetNewPosition(nextPos, xOff, 0);
                state += 1;
                break;
            }
        }
    }
}
