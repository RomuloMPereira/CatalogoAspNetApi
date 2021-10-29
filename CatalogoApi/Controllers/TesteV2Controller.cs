using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogoApi.Controllers
{
    [ApiVersion("2.0")]
    [Route("v{v:apiVersion}/teste")]
    [ApiController]
    public class TesteV2Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Content("<html><body><h2>Teste V2 Controller - V 2.0 </h2></body></html>", "text/html");
        }
    }
}
