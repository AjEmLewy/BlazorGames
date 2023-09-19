using Blazor.Extensions.Canvas.Canvas2D;
using Gejms.Client.Games.Tetris.Models;
using Gejms.Client.Games.Tetris.Models.Pieces;
using Gejms.Client.Games.Tetris.Models.Pieces.Components;
using Gejms.Client.Shared.GameServices;
using Gejms.Client.Shared.GameServices.Interfaces;

namespace Gejms.Client.Games.Tetris.Services;

public class TetrisBoard : IRenderable, IGameService, IRestartable, IIterable
{
    private PieceBase? currentPiece = null;
    private readonly PieceFactory pieceFactory;
    private readonly AudioPlayer audioPlayer;
    private readonly ScoreService scoreService;
    private bool shouldGameEnd = false;

    public TetrisSquare[,] Board { get; }
    public int BoardWidth { get; }
    public int BoardHeight { get; }
    public int ColAmount { get; }
    public int RowAmount { get; }

    public TetrisBoard(int boardWidth, int boardHeight, int colAmount, int rowAmount)
    {
        BoardWidth = boardWidth;
        BoardHeight = boardHeight;
        ColAmount = colAmount;
        RowAmount = rowAmount;

        Board = new TetrisSquare[ColAmount, RowAmount];
        BuildEmptyBoard();

        pieceFactory = GameServicesCollection.Instance.Get<PieceFactory>();
        audioPlayer = GameServicesCollection.Instance.Get<AudioPlayer>();
        scoreService = GameServicesCollection.Instance.Get<ScoreService>();
    }

    private void BuildEmptyBoard()
    {
        for (int c = 0; c < ColAmount; c++)
            for (int r = 0; r < RowAmount; r++)
                Board[c, r] = new TetrisSquare(c, r);
    }

    public void Restart()
    {
        shouldGameEnd = false;
        currentPiece = null;
        BuildEmptyBoard();
    }

    public async ValueTask Iterate(IGame game)
    {
        if (shouldGameEnd)
            await game.EndGame();
        else
            if (currentPiece == null)
            currentPiece = pieceFactory.SpawnNextPiece();
        await currentPiece.Update(game);
    }

    public async ValueTask AddPieceToBoard(PieceBase piece)
    {
        var coords = piece.components.Get<PiecePositionComponent>().GetCurrentPosition();
        var sprite = piece.parts[0].Sprite;

        foreach (var coord in coords)
        {
            Board[coord.X, coord.Y].Taken = true;
            Board[coord.X, coord.Y].Sprite = sprite;
        }

        currentPiece = null;

        var completedRows = GetCompleteLines();
        if (completedRows.Count > 0)
        {
            int pointsToAdd = (int)(10 * Math.Pow(completedRows.Count, 2.5));
            scoreService.AddPoints(pointsToAdd);
            RemoveFullLines(completedRows);
            await audioPlayer.PlaySound("julek_pa.mp3");
        }
    }

    public async ValueTask Render(Canvas2DContext context, IGame game)
    {
        var takenSquares = Board.Cast<TetrisSquare>().Where(b => b.Taken).ToArray();

        await context.SetFillStyleAsync("lightgreen");
        await context.FillRectAsync(0, 0, 500, 700);

        await context.SetFillStyleAsync("gray");
        foreach (var ts in takenSquares)
            await ts.Render(context, game);
        if (currentPiece != null)
            await currentPiece.Render(context, game);
    }


    private IList<int> GetCompleteLines()
    {
        List<int> fullRows = new();
        for (int r = 0; r < RowAmount; r++)
        {
            int takenCount = 0;
            for (int c = 0; c < ColAmount; c++)
            {
                if (Board[c, r].Taken)
                    ++takenCount;
            }
            if (takenCount == 10)
                fullRows.Add(r);
        }
        return fullRows;
    }

    private void RemoveFullLines(IList<int> rows)
    {
        foreach (int row in rows)
        {
            for (int r = row; r > 0; r--)
                for (int c = 0; c < ColAmount; c++)
                {
                    Board[c, r].Taken = Board[c, r - 1].Taken;
                    Board[c, r].Sprite = Board[c, r - 1].Sprite;
                }
        }
    }

    public void EndGame()
    {
        shouldGameEnd = true;
    }
}