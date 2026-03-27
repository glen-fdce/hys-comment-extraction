using System.Text.Json;
using GetComments.Data;
using GetComments.Entities;
using GetComments.Options;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Options;

namespace GetComments.Services;

public class BBCCommunicationService
{
    private readonly IDbContextFactory<ApplicationDbContext> _dbContextFactory;
    private readonly IHttpClientFactory _httpClientFactory;
    private readonly string _urlPrefix;
    private readonly string _apiKey;
    private readonly string _forumId;

    public BBCCommunicationService(IHttpClientFactory httpClientFactory,
        IOptions<HysOptions> hysOptions)
    {
        _httpClientFactory = httpClientFactory;
        _urlPrefix = hysOptions.Value.UrlPrefix;
        _apiKey = hysOptions.Value.ApiKey;
        _forumId = hysOptions.Value.ForumId;
    }

    public async Task<T?> HttpGetAsync<T>(string requestParams)
    {
        string url = $"{_urlPrefix}?apiKey={_apiKey}&forumId={_forumId}&isFirstDataRequested=true&requestParams={requestParams}";
        HttpClient client = _httpClientFactory.CreateClient("HYS");
        
        HttpRequestMessage request = new HttpRequestMessage(HttpMethod.Get, url);
        request.Headers.Add("accept", "application/json");
        request.Headers.Add("accept-language", "en-GB,en;q=0.5");
        request.Headers.Add("user-agent", "Mozilla/5.0 (Macintosh; Intel Mac OS X 10_15_7) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/145.0.0.0 Safari/537.36");
        
        HttpResponseMessage response = await client.SendAsync(request);
        string responseString = await response.Content.ReadAsStringAsync();

        if (!response.IsSuccessStatusCode)
        {
            throw new Exception(
                $"Request failed. Status: {(int)response.StatusCode} ({response.StatusCode}). Body: {responseString}");
        }
        
        return JsonSerializer.Deserialize<T>(responseString,
            new JsonSerializerOptions { PropertyNameCaseInsensitive = true });
    }
}
