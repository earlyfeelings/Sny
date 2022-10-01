namespace Sny.Web.Services.BackendProvider
{
    public class BackendProvider : IBackendProvider
    {

        private Uri _baseUri = new Uri("https://localhost:7026");
        public Uri GetUri(string relativeUri)
        {
            return new Uri(_baseUri, relativeUri);
        }
    }
}
