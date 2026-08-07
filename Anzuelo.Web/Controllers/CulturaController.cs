using Microsoft.AspNetCore.Localization;
using Microsoft.AspNetCore.Mvc;

namespace Anzuelo.Web.Controllers
{
    public class CulturaController : Controller
    {
        [HttpPost]
        public IActionResult Cambiar(string culture, string returnUrl)
        {
            Response.Cookies.Append(
                CookieRequestCultureProvider.DefaultCookieName,
                CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(culture)),
                new CookieOptions
                {
                    Expires = DateTimeOffset.UtcNow.AddYears(1),
                    IsEssential = true, 
                    SameSite = SameSiteMode.Lax
                }
            );

            if (!Url.IsLocalUrl(returnUrl))
            {
                return RedirectToAction("Index", "Home");
            }

            return LocalRedirect(returnUrl);
        }
    }
}
