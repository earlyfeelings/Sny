namespace Sny.Web.Services.BackendProvider
{
    public class BackendProvider : IBackendProvider
    {

        private Uri _baseUri;

        public BackendProvider(Uri baseUri)
        {
            _baseUri = baseUri;
        }

        public Uri GetUri(string relativeUri)
        {
            return new Uri(_baseUri, relativeUri);
        }
    }
}
