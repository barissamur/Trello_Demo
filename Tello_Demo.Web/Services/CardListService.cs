using Newtonsoft.Json;
using Tello_Demo.Web.Models;

namespace Tello_Demo.Web.Services;

public class CardListService
{
    private readonly HttpClient _clientFactory;

    public CardListService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory.CreateClient("APIClient");
    }

    public async Task<IEnumerable<CardList>> GetCardListsAsync()
    {
        var response = await _clientFactory.GetAsync("api/CardList");
        response.EnsureSuccessStatusCode();

        try
        {
            var responseStream = await response.Content.ReadAsStreamAsync();
            var cardLists = JsonConvert.DeserializeObject<IEnumerable<CardList>>(await response.Content.ReadAsStringAsync());
            return cardLists ?? new List<CardList>();
        }
        catch (JsonException ex)
        {
            throw new InvalidOperationException("JSON serileştirme hatası", ex);
        }
    }

    public async Task<HttpResponseMessage> CreateCardListAsync(CardList cardList)
    { 
        var response = await _clientFactory.PostAsJsonAsync("api/CardList", cardList);
         
        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Hata yanıtı içeriği: {errorContent}");
        }
        return response;
    }
}
