using Gejms.Client.Assets;
using Gejms.Client.Games.Tetris.Models.Pieces;
using Gejms.Client.Shared.GameServices.Interfaces;
using System.Drawing;
using System.Reflection;

namespace Gejms.Client.Games.Tetris.Services;

public class PieceFactory : IGameService, IRestartable
{
    private readonly IList<Type> pieceTypes;
    private readonly IAssetsCollection assetsCollection;
    private PieceBase nextPiece;
    public Point[] nextPieceInitStructure { get; private set; }

    public PieceFactory(IAssetsCollection assetsCollection)
    {
        pieceTypes = Assembly.GetAssembly(typeof(PieceBase))?.GetTypes()
            .Where(t => t.IsClass && !t.IsAbstract && t.IsSubclassOf(typeof(PieceBase))).ToList();
        this.assetsCollection = assetsCollection;
    }

    public PieceBase SpawnNextPiece()
    {
        int randomInt;

        if (nextPiece == null)
        {
            randomInt = Random.Shared.Next(0, pieceTypes.Count);
            nextPiece = Activator.CreateInstance(pieceTypes[randomInt]) as PieceBase;
        }
        var newPiece = nextPiece;

        randomInt = Random.Shared.Next(0, pieceTypes.Count);
        nextPiece = Activator.CreateInstance(pieceTypes[randomInt]) as PieceBase;
        nextPieceInitStructure = nextPiece.GetInitialStructure();
        OnNextPieceCreated?.Invoke();

        newPiece?.Spawn(assetsCollection);
        return newPiece;
    }

    public void Restart()
    {
        nextPiece = null;
    }

    public event NextPieceChangedHandler OnNextPieceCreated;
    public delegate void NextPieceChangedHandler();
}