using ActivityAssistent.Api.Interfaces.companies;
using ActivityAssistent.Api.Mappings;
using ActivityAssistent.Shared.Dtos.Companies;
using ActivityAssistent.Shared.Dtos.Identity;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Crm.Sdk.Messages;
using Microsoft.PowerPlatform.Dataverse.Client;
using Microsoft.Xrm.Sdk;
using Microsoft.Xrm.Sdk.Query;

namespace ActivityAssistent.Api.Infrastructure.Repositories.DataverseRepository
{
    public class DataverseCompanyRepository(IOrganizationServiceAsync dataverseClient) 
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

        public async Task<Company> GetByIdAsync(Guid CompanyId, CancellationToken Token)
        {
            using var context = new DataverseContext(dataverseClient);
            var DataverseAccount = context.AccountSet.Where(c => c.AccountId == CompanyId).FirstOrDefault();
            //var Copany = await context. .Where(c => c.CompanyId == CompanyId).FirstOrDefaultAsync(Token);
            if (DataverseAccount == null)
            {
                return null;
            }

            var Company = new Company
            {
                CompanyId = DataverseAccount.AccountId ?? Guid.Empty,
                Name = DataverseAccount.Name
                
            };
            return Company;
            //var Query = new QueryExpression("account")
            //{
            //    // Geef aan welke kolommen je wilt ophalen uit Dataverse
            //    ColumnSet = new ColumnSet("name")
            //};

            //Query.Criteria.AddCondition("accountid", ConditionOperator.Equal, CompanyId);

            //var Result = await dataverseClient.RetrieveMultipleAsync(Query);

            //if (Result == null || Result.Entities.Count == 0)
            //{
            //    return null;
            //}
            //return Result.Entities[0].GetAttributeValue<Guid?>("accountid");
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

        public async Task<List<Company>> GetCustomerAsync(CancellationToken Token)
        {
            var Query = new QueryExpression("account")
            {
                ColumnSet = new ColumnSet("accountid","name")
            };
            var Result =  await dataverseClient.RetrieveMultipleAsync(Query);

            var Companies = Result.Entities.Select(DataverseEntity => DataverseEntity.ToDomainEntity()).ToList();

            return Companies;
        }

        public Task UpdateAsync(Company Company, CancellationToken Token)
        {
            throw new NotImplementedException();
        }

        
    }
}
