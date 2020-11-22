using gamitude_backend.Models;
using MongoDB.Driver;
using gamitude_backend.Data;
using gamitude_backend.Repositories;

namespace gamitude_backend.Services
{
    public interface IPageService : IPageRepository
    {

    }
    public class PageService : PageRepository,IPageService
    {
        private readonly IMongoCollection<Page> _pages;


        public PageService(IDatabaseCollections dbCollections) : base(dbCollections)
        {
            _pages = dbCollections.pages;
        }

     
    }
}