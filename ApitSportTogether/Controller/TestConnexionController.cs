using Microsoft.AspNetCore.Mvc;
using System.Net.Http;

namespace ApiSportTogether.Controller
{
    [ApiController]
    [Route("ApiSportTogether/[controller]")]
    public class TestConnexionController
    {
        [HttpGet("Test")]
        public ActionResult<string> TestApiConnection()
        {
            return "L'api fonctionne";
        }
    }
}
