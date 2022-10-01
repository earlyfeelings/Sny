namespace Sny.Web.Services.BackendProvider
{
    public class BackendProvider : IBackendProvider
    {

        private Uri _baseUri;

        public BackendProvider(Uri baseUri, bool doNotCheck)
        {
            _baseUri = baseUri;
            DoNotCheck = doNotCheck;
        }

        public bool DoNotCheck { get; set; }

        public Uri GetUri(string relativeUri)
        {
            return new Uri(_baseUri, relativeUri);
        }
    }
}
