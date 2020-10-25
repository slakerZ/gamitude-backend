using System;
using Microsoft.AspNetCore.Mvc;

namespace gamitude_backend.Controllers
{
    [Route("api/[controller]")]
    [Route("/")]
    [ApiController]
    public class VersionController : ControllerBase
    {
        private String version = "Gamitude Backend v2.0";

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