using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
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
        private readonly AzureAdOptions _azureAdOptions;

        public ValuesController(IConfiguration configuration)
        {
            _azureAdOptions = new AzureAdOptions();
            configuration.Bind("AzureAd", _azureAdOptions);
        }

        // GET api/values
        [HttpGet]
        public async Task<string> Get()
        {
            const string GatewayUrl = "https://localhost:44302/api/values";
            const string GatewayResourceUri = "https://microsoft.onmicrosoft.com/a68d25f1-1e2b-4d4f-b503-ca1ae5525277";

            var authenticationContext = new AuthenticationContext($"{ _azureAdOptions.Instance }{ _azureAdOptions.TenantId}");
            var clientCredential = new ClientCredential(_azureAdOptions.AppId, _azureAdOptions.AppSecret);

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

        public class AzureAdOptions
        {
            public string AppId { get; set; }
            public string AppSecret { get; set; }
            public string Instance { get; set; }
            public string Domain { get; set; }
            public string TenantId { get; set; }
        }
    }
}
