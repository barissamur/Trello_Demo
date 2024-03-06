using Tello_Demo.Web.Models;

namespace Tello_Demo.Web.Services;

public class CardService
{
    private readonly HttpClient _clientFactory;

    public CardService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory.CreateClient("APIClient");
    }

    public async Task<HttpResponseMessage> CreateCardAsync(Card card)
    {
        var response = await _clientFactory.PostAsJsonAsync("api/Card", card);

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Hata yanıtı içeriği: {errorContent}");
        }
        return response;
    }

    public async Task<HttpResponseMessage> DeleteCardAsync(int id)
    {
        var response = await _clientFactory.DeleteAsync($"api/Card/{id}");

        if (!response.IsSuccessStatusCode)
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            Console.WriteLine($"Hata yanıtı içeriği: {errorContent}");
        }
        return response;
    }
}
