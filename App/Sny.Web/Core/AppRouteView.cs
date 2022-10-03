using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Components.Rendering;
using Microsoft.AspNetCore.Components;
using System.Net;
using Sny.Web.Services.UserContext;

namespace Sny.Web.Core
{
    /// <summary>
    /// Komponenta představuje Router - stará se o navigaci mezi jednotlivými stránkami.
    /// </summary>
    public class AppRouteView : RouteView
    {
        [Inject]
        public NavigationManager NavigationManager { get; set; } = default!;

        [Inject]
        public IUserContext UserContext { get; set; } = default!;

        protected override void Render(RenderTreeBuilder builder)
        {
            //Provede kontrolu autentizace. Pokud není uživatel přihlášen a je to pro přístup na danou stránku vyžadováno,
            //pak je uživatel přesměrován na /login. Jako parametr je předána relativní URL adresa stránky, kam chtěl uživatel přistoupit.
            //Toho lze využít po příhlášení, aby byl uživatel přesměrován na stránku, na kterou se pokoušel přistoupit.
            var authorize = Attribute.GetCustomAttribute(RouteData.PageType, typeof(AuthorizeAttribute)) != null;
            if (authorize && !UserContext.IsLoggedIn)
            {
                var returnUrl = WebUtility.UrlEncode(new Uri(NavigationManager.Uri).PathAndQuery);
                NavigationManager.NavigateTo($"/login?returnUrl={returnUrl}");
            }
            else
            {
                base.Render(builder);
            }
        }
    }
}
