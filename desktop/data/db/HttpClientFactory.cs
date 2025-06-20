using System.Net.Http;
using Microsoft.VisualBasic;

namespace desktop.data.db
{
    public class HttpClientFactory : IHttpClientFactory
    {
        private readonly string[] _options;

        public HttpClientFactory(string[] options)
        {
            _options = options;
        }

        public HttpClient CreateHttpClient(
            HttpMessageHandler? httpMessageHandler = null,
            bool? disposeHandler = null
        )
        {
            HttpClient httpClient;

            if (httpMessageHandler == null)
            {
                httpClient = new HttpClient();
            }
            else
            {
                if (disposeHandler != null)
                {
                    httpClient = new HttpClient(httpMessageHandler, (bool)disposeHandler);
                }
                else
                {
                    httpClient = new HttpClient(httpMessageHandler);
                }
            }

            if (_options.Length == 0)
            {
                return httpClient;
            }
            return httpClient;
        }
    }
}
