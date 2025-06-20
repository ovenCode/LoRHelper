using System.Net.Http;

namespace desktop.data.db
{
    public interface IHttpClientFactory
    {
        public HttpClient CreateHttpClient(
            HttpMessageHandler? httpMessageHandler,
            bool? disposeHandler
        );
    }
}
