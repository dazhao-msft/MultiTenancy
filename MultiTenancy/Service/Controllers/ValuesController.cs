using Microsoft.AspNetCore.Mvc;
using System;

namespace Service.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController : Controller
    {
        // GET api/values
        [HttpGet]
        public string Get() => DateTime.Now.ToString();
    }
}
