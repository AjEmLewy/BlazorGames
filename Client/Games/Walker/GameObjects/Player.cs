using Gejms.Client.Assets;
using Gejms.Client.Games.Tetris;
using Gejms.Client.Shared.GameServices;
using Gejms.Client.Shared.Models;
using Gejms.Client.Shared.Models.GameObjectComponents;
using System.Drawing;

namespace Gejms.Client.Games.Walker.GameObjects;

public class Player : GameObject
{
    private readonly AssetsProviderService assetsProvider;

    public Player()
    {
        Components.Add(new PositionComponent(this, new Point(250,250)));
        assetsProvider = GameServicesCollection.Instance.Get<AssetsProviderService>();
        var sprite = assetsProvider.AssetsCollection.Get<Sprite>("assets/Walker/objects/char_solo.png");
        var spriteRenderComponent = new SpriteRenderComponent(this, sprite);
        Components.Add(spriteRenderComponent);
        Components.Add(new PlayerBrain(this));
    }
}


public class PlayerBrain : IComponent
{
    private readonly PositionComponent pos;
    private readonly InputSystem inputSystem;
    private readonly GameTime gameTime;
    private readonly CameraComponent camera;
    private int speed = 750;

    public PlayerBrain(GameObject owner)
    {
        pos = owner.Components.Get<PositionComponent>();
        inputSystem = GameServicesCollection.Instance.Get<InputSystem>();
        gameTime = GameServicesCollection.Instance.Get<GameTime>();
        camera = GameServicesCollection.Instance.Get<CameraComponent>();
    }
    public ValueTask Iterate(IGame game)
    {
        Move();
        this.camera.SetPosition(pos.Position.X, pos.Position.Y);

        return ValueTask.CompletedTask;
    }

    private void Move()
    {
        if (inputSystem.keyboardKeyStates[(int)KeyboardKey.Right])
        {
            pos.Move((int)(speed * gameTime.MillisFromLastFrame / 1000), 0);
        }
        if (inputSystem.keyboardKeyStates[(int)KeyboardKey.Left])
        {
            pos.Move(-(int)(speed * gameTime.MillisFromLastFrame / 1000), 0);
        }
        if (inputSystem.keyboardKeyStates[(int)KeyboardKey.Up])
        {
            pos.Move(0, -(int)(speed * gameTime.MillisFromLastFrame / 1000));
        }
        if (inputSystem.keyboardKeyStates[(int)KeyboardKey.Down])
        {
            pos.Move(0, (int)(speed * gameTime.MillisFromLastFrame / 1000));
        }
    }
}