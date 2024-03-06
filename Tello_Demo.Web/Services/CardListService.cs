using Newtonsoft.Json;
using System.Net;
using System.Net.Http;
using Tello_Demo.Web.Models;

namespace Tello_Demo.Web.Services;

public class CardListService
{
    private readonly HttpClient _clientFactory;
    private readonly ILogger<CardListService> _logger;
    private readonly TokenService _tokenService;

    public CardListService(IHttpClientFactory clientFactory
        , ILogger<CardListService> logger
        , TokenService tokenService)
    {
        _clientFactory = clientFactory.CreateClient("APIClient");
        _logger = logger;
        _tokenService = tokenService;
    }

    public async Task<IEnumerable<CardList>> GetCardListsAsync()
    {
        try
        {
            var token = await _tokenService.GetTokenAsync();
            _clientFactory.DefaultRequestHeaders.Authorization = new System.Net.Http.Headers.AuthenticationHeaderValue("Bearer", token);

            var response = await _clientFactory.GetAsync("api/CardList");
            response.EnsureSuccessStatusCode();

            var responseStream = await response.Content.ReadAsStreamAsync();
            var cardLists = JsonConvert.DeserializeObject<IEnumerable<CardList>>(await response.Content.ReadAsStringAsync());
            return cardLists ?? new List<CardList>();
        }
        catch (HttpRequestException httpEx)
        {
            _logger.LogError("HTTP istek hatası: {Message}", httpEx.Message);
            throw;
        }
        catch (JsonException jsonEx)
        {
            _logger.LogError("JSON serileştirme hatası: {Message}", jsonEx.Message);
            throw new InvalidOperationException("JSON serileştirme hatası", jsonEx);
        }
    }

    public async Task<HttpResponseMessage> CreateCardListAsync(CardList cardList)
    {
        var response = await _clientFactory.PostAsJsonAsync("api/CardList", cardList);

        if (response.IsSuccessStatusCode)
            _logger.LogInformation("Liste başarıyla oluşturuldu: {@CardList}", cardList);

        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Liste oluşturulurken hata oluştu: {ErrorContent}", errorContent);
        }

        return response;
    }

    public async Task<HttpResponseMessage> UpdateCardListAsync(List<CardList> cardList)
    {
        HttpResponseMessage lastResponse = new();

        foreach (var item in cardList)
        {
            var response = await _clientFactory.PutAsJsonAsync($"api/CardList/{item.Id}", item);

            if (response.IsSuccessStatusCode)
                _logger.LogInformation("Liste güncellendi - Liste ID: {ListId}, Liste Başlığı: {Title}", item.Id, item.Title);

            else
            {
                var errorContent = await response.Content.ReadAsStringAsync();
                _logger.LogError("Liste güncellenirken bir hata oluştu - Liste ID: {ListId}, Hata: {ErrorContent}", item.Id, errorContent);
            }

            lastResponse = response;
        }

        return lastResponse;
    }


    public async Task<HttpResponseMessage> DeleteCardListAsync(int id)
    {
        var response = await _clientFactory.DeleteAsync($"api/CardList/{id}");

        if (response.IsSuccessStatusCode)
            _logger.LogInformation("Liste silindi - Liste ID: {ListId}", id);

        else
        {
            var errorContent = await response.Content.ReadAsStringAsync();
            _logger.LogError("Liste silinirken bir hata oluştu - Liste ID: {ListId}, Hata: {ErrorContent}", id, errorContent);
        }
        return response;
    }

}
