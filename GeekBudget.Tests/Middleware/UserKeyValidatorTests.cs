using System;
using System.Collections.Generic;
using GeekBudget.Middleware;
using GeekBudget.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Moq;
using Xunit;

namespace GeekBudget.Tests.Middleware
{
    public class UserKeyValidatorTests
    {
        [Fact]
        public async void CanAuthorizeRequest()
        {
            //Arrange
            var nextAuthorizedRequestCalled = false;

            // userrepo
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(r => r.CheckValidUserKey(It.IsAny<string>())).Returns(true);  //Validates to true
            userRepoMock.Setup(r => r.AreContactsEmpty()).Returns(false);                    //Contacts are not empty

            var userKeyValidator = new UserKeyValidator(next: async (innerHttpContext) =>
            {
                nextAuthorizedRequestCalled = true;
                await innerHttpContext.Response.WriteAsync("authorized body");
            });

            // service provider
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(provider => provider.GetService(typeof(IUserRepository)))
                .Returns(userRepoMock.Object);

            // context
            var defaultContext = new DefaultHttpContext();
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.SetupGet(context => context.RequestServices)
                .Returns(serviceProviderMock.Object);
            httpContextMock.Setup(context => context.Request)
                .Returns(defaultContext.Request);
            httpContextMock.Setup(context => context.Response)
                .Returns(defaultContext.Response);
            var requestContext = httpContextMock.Object;

            requestContext.Request.Headers.Add(new KeyValuePair<string, StringValues>("user-key", "password"));

            //Act
            await userKeyValidator.Invoke(requestContext);

            //Assert
            Assert.Equal(200, requestContext.Response.StatusCode);
            Assert.True(nextAuthorizedRequestCalled);
        }

        [Fact]
        public async void CanDeclineWrongRequest()
        {
            //Arrange
            var nextAuthorizedRequestCalled = false;

            // userrepo
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(r => r.CheckValidUserKey(It.IsAny<string>())).Returns(false);  //Validates to true
            userRepoMock.Setup(r => r.AreContactsEmpty()).Returns(false);                    //Contacts are not empty

            var userKeyValidator = new UserKeyValidator(next: async (innerHttpContext) =>
            {
                nextAuthorizedRequestCalled = true;
                await innerHttpContext.Response.WriteAsync("authorized body");
            });

            // service provider
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(provider => provider.GetService(typeof(IUserRepository)))
                .Returns(userRepoMock.Object);

            // context
            var defaultContext = new DefaultHttpContext();
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.SetupGet(context => context.RequestServices)
                .Returns(serviceProviderMock.Object);
            httpContextMock.Setup(context => context.Request)
                .Returns(defaultContext.Request);
            httpContextMock.Setup(context => context.Response)
                .Returns(defaultContext.Response);
            var requestContext = httpContextMock.Object;

            requestContext.Request.Headers.Add(new KeyValuePair<string, StringValues>("user-key", "password"));

            //Act
            await userKeyValidator.Invoke(requestContext);

            //Assert
            Assert.Equal(401, requestContext.Response.StatusCode);
            Assert.False(nextAuthorizedRequestCalled);
        }

        [Fact]
        public async void CanDeclineRequestWithoutAuthtoken()
        {
            //Arrange
            var nextAuthorizedRequestCalled = false;
            // userrepo
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(r => r.CheckValidUserKey(It.IsAny<string>())).Returns(false);  //Validates to false
            userRepoMock.Setup(r => r.AreContactsEmpty()).Returns(false);                    //Contacts are not empty

            var userKeyValidator = new UserKeyValidator(next: async (innerHttpContext) =>
            {
                nextAuthorizedRequestCalled = true;
                await innerHttpContext.Response.WriteAsync("authorized body");
            });

            // service provider
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(provider => provider.GetService(typeof(IUserRepository)))
                .Returns(userRepoMock.Object);

            // context
            var defaultContext = new DefaultHttpContext();
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.SetupGet(context => context.RequestServices)
                .Returns(serviceProviderMock.Object);
            httpContextMock.Setup(context => context.Request)
                .Returns(defaultContext.Request);
            httpContextMock.Setup(context => context.Response)
                .Returns(defaultContext.Response);
            var requestContext = httpContextMock.Object;

            //requestContext.Request.Headers.Add(new KeyValuePair<string, StringValues>("user-key", "password")); //No header in message!!!

            //Act
            await userKeyValidator.Invoke(requestContext);

            //Assert
            Assert.Equal(400, requestContext.Response.StatusCode);
            Assert.False(nextAuthorizedRequestCalled);
        }

        [Fact]
        public async void DoNotCheckAuthIfNoUsersExists()
        {
            //Arrange
            var nextAuthorizedRequestCalled = false;
            // userrepo
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(r => r.CheckValidUserKey(It.IsAny<string>())).Returns(false);  //Validates to false
            userRepoMock.Setup(r => r.AreContactsEmpty()).Returns(true);                    //Contacts are empty

            var userKeyValidator = new UserKeyValidator(next: async (innerHttpContext) =>
            {
                nextAuthorizedRequestCalled = true;
                await innerHttpContext.Response.WriteAsync("authorized body");
            });

            // service provider
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(provider => provider.GetService(typeof(IUserRepository)))
                .Returns(userRepoMock.Object);

            // context
            var defaultContext = new DefaultHttpContext();
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.SetupGet(context => context.RequestServices)
                .Returns(serviceProviderMock.Object);
            httpContextMock.Setup(context => context.Request)
                .Returns(defaultContext.Request);
            httpContextMock.Setup(context => context.Response)
                .Returns(defaultContext.Response);
            var requestContext = httpContextMock.Object;

            requestContext.Request.Headers.Add(new KeyValuePair<string, StringValues>("user-key", "password"));

            //Act
            await userKeyValidator.Invoke(requestContext);

            //Assert
            Assert.Equal(200, requestContext.Response.StatusCode);
            Assert.True(nextAuthorizedRequestCalled);
        }

        [Fact]
        public async void DoNotCheckAuthIfRequestIsCorsOptions()
        {
            //Arrange
            var nextAuthorizedRequestCalled = false;
            // userrepo
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(r => r.CheckValidUserKey(It.IsAny<string>())).Returns(false);  //Validates to false
            userRepoMock.Setup(r => r.AreContactsEmpty()).Returns(false);                    //Contacts are not empty

            var userKeyValidator = new UserKeyValidator(next: async (innerHttpContext) =>
            {
                nextAuthorizedRequestCalled = true;
                await innerHttpContext.Response.WriteAsync("authorized body");
            });

            // service provider
            var serviceProviderMock = new Mock<IServiceProvider>();
            serviceProviderMock.Setup(provider => provider.GetService(typeof(IUserRepository)))
                .Returns(userRepoMock.Object);

            // context
            var defaultContext = new DefaultHttpContext();
            var httpContextMock = new Mock<HttpContext>();
            httpContextMock.SetupGet(context => context.RequestServices)
                .Returns(serviceProviderMock.Object);
            httpContextMock.Setup(context => context.Request)
                .Returns(defaultContext.Request);
            httpContextMock.Setup(context => context.Response)
                .Returns(defaultContext.Response);
            var requestContext = httpContextMock.Object;

            requestContext.Request.Method = "OPTIONS";
            requestContext.Request.Headers.Add(new KeyValuePair<string, StringValues>("user-key", "password"));

            //Act
            await userKeyValidator.Invoke(requestContext);

            //Assert
            Assert.Equal(200, requestContext.Response.StatusCode);
            Assert.True(nextAuthorizedRequestCalled);
        }
    }
}
