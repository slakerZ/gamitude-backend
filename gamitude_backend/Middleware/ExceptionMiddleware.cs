

using System;
using System.Linq;
using System.Net;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using gamitude_backend.Dto;
using gamitude_backend.Exceptions;
using gamitude_backend.Extensions;
using Microsoft.AspNetCore.Http;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Localization;
using Microsoft.Extensions.Logging;
using MongoDB.Driver;

namespace gamitude_backend.Middleware
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionMiddleware> _logger;
        private readonly IStringLocalizer<ExceptionMiddleware> _localizer;

        public ExceptionMiddleware(RequestDelegate next, ILogger<ExceptionMiddleware> logger, IStringLocalizer<ExceptionMiddleware> localizer)
        {
            _logger = logger;
            _localizer = localizer;
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            String message = null;
            try
            {
                await _next(httpContext);
            }
            catch (LoginException ex)
            {
                _logger.LogWarning($"Login Exception: {ex}");
                message = handleLoginExceptionAsync(httpContext, ex);
            }
            catch (IdentityException ex)
            {
                _logger.LogWarning($"IdentityException: {ex}");
                message = handleIdentityExceptionAsync(httpContext, ex);
            }
            catch (MongoException ex)
            {
                _logger.LogError($"MongoException: {ex}");
                message = handleMongoExceptionAsync(httpContext, ex);
            }
            catch (ArgumentException ex)
            {
                _logger.LogError($"ArgumentException: {ex}");
                message = handleArgumentExceptionAsync(httpContext, ex);
            }
            catch (UnauthorizedAccessException ex)
            {
                _logger.LogWarning($"UnauthorizedAccessException: {ex}");
                message = handleUnauthorizedAccessExceptionAsync(httpContext, ex);

            }
            catch (Exception ex)
            {
                _logger.LogError($"Something went wrong: {ex}");
                message = handleExceptionAsync(httpContext, ex);
            }

            if (message != null)
            {
                await httpContext.Response.WriteAsync(new ControllerErrorResponse()
                {
                    message = message
                }.ToString());
            }

        }

        public String handleMongoExceptionAsync(HttpContext context, MongoException ex)
        {
            var message = "Huston we got a database problem";
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                message = ex.ToString();// ------------------------------------------FOR DEVELOPMENT PURPOSE
            }
            return message;

        }

        // Used for handling login exceptions maybe create LoginException?? 
        public String handleArgumentExceptionAsync(HttpContext context, ArgumentException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            var message = ex.Message;
            // var message = _localizer["defaultErrorMessage"];
            return message;

        }

        public String handleUnauthorizedAccessExceptionAsync(HttpContext context, UnauthorizedAccessException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            var message = ex.Message;
            // var message = _localizer["defaultErrorMessage"];
            return message;

        }

        public String handleLoginExceptionAsync(HttpContext context, LoginException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            var message = _localizer[ex.Message];
            return message;
        }

        //For latter upgrade
        //http://www.ziyad.info/en/articles/20-Localizing_Identity_Error_Messages
        public String handleIdentityExceptionAsync(HttpContext context, IdentityException ex)
        {
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.Unauthorized;
            var message = ex.errors.Aggregate("", (s, o) => s + o.Description + "\n");
            return message;
        }

        public String handleExceptionAsync(HttpContext context, Exception ex)
        {
            var message = "Huston we got an undefined problem";
            context.Response.ContentType = "application/json";
            context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
            // message = "something went wrong"
            // message = _localizer["defaultErrorMessage"];

            if (Environment.GetEnvironmentVariable("ASPNETCORE_ENVIRONMENT") == "Development")
            {
                message = ex.ToString();// ------------------------------------------FOR DEVELOPMENT PURPOSE
            }
            return message;
        }
    }
}