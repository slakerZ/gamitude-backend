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

namespace gamitude_backend.Repositories
{
    public interface IFolderRepository
    {
        Task<Folder> getByIdAsync(String id);
        Task<List<Folder>> getByUserIdAsync(String userId);
        Task createAsync(Folder folder);
        Task updateAsync(String id, Folder updateFolder);
        Task deleteByIdAsync(String id);
    }
    public class FolderRepository : IFolderRepository
    {
        private readonly IMongoCollection<Folder> _Folders;


        public FolderRepository(IDatabaseCollections dbCollections)
        {
            _Folders = dbCollections.folders;
        }

        public Task<Folder> getByIdAsync(String id)
        {
            return _Folders.Find<Folder>(Folder => Folder.id == id).FirstOrDefaultAsync();
        }

        public Task<List<Folder>> getByUserIdAsync(String userId)
        {
            return _Folders.Find<Folder>(Folder => Folder.userId == userId).ToListAsync();

        }

        public Task createAsync(Folder Folder)
        {
            return _Folders.InsertOneAsync(Folder);
        }

        public Task updateAsync(String id, Folder newFolder)
        {
            return _Folders.ReplaceOneAsync(Folder => Folder.id == id, newFolder);

        }

        public Task deleteByFolderAsync(Folder FolderIn)
        {
            return _Folders.DeleteOneAsync(Folder => Folder.id == FolderIn.id);
        }

        public System.Threading.Tasks.Task deleteByIdAsync(string id)
        {
            return _Folders.DeleteOneAsync(Folder => Folder.id == id);

        }

    }
}