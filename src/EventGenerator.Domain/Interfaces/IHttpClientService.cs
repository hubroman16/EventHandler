namespace EventGenerator.Domain.Interfaces;

public interface IHttpClientService
{
    Task<string> PostAsync(string uri, HttpContent content);
}