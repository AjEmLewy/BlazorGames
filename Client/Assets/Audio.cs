using Microsoft.AspNetCore.Components;

namespace Gejms.Client.Assets;

public class Audio
{
    public string Name { get; private set; }
    public string Path { get; private set; }
    public ElementReference ElementReference { get; set; }

    public Audio(string path)
    {
        if(string.IsNullOrEmpty(path))
            throw new ArgumentNullException("wrong audio path");
        Path = path;
        Name = Path.Split("/").Last();
        ElementReference = new ElementReference(Guid.NewGuid().ToString());
    }
}
