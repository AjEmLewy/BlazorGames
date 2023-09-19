using Microsoft.AspNetCore.Components;
using System.Drawing;
using System.Net.Http.Json;
using System.Text.Json;

namespace Gejms.Client.Assets.Retrivers;

public class SpriteSheetRetriver : IAssetRetriver<SpriteSheet>
{
    private readonly HttpClient httpClient;

    public SpriteSheetRetriver(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    public async ValueTask<SpriteSheet> Retrive(string path)
    {
        SpriteSheetMeta spritesheetMeta = await httpClient.GetFromJsonAsync<SpriteSheetMeta>(path, new JsonSerializerOptions { PropertyNameCaseInsensitive = true });

        var bytes = await httpClient.GetByteArrayAsync(spritesheetMeta.path);
        await using var stream = new MemoryStream(bytes);
        using var image = await SixLabors.ImageSharp.Image.LoadAsync(stream);
        var size = new Size(image.Width, image.Height);
        var elementRef = new ElementReference(Guid.NewGuid().ToString());

        Console.WriteLine(JsonSerializer.Serialize(spritesheetMeta));
        var frameSize = new Size(spritesheetMeta.frameSize.Width, spritesheetMeta.frameSize.Height);
        var spritesheetSize = new Size(spritesheetMeta.size.Width, spritesheetMeta.size.Height);

        var spritesheet = new SpriteSheet(spritesheetMeta.path, spritesheetMeta.fps, frameSize, spritesheetSize, spritesheetMeta.framesCount, elementRef)
        {
            Name = spritesheetMeta.name,
        };
        return spritesheet;
    }
    private class SpriteSheetMeta
    {
        public string name { get; set; }
        public string path { get; set; }
        public int fps { get; set; }
        public int framesCount { get; set; }
        public MySize size { get; set; }
        public MySize frameSize { get; set; }
    }
    private class MySize
    {
        public int Width { get; set; }
        public int Height { get; set; }
    }
}
