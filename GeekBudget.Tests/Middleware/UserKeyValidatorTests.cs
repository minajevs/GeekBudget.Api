using System;
using System.Collections.Generic;
using System.Text;
using GeekBudget.Middleware;
using GeekBudget.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Primitives;
using Moq;
using Xunit;

namespace GeekBudget.Test.Middleware
{
    public class UserKeyValidatorTests
    {
        [Fact]
        public async void CanAuthorizeRequest()
        {
            //Arrange
            var nextAuthorizedRequestCalled = false;
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(r => r.CheckValidUserKey(It.IsAny<string>())).Returns(true);  //Validates to true
            userRepoMock.Setup(r => r.AreContactsEmpty()).Returns(false);                    //Contacts are not empty

            var userKeyValidator = new UserKeyValidator(next: async (innerHttpContext) =>
            {
                nextAuthorizedRequestCalled = true;
                await innerHttpContext.Response.WriteAsync("authorized body");
            }, _repo: userRepoMock.Object);

            var requestContext = new DefaultHttpContext();
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
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(r => r.CheckValidUserKey(It.IsAny<string>())).Returns(false);  //Validates to false
            userRepoMock.Setup(r => r.AreContactsEmpty()).Returns(false);                    //Contacts are not empty

            var userKeyValidator = new UserKeyValidator(next: async (innerHttpContext) =>
            {
                nextAuthorizedRequestCalled = true;
                await innerHttpContext.Response.WriteAsync("authorized body");
            }, _repo: userRepoMock.Object);

            var requestContext = new DefaultHttpContext();
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
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(r => r.CheckValidUserKey(It.IsAny<string>())).Returns(false);  //Validates to false
            userRepoMock.Setup(r => r.AreContactsEmpty()).Returns(false);                    //Contacts are not empty

            var userKeyValidator = new UserKeyValidator(next: async (innerHttpContext) =>
            {
                nextAuthorizedRequestCalled = true;
                await innerHttpContext.Response.WriteAsync("authorized body");
            }, _repo: userRepoMock.Object);

            var requestContext = new DefaultHttpContext();
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
            var userRepoMock = new Mock<IUserRepository>();
            userRepoMock.Setup(r => r.CheckValidUserKey(It.IsAny<string>())).Returns(false);  //Validates to false
            userRepoMock.Setup(r => r.AreContactsEmpty()).Returns(true);                      //Contacts are empty

            var userKeyValidator = new UserKeyValidator(next: async (innerHttpContext) =>
            {
                nextAuthorizedRequestCalled = true;
                await innerHttpContext.Response.WriteAsync("authorized body");
            }, _repo: userRepoMock.Object);

            var requestContext = new DefaultHttpContext();
            requestContext.Request.Headers.Add(new KeyValuePair<string, StringValues>("user-key", "password"));

            //Act
            await userKeyValidator.Invoke(requestContext);

            //Assert
            Assert.Equal(200, requestContext.Response.StatusCode);
            Assert.True(nextAuthorizedRequestCalled);
        }
    }
}
