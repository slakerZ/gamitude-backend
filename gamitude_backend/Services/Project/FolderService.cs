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
    public interface IFolderService : IFolderRepository
    {

    }
    public class FolderService : FolderRepository , IFolderService
    {
        private readonly IMongoCollection<Folder> _folders;


        public FolderService(IDatabaseCollections dbCollections) : base(dbCollections)
        {
            _folders = dbCollections.folders;
        }


    }
}