using Gejms.Client.Games.Tetris;
using Gejms.Client.Games.Tetris.Models.Pieces;
using Gejms.Client.Games.Tetris.Services;
using Gejms.Client.Shared.GameServices;
using Gejms.Client.Shared.Models.GameObjectComponents;

namespace Gejms.Client.Games.Tetris.Models.Pieces.Components;

public class PieceBrain : IComponent
{
    private const int moveSideInterval = 120;
    private const int rotationInterval = 250;
    private const int defaultDownMovementInterval = 800;
    private const int minDownMovementInterval = 150;
    private const int spaceHitInterval = 500;
    private readonly PieceBase owner;
    private int downMovementInterval = defaultDownMovementInterval;

    private float lastMoveSideTime = -moveSideInterval / 2; //make it faster on spawn.
    private float lastMoveDownTime;
    private float lastRotationTime = -rotationInterval / 2;
    private float lastSpaceHitTime;

    private readonly GameTime gameTime;
    private readonly InputSystem inputSystem;
    private readonly AudioPlayer audioPlayer;
    private readonly PiecePositionComponent positionComponent;

    public PieceBrain(PieceBase owner)
    {
        this.owner = owner;

        gameTime = GameServicesCollection.Instance.Get<GameTime>();
        lastMoveSideTime = lastMoveDownTime = lastSpaceHitTime = lastRotationTime = gameTime.TotalMillis;
        inputSystem = GameServicesCollection.Instance.Get<InputSystem>();
        audioPlayer = GameServicesCollection.Instance.Get<AudioPlayer>();
        positionComponent = owner.components.Get<PiecePositionComponent>();

        var level = GameServicesCollection.Instance.Get<ScoreService>().Level;
        downMovementInterval = (int)Math.Max(downMovementInterval * Math.Pow(0.8, level - 1), minDownMovementInterval);
    }

    public async ValueTask Iterate(IGame game)
    {
        await HandlePieceRotation();
        await HandlePieceSideMovement();
        await HandlePieceDownMovement();
        await HandlePieceSpaceHit(game);
    }

    private async ValueTask HandlePieceSpaceHit(IGame game)
    {
        if (gameTime.TotalMillis - lastSpaceHitTime >= spaceHitInterval && inputSystem.keyboardKeyStates[(int)KeyboardKey.Space])
        {
            positionComponent.Land();
            lastSpaceHitTime = gameTime.TotalMillis;
            await audioPlayer.PlaySound("pew3.mp3");
        }

    }

    private async ValueTask HandlePieceSideMovement()
    {
        if (gameTime.TotalMillis - lastMoveSideTime >= moveSideInterval)
        {
            if (inputSystem.keyboardKeyStates[(int)KeyboardKey.Left])
            {
                await positionComponent.Move(-1, 0);
                lastMoveSideTime = gameTime.TotalMillis;
            }
            else if (inputSystem.keyboardKeyStates[(int)KeyboardKey.Right])
            {
                await positionComponent.Move(1, 0);
                lastMoveSideTime = gameTime.TotalMillis;
            }
        }
    }

    private async ValueTask HandlePieceDownMovement()
    {
        var downInterval = inputSystem.keyboardKeyStates[(int)KeyboardKey.Down] ? 100 : downMovementInterval;
        if (gameTime.TotalMillis - lastMoveDownTime > downInterval)
        {
            await positionComponent.Move(0, 1);
            lastMoveDownTime = gameTime.TotalMillis;
        }
    }

    private async ValueTask HandlePieceRotation()
    {
        if (gameTime.TotalMillis - lastRotationTime >= rotationInterval && inputSystem.keyboardKeyStates[(int)KeyboardKey.Up])
        {
            owner.Rotate();
            lastRotationTime = gameTime.TotalMillis;
        }
    }
}