using Microsoft.Extensions.Options;
using gamitude_backend.Settings;
using gamitude_backend.Configuration;

namespace gamitude_backend.Dto.stats
{
    public class GetStatsDto
    {

        public long strength { get; set; }

        public long intelligence { get; set; }

        public long fluency { get; set; }

        public long creativity { get; set; }

    }
}
