using gamitude_backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using gamitude_backend.Data;

namespace gamitude_backend.Repositories
{
    public interface IJournalRepository
    {
        Task<Journal> getByIdAsync(string id);
        Task<List<Journal>> getByUserIdAsync(string userId);
        Task<List<Journal>> getByProjectIdAsync(string userId);
        Task createAsync(Journal journal);
        Task updateAsync(string id, Journal updateJournal);
        Task deleteByIdAsync(string id);
    }
    public class JournalRepository : IJournalRepository
    {
        private readonly IMongoCollection<Journal> _journals;


        public JournalRepository(IDatabaseCollections dbCollections)
        {
            _journals = dbCollections.journals;
        }

        public Task<Journal> getByIdAsync(string id)
        {
            return _journals.Find<Journal>(Journal => Journal.id == id).FirstOrDefaultAsync();
        }

        public Task<List<Journal>> getByUserIdAsync(string userId)
        {
            return _journals.Find<Journal>(Journal => Journal.userId == userId).ToListAsync();
        }
        
        public Task<List<Journal>> getByProjectIdAsync(string projectId)
        {
            return _journals.Find<Journal>(Journal => Journal.projectId == projectId).ToListAsync();
        }

        public Task createAsync(Journal Journal)
        {
            return _journals.InsertOneAsync(Journal);
        }

        public Task updateAsync(string id, Journal newJournal)
        {
            return _journals.ReplaceOneAsync(Journal => Journal.id == id, newJournal);

        }

        public Task deleteByJournalAsync(Journal JournalIn)
        {
            return _journals.DeleteOneAsync(Journal => Journal.id == JournalIn.id);
        }

        public Task deleteByIdAsync(string id)
        {
            return _journals.DeleteOneAsync(Journal => Journal.id == id);

        }

    }
}