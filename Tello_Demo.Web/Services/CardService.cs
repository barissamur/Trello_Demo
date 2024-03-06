namespace Tello_Demo.Web.Services;

public class CardService
{
    private readonly HttpClient _clientFactory;

    public CardService(IHttpClientFactory clientFactory)
    {
        _clientFactory = clientFactory.CreateClient("APIClient");
    }


}
