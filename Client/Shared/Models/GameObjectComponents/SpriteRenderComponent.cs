using Blazor.Extensions.Canvas.Canvas2D;
using Gejms.Client.Assets;
using Gejms.Client.Games.Tetris;
using Gejms.Client.Shared.GameServices;
using Gejms.Client.Shared.GameServices.Interfaces;
using System.Drawing;

namespace Gejms.Client.Shared.Models.GameObjectComponents;

public class SpriteRenderComponent : IComponent, IRenderable
{
    private readonly Sprite sprite;
    private readonly PositionComponent pos;
    private readonly CameraComponent camera;

    public SpriteRenderComponent(GameObject owner, Sprite sprite)
    {
        this.sprite = sprite;
        pos = owner.Components.Get<PositionComponent>();
        camera = GameServicesCollection.Instance.Get<CameraComponent>();
    }
    public ValueTask Iterate(IGame game)
    {
        return ValueTask.CompletedTask;
    }

    public async ValueTask Render(Canvas2DContext context, IGame game)
    {
        Point camTopLeft = camera.GetTopLeftCorner();
        int destX = pos.Position.X - sprite.Size.Width / 2 - camTopLeft.X;
        int destY = pos.Position.Y - sprite.Size.Height / 2 - camTopLeft.Y;
        await context.DrawImageAsync(sprite.ElementRef,
            destX, destY, sprite.Size.Width, sprite.Size.Height);
    }
}
