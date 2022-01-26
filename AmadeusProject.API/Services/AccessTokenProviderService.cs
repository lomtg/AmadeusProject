namespace EducationalProject.Services;
public class AccessTokenProviderService
{
    private readonly ILogger<AccessTokenProviderService> _logger;
    private readonly AmadeusAPIOptions _amadeusAPIOptions;
    private readonly HttpClient _httpClient;
    public AccessTokenProviderService(
        ILogger<AccessTokenProviderService> logger,
        IOptions<AmadeusAPIOptions> amadeusAPIOptions,
        HttpClient httpClient)
    {
        _logger = logger;
        _amadeusAPIOptions = amadeusAPIOptions.Value;
        _httpClient = httpClient;
    }

    public async Task<string?> GetAccesToken(CancellationToken token)
    {
        try
        {

            var res = await _httpClient.SendAsync(RequestMessage.AccessTokenRequestMessage(_amadeusAPIOptions.ApiKey, _amadeusAPIOptions.ApiSecret), token);

            res.EnsureSuccessStatusCode();

            var stream = await res.Content.ReadAsStreamAsync();

            var oauthResult = await JsonSerializer.DeserializeAsync<OAuthResult>(stream);

            _logger.LogInformation("Token received");

            return oauthResult?.access_token;
        }
        catch (Exception ex)
        {
            _logger.LogInformation(ex.Message);
            return "";
        }
    }
}
