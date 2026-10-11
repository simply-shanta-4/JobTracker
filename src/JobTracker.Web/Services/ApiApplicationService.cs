using System.Net;
using System.Net.Http.Json;
using System.Text.Json;
using System.Text.Json.Serialization;
using JobTracker.Web.Models;

namespace JobTracker.Web.Services;

public class ApiApplicationService : IApplicationService
{
    private readonly HttpClient _http;

    // The API sends and expects enums as text ("Interview"), so match that here
    private static readonly JsonSerializerOptions JsonOptions =
        new(JsonSerializerDefaults.Web) { Converters = { new JsonStringEnumConverter() } };

    public ApiApplicationService(HttpClient http) => _http = http;

    public async Task<List<JobApplication>> GetAllAsync() =>
        await _http.GetFromJsonAsync<List<JobApplication>>("api/applications", JsonOptions)
        ?? new List<JobApplication>();

    public async Task<JobApplication?> GetByIdAsync(int id)
    {
        var response = await _http.GetAsync($"api/applications/{id}");
        if (response.StatusCode == HttpStatusCode.NotFound) return null;

        response.EnsureSuccessStatusCode();
        return await response.Content.ReadFromJsonAsync<JobApplication>(JsonOptions);
    }

    public async Task<JobApplication> AddAsync(JobApplication app)
    {
        var response = await _http.PostAsJsonAsync("api/applications", app, JsonOptions);
        response.EnsureSuccessStatusCode();
        return (await response.Content.ReadFromJsonAsync<JobApplication>(JsonOptions))!;
    }

    public async Task UpdateAsync(JobApplication app)
    {
        var response = await _http.PutAsJsonAsync($"api/applications/{app.Id}", app, JsonOptions);
        response.EnsureSuccessStatusCode();
    }

    public async Task DeleteAsync(int id)
    {
        var response = await _http.DeleteAsync($"api/applications/{id}");
        response.EnsureSuccessStatusCode();
    }
}