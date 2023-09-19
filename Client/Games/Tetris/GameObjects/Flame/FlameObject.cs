using Gejms.Client.Assets;
using Gejms.Client.Shared.GameServices;
using Gejms.Client.Shared.Models;
using Gejms.Client.Shared.Models.GameObjectComponents;
using System.Drawing;

namespace Gejms.Client.Games.Tetris.GameObjects.Flame;

public class FlameObject : GameObject
{
    public FlameObject(Point initialPosition)
    {
        var assetsProvider = GameServicesCollection.Instance.Get<AssetsProviderService>();
        var flamePosition = new PositionComponent(this, initialPosition);
        Components.Add(flamePosition);

        var castingSpriteSheet = assetsProvider.AssetsCollection.Get<SpriteSheet>("casting.png");
        var flameAnimatedComponent = new AnimatedRenderComponent(this, castingSpriteSheet);
        Components.Add(flameAnimatedComponent);

        Components.Add(new FlameBrainComponent(this));
    }
}
