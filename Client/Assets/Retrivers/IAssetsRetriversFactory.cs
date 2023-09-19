namespace Gejms.Client.Assets.Retrivers;

public interface IAssetsRetriversFactory
{
    public void Register<T>(IAssetRetriver<T> assetCollection) where T : IAsset;

    public IAssetRetriver<T> GetRetriver<T>() where T : IAsset;
}
