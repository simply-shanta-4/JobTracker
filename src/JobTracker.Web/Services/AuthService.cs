using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using JobTracker.Web.Models;

namespace JobTracker.Web.Services;

public record AuthResult(bool Success, string? Error = null);

public class AuthService
{
    private readonly HttpClient _http;
    private readonly JwtAuthStateProvider _auth;

    public AuthService(HttpClient http, JwtAuthStateProvider auth)
    {
        _http = http;
        _auth = auth;
    }

    public Task<AuthResult> LoginAsync(LoginModel model) =>
        SendAsync("api/auth/login", new { model.Email, model.Password }, model.RememberMe);

    public Task<AuthResult> RegisterAsync(RegisterModel model) =>
        SendAsync("api/auth/register", new { model.Email, model.Password }, remember: true);

    public Task LogoutAsync() => _auth.ClearTokenAsync();

    private async Task<AuthResult> SendAsync(string url, object body, bool remember)
    {
        try
        {
            var response = await _http.PostAsJsonAsync(url, body);

            if (response.IsSuccessStatusCode)
            {
                var result = await response.Content.ReadFromJsonAsync<AuthResponse>();
                await _auth.SetTokenAsync(result!.Token, remember);
                return new AuthResult(true);
            }

            return new AuthResult(false, await ReadErrorAsync(response));
        }
        catch (HttpRequestException)
        {
            return new AuthResult(false, "Could not reach the server. Is the API running?");
        }
    }

    private static async Task<string> ReadErrorAsync(HttpResponseMessage response)
    {
        if (response.StatusCode == HttpStatusCode.Unauthorized)
            return "Invalid email or password.";

        // Register failures come back as a JSON list of messages
        try
        {
            var errors = await response.Content.ReadFromJsonAsync<List<string>>();
            if (errors is { Count: > 0 })
                return string.Join(" ", errors);
        }
        catch (JsonException) { }

        return "Something went wrong. Please try again.";
    }
}