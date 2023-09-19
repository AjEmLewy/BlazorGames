using Gejms.Client.Games.Tetris;

namespace Gejms.Client.Shared.GameServices.Interfaces;

public interface IIterable
{
    ValueTask Iterate(IGame game);
}
