namespace Gejms.Client.Assets.Retrivers;

public interface IAssetRetriver<T> where T : IAsset
{
    public ValueTask<T> Retrive(string path);
}
