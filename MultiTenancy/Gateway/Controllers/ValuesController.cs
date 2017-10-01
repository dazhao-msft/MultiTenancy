using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Security.Claims;
using System.Threading.Tasks;

namespace Gateway.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        private readonly ILogger<ValuesController> _logger;

        public ValuesController(ILogger<ValuesController> logger)
        {
            _logger = logger;
        }

        // GET api/values
        [HttpGet]
        public Task Get()
        {
            Claim appId = User.FindFirst("appid");
            Claim tenantId = User.FindFirst("http://schemas.microsoft.com/identity/claims/tenantid");
            _logger.LogWarning("TraceId: {TraceId} | AppId: {AppId} | TenantId: {TenantId}", HttpContext.TraceIdentifier, appId.Value, tenantId.Value);

            // Service
            const string ServiceUrl = "http://localhost:44303/api/values";

            return HttpContext.ProxyRequest(new Uri(ServiceUrl));
        }
    }
}
