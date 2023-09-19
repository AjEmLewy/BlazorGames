namespace Gejms.Client.Games.Tetris;

public interface IGame
{
    ValueTask Iterate();
    ValueTask InitGame();
    ValueTask EndGame();
}
