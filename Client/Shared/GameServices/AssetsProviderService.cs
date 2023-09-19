using Gejms.Client.Assets;
using Gejms.Client.Shared.GameServices.Interfaces;

namespace Gejms.Client.Shared.GameServices;

public class AssetsProviderService : IGameService
{
    public IAssetsCollection AssetsCollection { get; private set; }

    public AssetsProviderService(IAssetsCollection assetsCollection)
    {
        AssetsCollection = assetsCollection;
    }

}
