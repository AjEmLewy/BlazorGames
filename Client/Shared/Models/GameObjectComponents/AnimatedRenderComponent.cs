using Blazor.Extensions.Canvas.Canvas2D;
using Gejms.Client.Assets;
using Gejms.Client.Games.Tetris;
using Gejms.Client.Shared.GameServices;
using Gejms.Client.Shared.GameServices.Interfaces;

namespace Gejms.Client.Shared.Models.GameObjectComponents;

public class AnimatedRenderComponent : IComponent, IRenderable
{
    private int currentFrameIndex = 0;
    private readonly GameTime gameTime;
    private readonly SpriteSheet spriteSheet;
    private readonly PositionComponent pos;
    private float lastSpriteChangeTime;

    public AnimatedRenderComponent(GameObject owner, SpriteSheet spriteSheet)
    {
        gameTime = GameServicesCollection.Instance.Get<GameTime>();
        this.spriteSheet = spriteSheet;
        pos = owner.Components.Get<PositionComponent>();
        lastSpriteChangeTime = gameTime.TotalMillis;
    }

    public async ValueTask Render(Canvas2DContext context, IGame game)
    {
        await context.DrawImageAsync(spriteSheet.ElementRef,
            spriteSheet.Sprites[currentFrameIndex].SpriteSheetPos.X, spriteSheet.Sprites[currentFrameIndex].SpriteSheetPos.Y, spriteSheet.FrameSize.Width, spriteSheet.FrameSize.Height,
            pos.Position.X - spriteSheet.FrameSize.Width / 2, pos.Position.Y - spriteSheet.FrameSize.Height / 2, spriteSheet.FrameSize.Width, spriteSheet.FrameSize.Height);
    }

    public ValueTask Iterate(IGame game)
    {
        if (gameTime.TotalMillis - lastSpriteChangeTime > 1000 / spriteSheet.FPS)
        {
            currentFrameIndex = (currentFrameIndex + 1) % spriteSheet.FramesCount;
            lastSpriteChangeTime = gameTime.TotalMillis;
        }
        return ValueTask.CompletedTask;
    }
}
