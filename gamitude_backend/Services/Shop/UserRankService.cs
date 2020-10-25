using gamitude_backend.Models;

using MongoDB.Driver;
using gamitude_backend.Repositories;
using gamitude_backend.Data;
using System.Threading.Tasks;
using System;

namespace gamitude_backend.Services
{


    public interface IUserRankService : IUserRankRepository
    {
    }

    public class UserRankService : UserRankRepository, IUserRankService
    {
        private readonly IRankRepository _rankRepository;

        public UserRankService(IDatabaseCollections dbCollections) : base(dbCollections)
        {
        }

    }
}