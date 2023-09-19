using Gejms.Client.Assets.Retrivers;
using System.Collections.Concurrent;

namespace Gejms.Client.Assets;

public class AssetsCollection : IAssetsCollection
{
    private readonly ConcurrentDictionary<string, IAsset> assets = new ConcurrentDictionary<string, IAsset>();
    private readonly IAssetsRetriversFactory assetsRetrivers;

    public AssetsCollection(IAssetsRetriversFactory assetsRetrivers)
    {
        this.assetsRetrivers = assetsRetrivers;
    }
    public async ValueTask<T> Retrive<T>(string path) where T : IAsset
    {
        //get the correct retriver and retrive the asset
        var asset = await assetsRetrivers.GetRetriver<T>().Retrive(path);

        if (asset == null)
            throw new ArgumentException("nie udalo sie retriwowac assetu. ");

        assets.AddOrUpdate(asset.Name, k => asset, (k, v) => asset);
        return asset;
    }

    public T Get<T>(string name) where T: class, IAsset
    {
        if (!assets.ContainsKey(name))
            throw new ArgumentException("no nie ma takiego asseta. ");
        return assets[name] as T;
    }

    public T GetRandom<T>() where T : class, IAsset
    {
        var type = typeof(T);
        var eligible = assets.Values.Where(val => val.GetType() == type).ToList();

        int rnd = Random.Shared.Next(0, eligible.Count);
        return eligible[rnd] as T;
    }
}
