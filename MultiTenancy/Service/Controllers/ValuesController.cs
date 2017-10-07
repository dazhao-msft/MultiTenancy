using Microsoft.AspNetCore.Mvc;
using System;

namespace Service.Controllers
{
    [Route("api/[controller]")]
    public class ValuesController
    {
        // GET api/values
        [HttpGet]
        public string Get() => $"From Service: {DateTime.Now}";
    }
}
