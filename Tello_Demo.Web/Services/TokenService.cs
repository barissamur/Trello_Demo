namespace Tello_Demo.Web.Services;

public class TokenService
{
    private readonly HttpClient _httpClient;
    private readonly string _apiBaseUrl;
    private readonly string _tokenEndpoint;

    public TokenService(HttpClient httpClient, IConfiguration config)
    {
        _httpClient = httpClient;
        _apiBaseUrl = config.GetValue<string>("ApiSettings:BaseUrl");
        _tokenEndpoint = $"{_apiBaseUrl}token";
    }


    public async Task<string> GetTokenAsync()
    {
        var response = await _httpClient.PostAsJsonAsync(_tokenEndpoint, new { Username = "statikKullaniciAdi" });
        response.EnsureSuccessStatusCode();

        var tokenResponse = await response.Content.ReadFromJsonAsync<TokenResponse>();
        return tokenResponse.Token;
    }
}

public class TokenResponse
{
    public string Token { get; set; }
}
