using Gejms.Client.Shared.GameServices.Interfaces;

namespace Gejms.Client.Games.Tetris.Services;

public class ScoreService : IGameService, IRestartable
{
    public int Score { get; private set; } = 0;
    public int Level { get; private set; } = 1;

    public void AddPoints(int amount)
    {
        int pointsToAdd = (int)(amount * Math.Pow(1.15, Level - 1));
        Score += pointsToAdd;
        if (Score >= 100 * Math.Pow(Level, 2))
            Level += 1;
    }

    public void Restart()
    {
        Score = 0;
        Level = 1;
    }
}
