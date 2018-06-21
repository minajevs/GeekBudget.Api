using System;
using System.Collections.Generic;
using System.Text;
using GeekBudget.Middleware;
using GeekBudget.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Moq;
using Xunit;

namespace GeekBudget.Tests.Middleware
{
    // TODO: Is it still needed???
    public class ErrorWrapperTests
    {
        [Fact]
        public async void Invoke_NoErrorOKResult()
        {
            //Arrange
            var errorWrapper = new ErrorWrapper(next: async (innerHttpContext) =>
            {
                await innerHttpContext.Response.WriteAsync("authorized body");
            });

            // context
            var defaultContext = new DefaultHttpContext();
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.Setup(context => context.Request)
                .Returns(defaultContext.Request);
            httpContextMock.Setup(context => context.Response)
                .Returns(defaultContext.Response);
            var requestContext = httpContextMock.Object;

            //Act
            await errorWrapper.Invoke(requestContext);

            //Assert
            Assert.Equal(200, requestContext.Response.StatusCode);
        }
    }
}
