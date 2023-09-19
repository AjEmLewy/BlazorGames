using Microsoft.AspNetCore.Components;
using System.Drawing;

namespace Gejms.Client.Assets;

public class SpriteSheet : IAsset
{
    public string Name { get; set; } = "";
    public string Path { get; set; } = "";
    public int FPS { get; set; }
    public Size SpriteSheetSize { get; set; }
    public Size FrameSize { get; set; }
    public int FramesCount { get; set; }

    public ElementReference ElementRef { get; set; }

    public List<SpriteSheetSprite> Sprites { get; private set; } = new();

    public SpriteSheet(string imagePath, int fps, Size frameSize, Size spritesheetSize, int framesCount, ElementReference elementRef)
    {
        if (string.IsNullOrEmpty(imagePath))
            throw new ArgumentNullException(nameof(imagePath));

        Path = imagePath;
        FPS = fps;
        FramesCount = framesCount;
        ElementRef = elementRef;
        FrameSize = frameSize;
        SpriteSheetSize = spritesheetSize;

        Load(imagePath);
    }

    private void Load(string path)
    {
        Console.WriteLine(FrameSize.Width + ", " + FrameSize.Height);
        int cols = SpriteSheetSize.Width / FrameSize.Width;
        int rows = SpriteSheetSize.Height / FrameSize.Height;

        int currFrameIndex = 0;
        while (currFrameIndex != FramesCount)
        {
            var sprite = new SpriteSheetSprite(path)
            {
                Size = FrameSize,
                ElementRef = ElementRef,
                Name = path,
                SpriteSheetPos = new Point(currFrameIndex % cols * FrameSize.Width, (int)currFrameIndex / rows * FrameSize.Height)
            };
            Sprites.Add(sprite);
            currFrameIndex++;
        }
    }
}
