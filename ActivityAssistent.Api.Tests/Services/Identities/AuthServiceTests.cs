using System;
using System.Threading;
using System.Threading.Tasks;
using ActivityAssistent.Api.Interfaces.Identity;
using ActivityAssistent.Api.Services;
using ActivityAssistent.Shared.Dtos.Identity;
using Microsoft.Extensions.Configuration;
using Moq;
using Xunit;

namespace ActivityAssistent.Api.Tests.Services.Identities
{
    public class AuthServiceTests
    {
        [Fact]
        public async global::System.Threading.Tasks.Task GetCurrentProfileAsync_WhenUserIsGuest_ReturnsGuestProfile()
        {
            var userRepository = new Mock<IUserRepository>();
            var userContext = new Mock<IUserContext>();
            userContext.SetupGet(context => context.CurrentUserId).Returns(Guid.Empty);
            userContext.SetupGet(context => context.Role).Returns(string.Empty);
            userContext.SetupGet(context => context.SubNrId).Returns((Guid?)null);

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection()
                .Build();

            var service = new AuthService(userRepository.Object, userContext.Object, configuration);

            var result = await service.GetCurrentProfileAsync(CancellationToken.None);

            Assert.Null(result.UserId);
            Assert.Equal("Gastgebruiker", result.FullName);
            Assert.Equal(string.Empty, result.Email);
            Assert.Equal(string.Empty, result.Role);
            Assert.Null(result.SubNrId);
        }

        [Fact]
        public async global::System.Threading.Tasks.Task GetCurrentProfileAsync_WhenUserNotFound_ThrowsUnauthorized()
        {
            var userRepository = new Mock<IUserRepository>();
            var userContext = new Mock<IUserContext>();
            var userId = Guid.NewGuid();
            userContext.SetupGet(context => context.CurrentUserId).Returns(userId);

            userRepository
                .Setup(repository => repository.GetProfileByIdAsync(userId, It.IsAny<CancellationToken>()))
                .ReturnsAsync((UserProfileDto?)null);

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection()
                .Build();

            var service = new AuthService(userRepository.Object, userContext.Object, configuration);

            await Assert.ThrowsAsync<UnauthorizedAccessException>(() => service.GetCurrentProfileAsync(CancellationToken.None));
        }

        [Fact]
        public async global::System.Threading.Tasks.Task LoginAsync_WhenUserNotFound_ReturnsUnauthorizedResult()
        {
            var userRepository = new Mock<IUserRepository>();
            var userContext = new Mock<IUserContext>();
            userRepository
                .Setup(repository => repository.GetUserForLoginByEmailAsync(It.IsAny<string>(), It.IsAny<CancellationToken>()))
                .ReturnsAsync((UserAuthDto?)null);

            var configuration = new ConfigurationBuilder()
                .AddInMemoryCollection()
                .Build();

            var service = new AuthService(userRepository.Object, userContext.Object, configuration);

            var result = await service.LoginAsync(new LoginCredentialsDto
            {
                Email = "unknown@example.com",
                Password = "password"
            }, CancellationToken.None);

            Assert.False(result.IsSuccess);
            Assert.Equal("Ongeldige inloggegevens.", result.ErrorMessage);
        }
    }
}
