using MongoDB.Entities;
using SearchService.Models;

namespace SearchService.Services;

public class AuctionServiceHttpClient
{
    private readonly HttpClient _httpClient;
    private readonly IConfiguration _configuration;

    public AuctionServiceHttpClient(HttpClient httpClient, IConfiguration configuration)
    {
        _httpClient = httpClient;
        _configuration = configuration;
    }

    public async Task<List<Item>> GetItemsForSearchDb()
    {
        var lastUpdated = await DB.Find<Item, string>()
            .Sort(x => x.Descending(y => y.ModifiedDate))
            .Project(x => x.ModifiedDate.ToString())
            .ExecuteFirstAsync();

        var url = _configuration["AuctionServiceUrl"] + "/api/auctions?date=" + lastUpdated;
        return await _httpClient.GetFromJsonAsync<List<Item>>(url) ?? new List<Item>();
    }
}