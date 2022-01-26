namespace EducationalProject.Helpers
{
    public static class RequestMessage
    {
        public static HttpRequestMessage AccessTokenRequestMessage(string apiKey,string apiSecret)
        {
            var requestMessage = new HttpRequestMessage(HttpMethod.Post, "/v1/security/oauth2/token")
            {
                Content = new StringContent(
                $"grant_type=client_credentials&client_id={apiKey}&client_secret={apiSecret}",
                Encoding.UTF8, "application/x-www-form-urlencoded"
                )
            };
            //hh
            return requestMessage;
        }

    }
}
