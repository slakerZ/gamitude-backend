using gamitude_backend.Models;
using System.Threading.Tasks;
using MongoDB.Driver;
using gamitude_backend.Data;

namespace gamitude_backend.Repositories
{
    public interface IThemeRepository
    {
        Task<Theme> getByIdAsync(string id);
        Task createAsync(Theme theme);
        Task updateAsync(string id, Theme updateTheme);
        Task deleteByIdAsync(string id);
    }
    public class ThemeRepository : IThemeRepository
    {
        private readonly IMongoCollection<Theme> _Themes;


        public ThemeRepository(IDatabaseCollections dbCollections)
        {
            _Themes = dbCollections.themes;
        }

        public Task<Theme> getByIdAsync(string id)
        {
            return _Themes.Find<Theme>(Theme => Theme.id == id).FirstOrDefaultAsync();
        }

        public Task createAsync(Theme Theme)
        {
            return _Themes.InsertOneAsync(Theme);
        }

        public Task updateAsync(string id, Theme newTheme)
        {
            return _Themes.ReplaceOneAsync(Theme => Theme.id == id, newTheme);

        }

        public Task deleteByThemeAsync(Theme ThemeIn)
        {
            return _Themes.DeleteOneAsync(Theme => Theme.id == ThemeIn.id);
        }

        public System.Threading.Tasks.Task deleteByIdAsync(string id)
        {
            return _Themes.DeleteOneAsync(Theme => Theme.id == id);

        }

    }
}