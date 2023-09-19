using Blazor.Extensions.Canvas.Canvas2D;
using Gejms.Client.Games.Tetris;
using Gejms.Client.Shared.GameServices.Interfaces;
using Gejms.Client.Shared.Models.GameObjectComponents;

namespace Gejms.Client.Games.Tetris.Models.Pieces.Components;

public class LandingTip : IComponent, IRenderable
{
    private readonly PieceBase owner;
    private readonly PiecePositionComponent positionComp;

    public LandingTip(PieceBase owner)
    {
        this.owner = owner;
        positionComp = owner.components.Get<PiecePositionComponent>();
    }
    public async ValueTask Render(Canvas2DContext context, IGame game)
    {
        foreach (var lp in positionComp.landingPoints)
        {
            await context.BeginPathAsync();
            await context.SetStrokeStyleAsync("rgb(255,255,0)");
            await context.SetLineWidthAsync(3);
            await context.StrokeRectAsync(lp.X * owner.parts[0].Size.Width, lp.Y * owner.parts[0].Size.Width, owner.parts[0].Size.Width, owner.parts[0].Size.Width);
        }
    }

    public ValueTask Iterate(IGame game)
    {
        return ValueTask.CompletedTask;
    }
}
