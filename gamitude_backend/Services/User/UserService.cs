
using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Claims;
using System.Threading.Tasks;
using AutoMapper;
using gamitude_backend.Dto.Authorization;
using gamitude_backend.Dto.User;
using gamitude_backend.Exceptions;
using gamitude_backend.Settings;
using gamitude_backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using gamitude_backend.Repositories;

namespace gamitude_backend.Services
{
    public interface IUserService
    {
        Task<List<User>> getAllAsync(int offset, int limit);
        Task<User> getByIdAsync(String id);
        Task<User> createAsync(User newUser, String password);
        Task<User> getByUserNameAsync(String userName);
        Task changePasswordAsync(String id, String oldPassword, String newPassword);
        Task<User> updateAsync(User updateUser);
        Task deleteByIdAsync(String id);
    }
    public class UserService : IUserService
    {
        private readonly ILogger<UserService> _logger;

        //TODO make async
        private readonly UserManager<User> _userManager;
        private readonly IMapper _mapper;
        private readonly IRankRepository _rankRepository;

        public UserService(ILogger<UserService> logger, UserManager<User> userManager, IMapper mapper,IRankRepository rankRepository)
        {
            _logger = logger;
            _userManager = userManager;
            _mapper = mapper;
            _rankRepository = rankRepository;
        }

        public async Task<User> createAsync(User newUser, String password)
        {
            newUser.currentRank = await _rankRepository.getRookieAsync();
            newUser.purchasedRanks.Add(newUser.currentRank);
            var result = await _userManager.CreateAsync(newUser, password);
            if (result.Succeeded)
            {
                return newUser;
            }
            else
            {
                var s = result.Errors.AsEnumerable();
                throw new IdentityException(s);
            }
        }

        public async Task<List<User>> getAllAsync(int offset, int limit)
        {
            var users = await _userManager.Users
                .OrderByDescending(o => o.Id)
                .Skip(offset)
                .Take(limit)
                .ToListAsync();
            return users;
        }

        public Task<User> getByIdAsync(String id)
        {
            return _userManager.FindByIdAsync(id);
        }

        public Task<User> getByUserNameAsync(String userName)
        {
            return _userManager.FindByNameAsync(userName);
        }

        public async Task<User> updateAsync(User updateUser)
        {
            var user = await _userManager.FindByIdAsync(updateUser.Id.ToString());
            user = _mapper.Map<User, User>(updateUser, user);
            var result = await _userManager.UpdateAsync(user);
            if (result.Succeeded)
            {
                return user;
            }
            else
            {
                var s = result.Errors.AsEnumerable();
                throw new IdentityException(s);
            }
        }
        public async Task changePasswordAsync(String id, String oldPassword, String newPassword)
        {
            var user = await _userManager.FindByIdAsync(id);
            var result = await _userManager.ChangePasswordAsync(user, oldPassword, newPassword);
            if (!result.Succeeded)
            {
                var s = result.Errors.AsEnumerable();
                throw new IdentityException(s);
            }
        }

        public async Task deleteByIdAsync(String id)
        {
            var user = await _userManager.FindByIdAsync(id);
            var result = await _userManager.DeleteAsync(user);
            if (!result.Succeeded)
            {
                var s = result.Errors.AsEnumerable();
                throw new IdentityException(s);
            }
        }
    }

}