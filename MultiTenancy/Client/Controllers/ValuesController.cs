using AadConfiguration;
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
        // GET api/values
        [HttpGet]
        public async Task<string> Get()
        {
            var aadConfiguration = new ConfigurationBuilder().AddJsonFile(AadOptionsDefaults.DefaultFile).Build();

            // Gateway
            const string GatewayUrl = "https://localhost:44302/api/values";

            var gatewayOptions = aadConfiguration.GetSection("Gateway").Get<AadOptions>();

            var clientOptions = aadConfiguration.GetSection("Client").Get<AadOptions>();

            var authenticationContext = new AuthenticationContext($"{clientOptions.Instance}{clientOptions.TenantId}");
            var clientCredential = new ClientCredential(clientOptions.AppId, clientOptions.AppSecret);

            var result = await authenticationContext.AcquireTokenAsync(gatewayOptions.Audience, clientCredential);

            var clientHandler = new HttpClientHandler()
            {
                AllowAutoRedirect = false,
                UseCookies = false,
                ServerCertificateCustomValidationCallback = HttpClientHandler.DangerousAcceptAnyServerCertificateValidator
            };

            using (var client = new HttpClient(clientHandler, true))
            {
                var request = new HttpRequestMessage
                {
                    RequestUri = new Uri(GatewayUrl),
                    Method = HttpMethod.Get
                };
                request.Headers.Authorization = new AuthenticationHeaderValue("Bearer", result.AccessToken);

                var response = await client.SendAsync(request);

                return await response.Content.ReadAsStringAsync();
            }
        }
    }
}
