using Microsoft.AspNetCore.Components.WebAssembly.Http;
using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace AgendamentosWEB.Services
{
    public class CookieHandler : DelegatingHandler
    {
        protected override Task<HttpResponseMessage> SendAsync(HttpRequestMessage request, CancellationToken cancellationToken)
        {
            request.SetBrowserRequestCredentials(BrowserRequestCredentials.Include);
            return base.SendAsync(request, cancellationToken);
        }
    }
}
