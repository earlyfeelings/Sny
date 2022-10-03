namespace Sny.Web.Services.LocalStorageService
{
    /// <summary>
    /// Poskytuje rozhraní pro přístup do Local storage webového prohlížeče.
    /// </summary>
    public interface ILocalStorageService
    {
        Task<T?> GetItem<T>(string key);
        Task SetItem<T>(string key, T value);
        Task RemoveItem(string key);
    }
}
