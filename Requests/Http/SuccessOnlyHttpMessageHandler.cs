namespace brokenHand.Requests.Http
{
    public class SuccessOnlyHttpMessageHandler : DelegatingHandler
    {
        protected override async Task<HttpResponseMessage> SendAsync(
            HttpRequestMessage request,
            CancellationToken cancellationToken
        )
        {
            HttpResponseMessage response = await base.SendAsync(request, cancellationToken);

            if (!response.IsSuccessStatusCode)
            {
                throw new HttpRequestNoSuccessException { Response = response };
            }

            return response;
        }
    }
}
