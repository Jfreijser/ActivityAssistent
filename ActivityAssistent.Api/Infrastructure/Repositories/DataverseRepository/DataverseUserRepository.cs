//using ActivityAssistent.Shared.Dtos.Identity;
//using ActivityAssistent.Shared.Interfaces.Identity;
//using Microsoft.PowerPlatform.Dataverse.Client;
//using Microsoft.Xrm.Sdk;
//using Microsoft.Xrm.Sdk.Query;

//namespace ActivityAssistent.Api.Infrastructure.Repositories.DataverseRepository
//{
//    public class DataverseUserRepository(IOrganizationServiceAsync DataverseClient)
//        public async Task<UserProfileDto?> GetProfileByIdAsync(Guid UserId, CancellationToken Token = default)
//        {

//            var Result = await DataverseClient.RetrieveAsync(
//                "cre7e_externalcustomer",
//                UserId,
//                new ColumnSet("cre7e_name", "cre7e_emailaddress")
//            );

//            if (Result == null) return null;

//            return new UserProfileDto
//            {
//                UserId = UserId,
//                FullName = Result.GetAttributeValue<string>("cre7e_name"),
//                Email = Result.GetAttributeValue<string>("cre7e_emailaddress"),
//                JobTitle = "Sales Representative" 
//            };
//        }
        
//        public async Task<UserAuthDto?> GetUserForLoginByEmailAsync(string Email, CancellationToken Token = default)
//        {

//            //if (DataverseClient.IsReady)
//            //{
//            //    Console.WriteLine("Succesvol verbonden met Dataverse!");
//            //    IOrganizationService service = (IOrganizationService)DataverseClient;

//            //    // Voorbeeld aanroep: Haal de naam van de eerste 5 accounts op
//            //    QueryExpression query = new QueryExpression("account") { ColumnSet = new ColumnSet("name") };
//            //    EntityCollection results = service.RetrieveMultiple(query);

//            //    foreach (var entity in results.Entities)
//            //    {
//            //        Console.WriteLine($"Account Naam: {entity.GetAttributeValue<string>("name")}");
//            //    }
//            //}
//            //else
//            //{
//            //    Console.WriteLine($"Verbinding mislukt: {serviceClient.LastError}");
//            //}
        

//        // the table in dataverse 
//        var Query = new QueryExpression("cre7e_externalcustomer");
           
//            Query.ColumnSet = new ColumnSet(
//                "cre7e_externalcustomerid",
//                "cre7e_name",
//                "cre7e_emailaddress",
//                "cre7e_passwordhashed"
//            );
//            Query.Criteria.AddCondition("cre7e_emailaddress", ConditionOperator.Equal, Email);

//            var Result = await DataverseClient.RetrieveMultipleAsync(Query);

//            if (Result.Entities.Count == 0) return null;

//            var User = Result.Entities[0];

//            return new UserAuthDto
//            {
//                UserId = User.Id,
//                FullName = User.GetAttributeValue<string>("cre7e_name"),
//                Email = User.GetAttributeValue<string>("cre7e_emailaddress"),
//                PasswordHash = User.GetAttributeValue<string>("cre7e_passwordhashed")
//            };
//        }
//    }
//}
