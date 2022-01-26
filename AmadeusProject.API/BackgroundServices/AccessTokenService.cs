namespace EducationalProject.BackgroundServices;

public class AccessTokenService : BackgroundService
{
    private readonly AccessTokenProviderService _tokenProvider;
    private readonly ILogger<AccessTokenService> _logger;
    private readonly IOptionsMonitor<ServiceAvailableOptions> _serviceAvailableOptions;
    private readonly IOptionsMonitor<AccessTokenOptions> _accessTokenOptions;

    public AccessTokenService(AccessTokenProviderService tokenProvider,
        ILogger<AccessTokenService> logger,
        IOptionsMonitor<ServiceAvailableOptions> serviceAvailableOptions,
        IOptionsMonitor<AccessTokenOptions> accessTokenOptions)
    {
        _tokenProvider = tokenProvider;
        _logger = logger;
        _serviceAvailableOptions = serviceAvailableOptions;
        _accessTokenOptions = accessTokenOptions;
    }

    public override Task StartAsync(CancellationToken cancellationToken)
    {
        if (!_serviceAvailableOptions.CurrentValue.ServiceAvailable)
        {
            return Task.CompletedTask;
        }

        return base.StartAsync(cancellationToken);
    }


    public override Task StopAsync(CancellationToken cancellationToken)
    {
        _logger.LogInformation("Hosted service shutting down");
        return base.StopAsync(cancellationToken);
    }

    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        int refreshTime;
        if (_serviceAvailableOptions.CurrentValue.ServiceAvailable)
        {
            while (!stoppingToken.IsCancellationRequested)
            {
                var token = await _tokenProvider.GetAccesToken(stoppingToken);

                if (!string.IsNullOrEmpty(token))
                {
                    _serviceAvailableOptions.CurrentValue.ServiceAvailable = true;
                    _accessTokenOptions.CurrentValue.AccessToken = token;
                    refreshTime = 1790000;
                }
                else
                {
                    _serviceAvailableOptions.CurrentValue.ServiceAvailable = false;
                    refreshTime = 10000;
                }

                await Task.Delay(refreshTime, stoppingToken);
            }
        }
    }
}

