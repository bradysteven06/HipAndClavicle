using Azure;
using Microsoft.Extensions.Configuration;

namespace HipAndClavicle.UtilityClasses
{
    public static class CookieUtility
    {
        private static IHttpContextAccessor _contextAccessor;
        private static string cookieNamePrefix;
        private static string shoppingCartCookieName;

        public static void Initialize(IHttpContextAccessor contextAccessor, IConfiguration configuration)
        {
            _contextAccessor = contextAccessor;
            cookieNamePrefix = configuration["CookieSettings:CookiePrefix"];
            shoppingCartCookieName = configuration["CookieSettings:CookiePrefix"] + "CartId";
        }

        public static string GetShopingCartCookieName()
        {
            return cookieNamePrefix + "CartId";
        }

        // Gets cookie with cartId in it
        public static string GetCookie()
        {
            return _contextAccessor.HttpContext.Request.Cookies[shoppingCartCookieName];
        }

        // Sets cartId to cookie
        public static void SetCookie(string cookieValue)
        {
            _contextAccessor.HttpContext.Response.Cookies.Append(shoppingCartCookieName, cookieValue, new CookieOptions());
        }

        // Deletes all cookies with specified cookie name prefix
        public static void DeleteCookie()
        {
            /*var matchingCookies = context.Request.Cookies
                .Where(cookie => cookie.Key.StartsWith(cookieNamePrefix))
                .Select(cookie => cookie.Key);*/

            /*foreach (var cookieKey in matchingCookies)
            {
                _contextAccessor.HttpContext.Response.Cookies.Delete(cookieKey);
            }*/

            var matchingCookies = new List<string>();

            foreach (var cookieKey in _contextAccessor.HttpContext.Request.Cookies.Keys)
            {
                if (cookieKey.StartsWith(cookieNamePrefix, StringComparison.Ordinal))
                {
                    matchingCookies.Add(cookieKey);
                }
            }

            foreach (var matchingCookie in matchingCookies)
            {
                _contextAccessor.HttpContext.Response.Cookies.Delete(matchingCookie);
            }
        }

        // Deletes shopping cart cookie
        public static void DeleteShoppingCartCookie()
        {
            _contextAccessor.HttpContext.Response.Cookies.Delete(shoppingCartCookieName);
        }

        // unsure on what this method would do
        public static void RefreshShoppingCartCookie()
        {

        }

    }
}
