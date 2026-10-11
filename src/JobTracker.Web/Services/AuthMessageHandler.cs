using System.Net;
using System.Net.Http.Headers;
using Microsoft.AspNetCore.Components;

namespace JobTracker.Web.Services;

public class AuthMessageHandler : DelegatingHandler
{
    private readonly JwtAuthStateProvider _auth;
    private readonly NavigationManager _nav;

    public AuthMessageHandler(JwtAuthStateProvider auth, NavigationManager nav)
    {
        _auth = auth;
        _nav = nav;
    }

    protected override async Task<HttpResponseMessage> SendAsync(
        HttpRequestMessage request, CancellationToken cancellationToken)
    {
        var token = await _auth.GetTokenAsync();
        if (!string.IsNullOrEmpty(token))
            request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", token);

        var response = await base.SendAsync(request, cancellationToken);

        // Token rejected or expired: log out and go to the login page
        if (response.StatusCode == HttpStatusCode.Unauthorized)
        {
            await _auth.ClearTokenAsync();
            _nav.NavigateTo("login");
        }

        return response;
    }
}