using gamitude_backend.Models;
using System.Collections.Generic;
using System.Linq;
using System;
using Microsoft.Extensions.Logging;
using AutoMapper;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using gamitude_backend.Settings;
using MongoDB.Driver;
using gamitude_backend.Data;
using gamitude_backend.Repositories;

namespace gamitude_backend.Services
{
    public interface ITimerService : ITimerRepository
    {

    }
    public class TimerService : TimerRepository , ITimerService
    {
        private readonly IMongoCollection<Timer> _folders;


        public TimerService(IDatabaseCollections dbCollections) : base(dbCollections)
        {
            _folders = dbCollections.timers;
        }


    }
}