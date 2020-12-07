using gamitude_backend.Models;
using MongoDB.Driver;
using gamitude_backend.Data;
using gamitude_backend.Repositories;

namespace gamitude_backend.Services
{
    public interface IJournalService : IJournalRepository
    {

    }
    public class JournalService : JournalRepository,IJournalService
    {
        private readonly IMongoCollection<Journal> _journals;


        public JournalService(IDatabaseCollections dbCollections) : base(dbCollections)
        {
            _journals = dbCollections.journals;
        }

     
    }
}