namespace Gejms.Client.Assets.Retrivers;

public class AssetsRetriversFactory : IAssetsRetriversFactory
{
    private readonly Dictionary<Type, object> assetsRetrivers = new();

    public void Register<T>(IAssetRetriver<T> assetsRetriver) where T : IAsset
    {
        var type = typeof(T);
        if (!assetsRetrivers.ContainsKey(type))
            assetsRetrivers[type] = null;
        assetsRetrivers[type] = assetsRetriver;
    }

    public IAssetRetriver<T> GetRetriver<T>() where T : IAsset
    {
        var type = typeof(T);
        if (!assetsRetrivers.ContainsKey(type))
            throw new ArgumentNullException("nie ma takiego collectionu zarejestrowanego: " + type);
        return assetsRetrivers[type] as IAssetRetriver<T>;
    }
}
