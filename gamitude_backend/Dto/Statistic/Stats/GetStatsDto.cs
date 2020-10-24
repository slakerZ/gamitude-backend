using Microsoft.Extensions.Options;
using gamitude_backend.Settings;
using gamitude_backend.Configuration;

namespace gamitude_backend.Dto.stats
{
    public class GetStatsDto
    {

        public int strength { get; set; }

        public int intelligence { get; set; }

        public int fluency { get; set; }

        public int creativity { get; set; }

    }
}
