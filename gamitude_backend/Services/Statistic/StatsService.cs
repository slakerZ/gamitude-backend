using gamitude_backend.Models;
 
using System.Collections.Generic;
using System.Linq;
using System;
using System.Threading.Tasks;
using Microsoft.Extensions.Logging;
using gamitude_backend.Dto.stats;
 
using gamitude_backend.Settings;
using AutoMapper;
using MongoDB.Driver;
using MongoDB.Driver.Linq;
using gamitude_backend.Data;
using gamitude_backend.Repositories;

namespace gamitude_backend.Services
{

    public interface IStatsService : IStatsRepository
    {
    }

    public class StatsService : StatsRepository,IStatsService
    {
        private readonly IMongoCollection<Stats> _stats;
        private readonly IMapper _mapper;
        private readonly ILogger<StatsService> _logger;

        public StatsService(IMapper mapper, IDatabaseCollections dbCollections, ILogger<StatsService> logger) : base(dbCollections)
        {
            _stats = dbCollections.stats;
        }


    }
}