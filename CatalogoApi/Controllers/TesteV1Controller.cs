using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace CatalogoApi.Controllers
{
    [ApiVersion("1.0", Deprecated = true)] //Informa no header que a versão está depreciada
    [Route("v{v:apiVersion}/teste")]
    [ApiController]
    public class TesteV1Controller : ControllerBase
    {
        [HttpGet]
        public IActionResult Get()
        {
            return Content("<html><body><h2>Teste V1 Controller - V 1.0 </h2></body></html>", "text/html");
        }
    }
}
