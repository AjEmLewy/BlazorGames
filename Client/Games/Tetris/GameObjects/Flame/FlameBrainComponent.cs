using Gejms.Client.Shared.GameServices;
using Gejms.Client.Shared.Models;
using Gejms.Client.Shared.Models.GameObjectComponents;

namespace Gejms.Client.Games.Tetris.GameObjects.Flame;

public class FlameBrainComponent : IComponent
{
    private readonly InputSystem inputSystem;
    private readonly PositionComponent flamePosition;
    public FlameBrainComponent(GameObject owner)
    {
        inputSystem = GameServicesCollection.Instance.Get<InputSystem>();
        flamePosition = owner.Components.Get<PositionComponent>();
    }
    public ValueTask Iterate(IGame game)
    {
        if (inputSystem.keyboardKeyStates[(int)KeyboardKey.Left])
            flamePosition.Move(-1, 0);

        if (inputSystem.keyboardKeyStates[(int)KeyboardKey.Right])
            flamePosition.Move(1, 0);

        return ValueTask.CompletedTask;
    }
}
