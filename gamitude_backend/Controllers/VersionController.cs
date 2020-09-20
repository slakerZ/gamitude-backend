using System;
using Microsoft.AspNetCore.Mvc;

namespace gamitude_backend.Controllers
{
    [Route("api/[controller]")]
    [Route("/")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        private String version = "Gamitude Backend Alpha v1.1";

        public VersionController()
        {

        }

        [HttpGet]
        public ActionResult<String> Version()
        {

            return Created("Version", version);
        }

    }
}