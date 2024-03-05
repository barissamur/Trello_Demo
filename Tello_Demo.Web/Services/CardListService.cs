using System.Text.Json;
using Tello_Demo.Application.DTOs;

namespace Tello_Demo.Web.Services;

public class CardListService
{
    private readonly HttpClient _clientFactory;

    public CardListService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory.CreateClient("APIClient");
    }

    public async Task<IEnumerable<CardListDTO>> GetCardListsAsync()
    {
        var response = await _clientFactory.GetAsync("api/CardList");
        response.EnsureSuccessStatusCode();

        var responseStream = await response.Content.ReadAsStreamAsync();
        var cardLists = await JsonSerializer.DeserializeAsync<IEnumerable<CardListDTO>>(responseStream);

        return cardLists ?? new List<CardListDTO>();
    }
}
