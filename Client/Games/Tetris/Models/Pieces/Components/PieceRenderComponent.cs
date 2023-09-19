using Blazor.Extensions.Canvas.Canvas2D;
using Gejms.Client.Assets;
using Gejms.Client.Shared.GameServices.Interfaces;
using Gejms.Client.Shared.Models.GameObjectComponents;

namespace Gejms.Client.Games.Tetris.Models.Pieces.Components;

public class PieceRenderComponent : IComponent, IRenderable
{
    private readonly PieceBase owner;
    private readonly PiecePositionComponent position;

    public PieceRenderComponent(PieceBase owner, Sprite sprite)
    {
        this.owner = owner;
        position = owner.components.Get<PiecePositionComponent>();
        foreach (var part in owner.parts)
            part.Sprite = sprite;
    }
    public async ValueTask Render(Canvas2DContext context, IGame game)
    {
        await context.SaveAsync();
        foreach (var t in owner.parts)
            await t.Render(context, game);


        await context.SetFillStyleAsync("yellow");


        await context.RestoreAsync();
    }

    public ValueTask Iterate(IGame game)
    {
        return ValueTask.CompletedTask;
    }
}
