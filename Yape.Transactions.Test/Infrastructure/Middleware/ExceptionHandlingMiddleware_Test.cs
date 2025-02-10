using FluentValidation.Results;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Moq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Yape.Transactions.Infrastructure.Middleware;
using Yape.Transactions.Utils.Exceptions;

namespace Yape.Transactions.Test.Infrastructure.Middleware
{
    public class ExceptionHandlingMiddleware_Test
    {
        private readonly Mock<RequestDelegate> _nextMock;
        private readonly ExceptionHandlingMiddleware _middleware;

        public ExceptionHandlingMiddleware_Test()
        {
            _nextMock = new Mock<RequestDelegate>();
            _middleware = new ExceptionHandlingMiddleware(_nextMock.Object);
        }

        [Fact]
        public async Task InvokeAsync_ShouldCallNextMiddleware_WhenNoException()
        {

            var context = new DefaultHttpContext();
            await _middleware.InvokeAsync(context);
            _nextMock.Verify(next => next(context), Times.Once);
        }

        [Fact]
        public async Task InvokeAsync_ShouldHandleApiErrorDataException()
        {

            var context = new DefaultHttpContext();
            var exception = new ApiErrorDataException("Error message", "ErrorCode");

            _nextMock.Setup(next => next(context)).ThrowsAsync(exception);
            await _middleware.InvokeAsync(context);
            Assert.Equal("application/json", context.Response.ContentType);
            Assert.Equal(200, context.Response.StatusCode);

        }

  

      

        [Fact]
        public async Task InvokeAsync_ShouldHandleGeneralException()
        {

            var context = new DefaultHttpContext();
            var exception = new Exception("General error");

            _nextMock.Setup(next => next(context)).ThrowsAsync(exception);
            await _middleware.InvokeAsync(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            var responseBody = await new StreamReader(context.Response.Body).ReadToEndAsync();

            Assert.Equal("application/json", context.Response.ContentType);
            Assert.Equal(500, context.Response.StatusCode);
        }

    }
}

