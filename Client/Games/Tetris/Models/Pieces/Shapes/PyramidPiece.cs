﻿using System.Drawing;

namespace Gejms.Client.Games.Tetris.Models.Pieces.Shapes;

public class PyramidPiece : PieceBase
{
    public PyramidPiece()
    {
        numOfStates = 4; // 0- poiting up; 1 - left; 2 - down; 3 - right
    }

    public override Point[] GetInitialStructure()
    {
        return new Point[4]
        {
            new Point(0, 0),
            new Point(-1, 1),
            new Point(0, 1),
            new Point(1, 1),
        };
    }

    //rotation center is the center sq
    public override void Rotate()
    {
        var currPos = positionComponent.GetCurrentPosition();
        List<int> possibleXOffsets = new List<int> { 0 }; // no offset on start
        var nextPos = new Point[4];

        if (state % numOfStates == 0)
        {
            nextPos = new Point[4]
            {
                new Point(currPos[2].X, currPos[2].Y - 1),
                new Point(currPos[2].X - 1, currPos[2].Y),
                new Point(currPos[2].X, currPos[2].Y),
                new Point(currPos[2].X, currPos[2].Y + 1)
            };
        }
        else if (state % numOfStates == 1)
        {
            nextPos = new Point[4]
            {
                new Point(currPos[2].X - 1, currPos[2].Y),
                new Point(currPos[2].X, currPos[2].Y),
                new Point(currPos[2].X + 1, currPos[2].Y),
                new Point(currPos[2].X, currPos[2].Y +1)
            };
            possibleXOffsets.Add(-1);
        }
        else if (state % numOfStates == 2)
        {
            // here rotation sq is the 2nd one
            nextPos = new Point[4]
            {
                new Point(currPos[1].X, currPos[1].Y - 1),
                new Point(currPos[1].X, currPos[1].Y),
                new Point(currPos[1].X + 1, currPos[1].Y),
                new Point(currPos[1].X, currPos[1].Y + 1)
            };
        }
        else
        {
            // here rotation sq is the 2nd one
            nextPos = new Point[4]
            {
                new Point(currPos[1].X, currPos[1].Y - 1),
                new Point(currPos[1].X -1, currPos[1].Y),
                new Point(currPos[1].X, currPos[1].Y),
                new Point(currPos[1].X + 1, currPos[1].Y)
            };
            possibleXOffsets.Add(1);
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

        return;
    }
}
