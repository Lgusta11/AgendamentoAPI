using Microsoft.AspNetCore.Http;

namespace SistemaAfsWeb.Response
{
    internal class Cookies
    {
        private readonly IHttpContextAccessor _httpContextAccessor;

        public Cookies(IHttpContextAccessor httpContextAccessor)
        {
            _httpContextAccessor = httpContextAccessor;
        }

        internal void Append(string cookieName, string cookieValue, CookieOptions options)
        {
            var httpContext = _httpContextAccessor.HttpContext;
            if (httpContext?.Response != null)
            {
                httpContext.Response.Cookies.Append(cookieName, cookieValue, options);
            }
            else
            {
                throw new InvalidOperationException("HttpContext or Response is null.");
            }
        }
    }
}
