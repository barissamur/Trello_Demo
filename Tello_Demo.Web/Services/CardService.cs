using Tello_Demo.Web.Models;

namespace Tello_Demo.Web.Services;

public class CardService
{
    private readonly HttpClient _clientFactory;
    private readonly ILogger<CardService> _logger;

    public CardService(IHttpClientFactory clientFactory
        , ILogger<CardService> logger)
    {
        _clientFactory = clientFactory.CreateClient("APIClient");
        _logger = logger;
    }

    public async Task<HttpResponseMessage> CreateCardAsync(Card card)
    {
        var response = await _clientFactory.PostAsJsonAsync("api/Card", card);

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Kart başarıyla oluşturuldu: {@Card}", card);
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Kart oluşturulurken bir hata oluştu - Hata: {ErrorContent}, Kart: {@Card}", errorContent, card);
        }
        return response;
    }


    public async Task<HttpResponseMessage> DeleteCardAsync(int id)
    {
        var response = await _clientFactory.DeleteAsync($"api/Card/{id}");

        if (response.IsSuccessStatusCode)
        {
            _logger.LogInformation("Kart başarıyla silindi - Kart ID: {CardId}", id);
        }
        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Kart silinirken bir hata oluştu - Kart ID: {CardId}, Hata: {ErrorContent}", id, errorContent);
        }
        return response;
    }

}
