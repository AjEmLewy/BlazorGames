using Microsoft.AspNetCore.Components.Authorization;
using System.Security.Claims;

namespace Gejms.Client.Shared.Utilities;

public class GejmsAuthenticationStateProvider : AuthenticationStateProvider
{
    private readonly HttpClient httpClient;
    private ClaimsPrincipal currentUser = new ClaimsPrincipal();

    public GejmsAuthenticationStateProvider(HttpClient httpClient)
    {
        this.httpClient = httpClient;
    }
    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        if (currentUser.Claims.Any())
        {
            return new AuthenticationState(currentUser);
        }
        var resp = await httpClient.GetAsync("/api/account");
        if (!resp.IsSuccessStatusCode)
            return new AuthenticationState(new ClaimsPrincipal());
        var username = await resp.Content.ReadAsStringAsync();

        ClaimsIdentity user = new ClaimsIdentity(nameof(GejmsAuthenticationStateProvider));
        user.AddClaim(new Claim(ClaimTypes.Name, username));

        currentUser = new ClaimsPrincipal(user);
        return new AuthenticationState(currentUser);
    }

    public void NotifyAuthChanged()
    {
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }
}
