using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Threading.Tasks;

namespace Gateway.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public Task Get()
        {
            // Service
            const string ServiceUrl = "http://localhost:44303/api/values";

            return HttpContext.ProxyRequest(new Uri(ServiceUrl));
        }
    }
}
