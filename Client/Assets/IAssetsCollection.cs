namespace Gejms.Client.Assets;

public interface IAssetsCollection
{
    public ValueTask<T> Retrive<T>(string path) where T : IAsset;
    public T Get<T>(string name) where T: class, IAsset;
    public T GetRandom<T>() where T : class, IAsset;
}
