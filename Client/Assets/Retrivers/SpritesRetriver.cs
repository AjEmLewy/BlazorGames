using Microsoft.AspNetCore.Components;
using System.Drawing;

namespace Gejms.Client.Assets.Retrivers;

public class SpritesRetriver : IAssetRetriver<Sprite>
{
    private readonly HttpClient httpClient;

    public SpritesRetriver(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    public async ValueTask<Sprite> Retrive(string path)
    {
        var bytes = await httpClient.GetByteArrayAsync(path);
        await using var stream = new MemoryStream(bytes);
        using var image = await SixLabors.ImageSharp.Image.LoadAsync(stream);
        var size = new Size(image.Width, image.Height);
        var elementRef = new ElementReference(Guid.NewGuid().ToString());

        var sprite = new Sprite(path) { ElementRef = elementRef, Name = path, Size = size };

        return sprite;
    }
}
