using Blazor.Extensions.Canvas.Canvas2D;
using Gejms.Client.Games.Tetris;
using Gejms.Client.Shared.GameServices.Interfaces;
using Gejms.Client.Shared.Models.GameObjectComponents;

namespace Gejms.Client.Shared.Models;

public class GameObject : IIterable
{
    public ComponentsCollection Components { get; set; } = new();

    public async ValueTask Iterate(IGame game)
    {
        foreach (IComponent component in Components)
            await component.Iterate(game);
    }

    public async ValueTask Render(Canvas2DContext context, IGame game)
    {
        foreach (var component in Components)
            if (component is IRenderable renderable)
                await renderable.Render(context, game);
    }
}
