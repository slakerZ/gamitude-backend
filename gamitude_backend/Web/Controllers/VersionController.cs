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

        /// <summary>
        /// Health check endpoint with application version.
        /// </summary>
        [HttpGet]
        public ActionResult<string> Version()
        {
            return Created("Version", version);
        }

    }
}