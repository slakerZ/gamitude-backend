using gamitude_backend.Models;
using System.Collections.Generic;
using System.Threading.Tasks;
using MongoDB.Driver;
using gamitude_backend.Data;

namespace gamitude_backend.Repositories
{
    public interface IPageRepository
    {
        Task<Page> getByIdAsync(string id);
        Task<List<Page>> getByJournalIdAsync(string journalId);
        Task createAsync(Page page);
        Task updateAsync(string id, Page updatePage);
        Task deleteByIdAsync(string id);
    }
    public class PageRepository : IPageRepository
    {
        private readonly IMongoCollection<Page> _page;


        public PageRepository(IDatabaseCollections dbCollections)
        {
            _page = dbCollections.pages;
        }

        public Task<Page> getByIdAsync(string id)
        {
            return _page.Find<Page>(Page => Page.id == id).FirstOrDefaultAsync();
        }

        public Task<List<Page>> getByJournalIdAsync(string journalId)
        {
            return _page.Find<Page>(Page => Page.journalId == journalId).ToListAsync();

        }

        public Task createAsync(Page Page)
        {
            return _page.InsertOneAsync(Page);
        }

        public Task updateAsync(string id, Page newPage)
        {
            return _page.ReplaceOneAsync(Page => Page.id == id, newPage);

        }

        public Task deleteByPageAsync(Page PageIn)
        {
            return _page.DeleteOneAsync(Page => Page.id == PageIn.id);
        }

        public Task deleteByIdAsync(string id)
        {
            return _page.DeleteOneAsync(Page => Page.id == id);

        }

    }
}