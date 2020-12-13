using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;

namespace gamitude_backend.Controllers
{
    [Route("api/[controller]")]
    [Route("/")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        private string version = "Gamitude Backend v2.0";

        public VersionController()
        {

        }

        [HttpGet]
        public async Task<ActionResult<string>> Version()
        {
            await new Example().Execute();

            return Created("Version", version);
        }

    }
}