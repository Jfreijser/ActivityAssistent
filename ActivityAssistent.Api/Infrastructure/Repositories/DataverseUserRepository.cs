using ActivityAssistent.Shared.Dtos.Identity;
using ActivityAssistent.Shared.Interfaces.Identity;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk.Query;

namespace ActivityAssistent.Api.Infrastructure.Repositories
{
    public class DataverseUserRepository(IOrganizationServiceAsync DataverseClient) : IUserRepository
    {
        public async Task<UserProfileDto?> GetProfileByIdAsync(Guid UserId, CancellationToken Token = default)
        {
            var Result = await DataverseClient.RetrieveAsync("systemuser",UserId,new ColumnSet("fullname", "internalemailaddress", "title"));

            if (Result == null) return null;
            
            return new UserProfileDto
            {
                UserId = UserId,
                FullName = Result.GetAttributeValue<string>("fullname"),
                Email = Result.GetAttributeValue<string>("internalemailaddress"),
                JobTitle = Result.GetAttributeValue<string>("title")
            };
        }
    }
}
