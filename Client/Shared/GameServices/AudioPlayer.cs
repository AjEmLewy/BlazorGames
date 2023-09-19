using Gejms.Client.Shared.GameServices.Interfaces;
using Microsoft.JSInterop;

namespace Gejms.Client.Shared.GameServices;

public class AudioPlayer : IGameService
{
    private readonly IJSRuntime jsRuntime;

    public AudioPlayer(IJSRuntime JsRuntime)
    {
        jsRuntime = JsRuntime;
    }

    public async ValueTask PlaySound(string audioName)
    {
        if (string.IsNullOrEmpty(audioName))
            throw new ArgumentNullException(audioName);
        await jsRuntime.InvokeAsync<object>("playBackgroundMusic", audioName);
    }
}
