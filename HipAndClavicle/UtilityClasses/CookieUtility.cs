using Microsoft.Extensions.Configuration;

namespace HipAndClavicle.UtilityClasses
{
    public class CookieUtility
    {
        private readonly IConfiguration cookieConfig;

        public CookieUtility(IConfiguration configuration)
        {
            cookieConfig = configuration;
        }

        public string GetShopingCartCookieName()
        {
            string cookiePrefix = cookieConfig.GetSection("CookieSettings")["CookiePrefix"];
            return cookiePrefix + "CartId";
        }
    }
}
