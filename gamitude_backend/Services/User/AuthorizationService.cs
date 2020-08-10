using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;
using AutoMapper;
using gamitude_backend.Data;
using gamitude_backend.Dto.Authorization;
using gamitude_backend.Exceptions;
using gamitude_backend.Settings;
using gamitude_backend.Models;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using Microsoft.IdentityModel.Tokens;

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
        private readonly DataContext _dbContext;
        private readonly SignInManager<User> _signInManager;
        private readonly JwtSettings _jwtSettings;
        private readonly ILogger<AuthorizationService> _logger;
        //TODO inject configuration
        public AuthorizationService(
            ILogger<AuthorizationService> logger,
            IMapper mapper,
            DataContext dbContext,
            IOptions<JwtSettings> jwtSettings,
            SignInManager<User> signInManager)
        {
            _logger = logger;
            _mapper = mapper;
            _dbContext = dbContext;
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
            var key = Encoding.ASCII.GetBytes(_jwtSettings.secret);//TODO inject Config
            var Expires = DateTime.UtcNow.AddDays(1);
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Id),
                    new Claim(ClaimTypes.NameIdentifier, user.Id)
                }),
                Expires = Expires,
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(key), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            UserToken userToken = new UserToken();
            userToken.token = tokenHandler.WriteToken(token);
            userToken.userId = user.Id;
            userToken.date_expires = Expires;
            return await createOrUpdateTokenAsync(userToken);
        }

        private async Task<UserToken> createOrUpdateTokenAsync(UserToken newUserToken)
        {
            _logger.LogInformation("In createOrUpdateToken");
            UserToken userToken = await _dbContext.userTokens.FirstOrDefaultAsync(u => u.userId == newUserToken.userId);
            if (null != userToken)
            {
                userToken.date_expires = newUserToken.date_expires;
                userToken.token = newUserToken.token;
            }
            else
            {
                userToken = newUserToken;
                await _dbContext.userTokens.AddAsync(userToken);
            }
            await _dbContext.SaveChangesAsync();
            return await _dbContext.userTokens.Include(o => o.user).FirstOrDefaultAsync(u => u.id == userToken.id);
        }

        //TODO ask if user can have multiple tokens?? optimize query this makes 2 calls
        private void revokeToken(String userId)
        {
            _logger.LogInformation("In createOrUpdateToken");
            _dbContext.Remove(_dbContext.userTokens.Single(u => u.userId == userId));
            _dbContext.SaveChanges();
        }
    }
}