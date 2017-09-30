using AadConfiguration;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Clients.ActiveDirectory;
using System;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Threading.Tasks;

namespace Client.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public async Task<string> Get()
        {
            // CciWebApi
            const string GatewayUrl = "https://localhost:44302/api/values";
            const string GatewayResourceUri = "https://microsoft.onmicrosoft.com/66e86ee1-26e2-4ed6-a37f-be28838e3765";

            var aadOptions = new AadOptions();
            new AadOptionsBuilder().Bind("CciWebApp", aadOptions);

            var authenticationContext = new AuthenticationContext($"{ aadOptions.Instance }{ aadOptions.TenantId}");
            var clientCredential = new ClientCredential(aadOptions.AppId, aadOptions.AppSecret);

            var result = await authenticationContext.AcquireTokenAsync(GatewayResourceUri, clientCredential);

            var clientHandler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                UseCookies = false,
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            using (var client = new HttpClient(clientHandler, true))
            {
                var request = new HttpRequestMessage();
                request.RequestUri = new Uri(GatewayUrl);
                request.Method = HttpMethod.Get;
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);

                var response = await client.SendAsync(request);

                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}
