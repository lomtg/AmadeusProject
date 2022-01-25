namespace EducationalProject
{
    public class FlightService
    {
        private readonly IOptionsMonitor<AccessTokenOptions> _accessTokenOptions;

        public FlightService(IOptionsMonitor<AccessTokenOptions> accessTokenOptions)
        {
            _accessTokenOptions = accessTokenOptions;
        }

        public string Get()
        { 
            return _accessTokenOptions.CurrentValue.AccessToken;
        }

    }
}
