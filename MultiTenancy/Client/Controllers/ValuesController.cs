using Microsoft.AspNetCore.Mvc;
using System;

namespace Client.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public string Get() => $"From Client: {DateTime.Now}";
    }
}
