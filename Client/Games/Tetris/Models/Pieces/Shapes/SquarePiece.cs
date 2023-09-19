using System.Drawing;

namespace Gejms.Client.Games.Tetris.Models.Pieces.Shapes;

public class SquarePiece : PieceBase
{
    public SquarePiece()
    {
        numOfStates = 1;
    }

    public override void Rotate()
    {
        return;
    }

    public override Point[] GetInitialStructure()
    {
        return new Point[4]
        {
            new Point(0, 0),
            new Point(1, 0),
            new Point(0, 1),
            new Point(1, 1),
        };
    }
}
