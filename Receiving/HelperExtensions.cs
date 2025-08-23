using Microsoft.AspNetCore.Antiforgery;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Template_Tabler
{
    public static class HelperExtensions
    {
        public static string GetCsrfToken(this HttpContext httpContext)
        {
            var antiforgery = httpContext.RequestServices.GetRequiredService<IAntiforgery>();
            var tokens = antiforgery.GetAndStoreTokens(httpContext);
            return tokens.RequestToken;
        }

        public static (string RequestToken, string HeaderName) GetCsrfTokens(this HttpContext httpContext)
        {
            var antiforgery = httpContext.RequestServices.GetRequiredService<IAntiforgery>();
            var tokens = antiforgery.GetAndStoreTokens(httpContext);
            return (tokens.RequestToken, tokens.HeaderName);
        }

        public static string GetBaseUrl(string fullUrl, string basePath = "gffoods")
        {
            try
            {
                Uri uri = new Uri(fullUrl);
                // ถ้า path เป็น "/" หรือว่าง (กรณี http://bp-webdev หรือ http://bp-webdev/)
                if (string.IsNullOrEmpty(uri.AbsolutePath) || uri.AbsolutePath == "/")
                {
                    // return เฉพาะ host เลย
                    return $"{uri.Scheme}://{uri.Authority}";
                }
                // ถ้า path มี basePath อยู่ด้านหน้าตามด้วย / 
                string[] segments = uri.AbsolutePath.Split('/', StringSplitOptions.RemoveEmptyEntries);
                if (segments.Length > 0 && segments[0].Equals(basePath, StringComparison.OrdinalIgnoreCase))
                {
                    return $"{uri.Scheme}://{uri.Authority}/{basePath}/home";
                }
                // default: return เฉพาะ host
                return $"{uri.Scheme}://{uri.Authority}";
            }
            catch //(UriFormatException)
            {
                return fullUrl;//
            }
        }

    }
}
