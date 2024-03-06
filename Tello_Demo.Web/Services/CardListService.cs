using Newtonsoft.Json;
using Tello_Demo.Web.DTOs;

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

        try
        {
            var responseStream = await response.Content.ReadAsStreamAsync();
            var cardLists = JsonConvert.DeserializeObject<IEnumerable<CardListDTO>>(await response.Content.ReadAsStringAsync());
            return cardLists ?? new List<CardListDTO>();
        }
        catch (JsonException ex)
        {
            // Hata detaylarını loglayın veya hata mesajını düzeltmek için işlem yapın.
            throw new InvalidOperationException("JSON serileştirme hatası", ex);
        }

    }
}
