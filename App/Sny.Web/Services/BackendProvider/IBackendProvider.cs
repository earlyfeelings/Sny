﻿namespace Sny.Web.Services.BackendProvider
{
    public interface IBackendProvider
    {
        Uri GetUri(string relativeUri);
    }
}