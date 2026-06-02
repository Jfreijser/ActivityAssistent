using System;
using ActivityAssistent.Api.Infrastructure;
using Microsoft.AspNetCore.Http;
using Xunit;

namespace ActivityAssistent.Api.Tests.Infrastructure
{
    public class UserContextTests
    {
        [Fact]
        public void CurrentUserId_WhenNoClaims_ReturnsEmptyGuid()
        {
            var httpContext = new DefaultHttpContext();
            var accessor = new HttpContextAccessor { HttpContext = httpContext };
            var context = new UserContext(accessor);

            var result = context.CurrentUserId;

            Assert.Equal(Guid.Empty, result);
        }
    }
}
