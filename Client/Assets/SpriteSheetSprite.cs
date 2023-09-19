using System.Drawing;

namespace Gejms.Client.Assets;

public class SpriteSheetSprite : Sprite
{
    public Point SpriteSheetPos { set; get; }
    public SpriteSheetSprite(string imagePath) : base(imagePath)
    {
    }
}
