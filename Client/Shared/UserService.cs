using Gejms.Client.Shared.Utilities;
using Gejms.Shared.Models;
using Microsoft.AspNetCore.Components.Authorization;
using System.Text;
using System.Text.Json;

namespace Gejms.Client.Shared;

public class UserService
{
    private readonly HttpClient httpClient;
    private readonly GejmsAuthenticationStateProvider authenticationState;

    public UserService(HttpClient httpClient, AuthenticationStateProvider authenticationState)
    {
        this.httpClient = httpClient;
        this.authenticationState = (GejmsAuthenticationStateProvider)authenticationState;
    }

    public async ValueTask Login(UserDTO userDto)
    {
        var jsonContent = JsonSerializer.Serialize(userDto);
        var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync("/api/Account/Login", stringContent);
        response.EnsureSuccessStatusCode();
        authenticationState.NotifyAuthChanged();
    }
    
    public async ValueTask Register(UserDTO userDto)
    {
        var jsonContent = JsonSerializer.Serialize(userDto);
        var stringContent = new StringContent(jsonContent, Encoding.UTF8, "application/json");
        var response = await httpClient.PostAsync("/api/Account/Register", stringContent);
        response.EnsureSuccessStatusCode();
        authenticationState.NotifyAuthChanged();
    }
}
