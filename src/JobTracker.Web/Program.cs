using Microsoft.AspNetCore.Components.Authorization;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using JobTracker.Web;
using JobTracker.Web.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

var apiUrl = builder.Configuration["ApiUrl"]
    ?? throw new InvalidOperationException("ApiUrl is not configured.");

// Authentication state
builder.Services.AddAuthorizationCore();
builder.Services.AddCascadingAuthenticationState();
builder.Services.AddSingleton<JwtAuthStateProvider>();
builder.Services.AddSingleton<AuthenticationStateProvider>(sp =>
    sp.GetRequiredService<JwtAuthStateProvider>());

// Login and register calls (plain client: no token needed yet)
builder.Services.AddSingleton<AuthService>(sp =>
    new AuthService(
        new HttpClient { BaseAddress = new Uri(apiUrl) },
        sp.GetRequiredService<JwtAuthStateProvider>()));

// Applications calls (the handler adds the token to every request)
builder.Services.AddTransient<AuthMessageHandler>();
builder.Services.AddHttpClient<IApplicationService, ApiApplicationService>(client =>
        client.BaseAddress = new Uri(apiUrl))
    .AddHttpMessageHandler<AuthMessageHandler>();

await builder.Build().RunAsync();