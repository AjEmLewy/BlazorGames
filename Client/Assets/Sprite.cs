using Microsoft.AspNetCore.Components;
using System.Drawing;

namespace Gejms.Client.Assets;

public class Sprite : IAsset
{
    public string Name { get; set; }
    public ElementReference ElementRef { get; set; }
    public Size Size { get; set; }

    public Sprite(string imagePath)
    {
        if (string.IsNullOrEmpty(imagePath))
            throw new ArgumentNullException(nameof(imagePath));
    }
}
