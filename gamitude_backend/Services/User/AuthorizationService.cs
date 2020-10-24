using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using gamitude_backend.Dto.Authorization;
using gamitude_backend.Exceptions;
using gamitude_backend.Settings;
using gamitude_backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;
using MongoDB.Driver;

namespace gamitude_backend.Services
{
    public interface IAuthorizationService
    {
        Task<UserToken> authorizeUserAsync(String login,String password);
    }
    public class AuthorizationService : IAuthorizationService
    {
        //TODO make async
        private readonly IMapper _mapper;
        private readonly IMongoCollection<UserToken> _UsersToken;

        private readonly SignInManager<User> _signInManager;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<AuthorizationService> _logger;
        //TODO inject configuration
        public AuthorizationService(
            ILogger<AuthorizationService> logger,
            IMapper mapper,
            IDatabaseSettings settings,
            IOptions<JwtSettings> jwtSettings,
            SignInManager<User> signInManager)
        {
            var client = new MongoClient(settings.connectionString);
            var database = client.GetDatabase(settings.databaseName);

            _UsersToken = database.GetCollection<UserToken>(settings.usersTokenCollectionName);
            _logger = logger;
            _mapper = mapper;
            _signInManager = signInManager;
            _jwtSettings = jwtSettings.Value;
        }


        public async Task<UserToken> authorizeUserAsync(String login,String password)
        {
            _logger.LogInformation("In authorizeUserAsync");

            var result = await _signInManager.PasswordSignInAsync(login,
                               password,isPersistent: false, lockoutOnFailure: false);
            if (!result.Succeeded)
            {
                throw new LoginException("passwordWrongErrorMessage");
            }
            _logger.LogInformation("authentication successful");
            
            var user  = await _signInManager.UserManager.FindByNameAsync(login);
            _logger.LogInformation("User:"+ user.name);
            // authentication successful so generate jwt token
            var tokenHandler = new JwtSecurityTokenHandler();
            var key = Encoding.ASCII.GetBytes(_jwtSettings.secret);
            var Expires = DateTime.UtcNow.AddDays(1);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.name),
                    new Claim(ClaimTypes.NameIdentifier, user.Id.ToString())
                }),
                Expires = Expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            UserToken userToken = new UserToken();
            userToken.token = tokenHandler.WriteToken(token);
            userToken.userId = user.Id.ToString();
            userToken.dateExpires = Expires;
            return await createOrUpdateTokenAsync(userToken);
        }

        private async Task<UserToken> createOrUpdateTokenAsync(UserToken userToken)
        {
            _logger.LogInformation("In createOrUpdateTokenAsync");
            UserToken user = await _UsersToken.Find(UserToken => UserToken.userId == userToken.userId).FirstOrDefaultAsync();
            if (user == null)
            {
                await _UsersToken.InsertOneAsync(userToken);
            }
            else
            {
                userToken.id=user.id;
                await _UsersToken.ReplaceOneAsync(UserToken => UserToken.userId == userToken.userId, userToken);
            }
            return userToken;
        }

        public Task RemoveAsync(UserToken userToken) 
        {
            _logger.LogInformation("In RemoveAsync userToken");
            return _UsersToken.DeleteOneAsync(UserToken => UserToken.id == userToken.id);
        }

        public Task RemoveAsync(string id)
        {
            _logger.LogInformation("In RemoveAsync id");
            return _UsersToken.DeleteOneAsync(UserToken => UserToken.id == id);
        }
    }
}