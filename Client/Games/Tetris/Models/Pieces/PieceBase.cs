using Blazor.Extensions.Canvas.Canvas2D;
using Gejms.Client.Assets;
using Gejms.Client.Games.Tetris.Models.Pieces.Components;
using Gejms.Client.Shared.GameServices.Interfaces;
using Gejms.Client.Shared.Models.GameObjectComponents;
using System.Drawing;

namespace Gejms.Client.Games.Tetris.Models.Pieces;

public abstract class PieceBase : IRenderable, IPiece
{
    // Parts are always numbered top -> down and on the same level left -> right
    // we do not allow up - down offsetting. Maybe in the future. it would generate endless changing

    public ComponentsCollection components = new();
    protected PiecePositionComponent positionComponent;
    public TetrisSquare[] parts = new TetrisSquare[4];

    protected int state = 0;
    protected int numOfStates;


    public PieceBase()
    {

    }

    public void Spawn(IAssetsCollection assetsCollection)
    {
        positionComponent = new PiecePositionComponent(this);
        components.Add(positionComponent);

        var landingTipComponent = new LandingTip(this);
        components.Add(landingTipComponent);

        var pieceBrain = new PieceBrain(this);

        var sprite = assetsCollection.GetRandom<Sprite>();
        var spriteRenderComponent = new PieceRenderComponent(this, sprite);

        components.Add(pieceBrain);
        components.Add(spriteRenderComponent);
    }

    public async ValueTask Update(IGame game)
    {
        foreach (IComponent comp in components)
        {
            await comp.Iterate(game);
        }

    }

    //return initial points relative to sq (5,0)
    public abstract Point[] GetInitialStructure();

    public async ValueTask Render(Canvas2DContext context, IGame game)
    {
        foreach (var comp in components)
            if (comp is IRenderable renderable)
                await renderable.Render(context, game);
    }

    public abstract void Rotate();
}
