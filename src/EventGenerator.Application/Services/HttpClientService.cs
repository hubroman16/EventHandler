using System.Text;
using EventGenerator.Domain.Interfaces;
using Serilog;

namespace EventGenerator.Application.Services;

public class HttpClientService : IHttpClientService
{
    private readonly HttpClient _httpClient;

    public HttpClientService(HttpClient httpClient)
    {
        _httpClient = httpClient;
    }
    
    public async Task<string> PostAsync(string uri, HttpContent content)
    {
        Log.Information("Sending POST request to {Uri}", uri);

        var response = await _httpClient.PostAsync(uri, content);
        response.EnsureSuccessStatusCode();

        var responseContent = await response.Content.ReadAsStringAsync();
        Log.Information("Received response: {ResponseContent}", responseContent);

        return responseContent;
    }
}