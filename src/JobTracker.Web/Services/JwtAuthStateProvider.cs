using System.Security.Claims;
using System.Text;
using System.Text.Json;
using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.JSInterop;

namespace JobTracker.Web.Services;

public class JwtAuthStateProvider : AuthenticationStateProvider
{
    private const string TokenKey = "authToken";

    private static readonly AuthenticationState Anonymous =
        new(new ClaimsPrincipal(new ClaimsIdentity()));

    private readonly IJSRuntime _js;

    public JwtAuthStateProvider(IJSRuntime js) => _js = js;

    // "Remember me" ticked -> localStorage (survives closing the browser)
    // unticked -> sessionStorage (cleared when the tab closes)
    public async Task SetTokenAsync(string token, bool remember)
    {
        await ClearStorageAsync();
        var storage = remember ? "localStorage" : "sessionStorage";
        await _js.InvokeVoidAsync($"{storage}.setItem", TokenKey, token);
        NotifyAuthenticationStateChanged(GetAuthenticationStateAsync());
    }

    public async Task<string?> GetTokenAsync()
    {
        var token = await _js.InvokeAsync<string?>("localStorage.getItem", TokenKey);
        return token ?? await _js.InvokeAsync<string?>("sessionStorage.getItem", TokenKey);
    }

    public async Task ClearTokenAsync()
    {
        await ClearStorageAsync();
        NotifyAuthenticationStateChanged(Task.FromResult(Anonymous));
    }

    public override async Task<AuthenticationState> GetAuthenticationStateAsync()
    {
        var token = await GetTokenAsync();
        if (string.IsNullOrWhiteSpace(token)) return Anonymous;

        var claims = ParseClaims(token);
        if (claims is null)
        {
            await ClearStorageAsync(); // bad or expired token
            return Anonymous;
        }

        var identity = new ClaimsIdentity(claims, "jwt", nameType: "email", roleType: "role");
        return new AuthenticationState(new ClaimsPrincipal(identity));
    }

    private async Task ClearStorageAsync()
    {
        await _js.InvokeVoidAsync("localStorage.removeItem", TokenKey);
        await _js.InvokeVoidAsync("sessionStorage.removeItem", TokenKey);
    }

    private static List<Claim>? ParseClaims(string token)
    {
        try
        {
            var parts = token.Split('.');
            if (parts.Length != 3) return null;

            var payload = parts[1].Replace('-', '+').Replace('_', '/');
            payload = (payload.Length % 4) switch
            {
                2 => payload + "==",
                3 => payload + "=",
                _ => payload
            };

            var json = Encoding.UTF8.GetString(Convert.FromBase64String(payload));
            var values = JsonSerializer.Deserialize<Dictionary<string, JsonElement>>(json);
            if (values is null) return null;

            if (values.TryGetValue("exp", out var exp) &&
                DateTimeOffset.FromUnixTimeSeconds(exp.GetInt64()) <= DateTimeOffset.UtcNow)
                return null; // expired

            return values.Select(kv => new Claim(kv.Key, kv.Value.ToString())).ToList();
        }
        catch
        {
            return null;
        }
    }
}