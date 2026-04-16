namespace brokenHand.Requests.Http
{
    public class HttpRequestNoSuccessException : Exception
    {
        public HttpResponseMessage? Response { get; set; }
    }
}
