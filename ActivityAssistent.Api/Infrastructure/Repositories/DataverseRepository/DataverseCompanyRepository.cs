using ActivityAssistent.Api.Mappings;
using ActivityAssistent.Shared.Dtos.Companies;
using ActivityAssistent.Shared.Dtos.Identity;
using ActivityAssistent.Shared.Interfaces.companies;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.ReportingServices.ReportProcessing.ReportObjectModel;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace ActivityAssistent.Api.Infrastructure.Repositories.DataverseRepository
{
    public class DataverseCompanyRepository(IOrganizationServiceAsync dataverseClient) : ICompanyRepository
    {
        public async Task<Company> CreateAsync(Company Company, CancellationToken Token)
        {
            var DataverseEntity = Company.ToDataverseEntity();
            try
            {
                Guid NewGuid = await dataverseClient.CreateAsync(DataverseEntity);
                Company.CompanyId = NewGuid;
                return Company;
            }
            catch (Exception ex)
            {

                Console.WriteLine($"somthing went wrong: {ex}");
                return null;
            }
           

           

        }

        public Task DeleteAsync(Guid CompanyId, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public Task<IEnumerable<Company>> GetAllAsync(CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public Task<Company> GetByIdAsync(Guid CompanyId, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        public async Task<string> GetByNameAsync(string Name, CancellationToken Token)
        {
           
            var Query = new QueryExpression("account")
            {
                // Geef aan welke kolommen je wilt ophalen uit Dataverse
                ColumnSet = new ColumnSet("name")
            };

            Query.Criteria.AddCondition("name", ConditionOperator.Equal, Name);

            var Result = await dataverseClient.RetrieveMultipleAsync(Query);

            if (Result == null || Result.Entities.Count == 0)
            {
                return null;
            }
            return Result.Entities[0].GetAttributeValue<string>("name");

        }

        public Task UpdateAsync(Company Company, CancellationToken Token)
        {
            throw new NotImplementedException();
        }
    }
}
