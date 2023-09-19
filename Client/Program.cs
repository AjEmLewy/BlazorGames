using Gejms.Client;
using Gejms.Client.Assets;
using Gejms.Client.Assets.Retrivers;
using Gejms.Client.Shared;
using Gejms.Client.Shared.Utilities;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Microsoft.Extensions.DependencyInjection.Extensions;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddTransient(sp => new HttpClient { BaseAddress = new Uri(builder.HostEnvironment.BaseAddress) });

builder.Services.AddAuthorizationCore();
builder.Services.TryAddSingleton<AuthenticationStateProvider, GejmsAuthenticationStateProvider>();
//builder.Services.TryAddSingleton(sp => (GejmsAuthenticationStateProvider)sp.GetRequiredService<AuthenticationStateProvider>());

builder.Services.AddSingleton<UserService>();

builder.Services.AddSingleton<IAssetsCollection, AssetsCollection>();
builder.Services.AddSingleton<IAssetRetriver<Sprite>, SpritesRetriver >();
builder.Services.AddSingleton<IAssetRetriver<SpriteSheet>, SpriteSheetRetriver>();
builder.Services.AddSingleton<IAssetsRetriversFactory>( ctx =>
{
    var factory = new AssetsRetriversFactory();

    factory.Register(ctx.GetService<IAssetRetriver<Sprite>>());
    factory.Register(ctx.GetService<IAssetRetriver<SpriteSheet>>());

    return factory;
});

await builder.Build().RunAsync();