using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Net;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Http;
using FluentValidation;
using Yape.Transactions.Utils.Exceptions;

namespace Yape.Transactions.Infrastructure.Middleware
{
    public class ExceptionHandlingMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<ExceptionHandlingMiddleware> _logger;

        public ExceptionHandlingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            object response;
            switch (exception)
            {

                case ApiErrorDataException ex:
                    response = new
                    {
                        success = true,
                        codError = ex.ErrorCode,
                        message = ex.Message,
                        error = ex.Message,
                        data = string.Empty
                    };
                    break;
 
                case ValidationException ex:
                    response = new
                    {
                        success = false,
                        codError = "COD400",
                        message = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }),
                        error = ex.Errors.Select(e => new { e.PropertyName, e.ErrorMessage }),
                        data = string.Empty
                    };
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    break;
                default:
                    response = new
                    {
                        success = false,
                        codError = "COD500",
                        message = "Error interno del servidor",
                        error = exception.Message,
                        data = string.Empty
                    };
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    break;
            }

            return context.Response.WriteAsync(JsonSerializer.Serialize(response));
        }

    }
}