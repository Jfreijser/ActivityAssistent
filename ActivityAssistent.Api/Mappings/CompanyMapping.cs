using ActivityAssistent.Shared.Dtos.Companies;
using Microsoft.Xrm.Sdk;

namespace ActivityAssistent.Api.Mappings
{
    public static class CompanyMapping
    {
        public static Entity ToDataverseEntity(this Company Company)
        {
            var DataverseEntity = new Entity("account");

            // Als de ID al bekend is (bijv. bij een Update), geef hem dan mee
            if (Company.CompanyId.HasValue && Company.CompanyId.Value != Guid.Empty)
            {
                DataverseEntity.Id = Company.CompanyId.Value;
            }

            DataverseEntity["name"] = Company.Name;
            DataverseEntity["telephone1"] = Company.PhoneNumber;
            DataverseEntity["emailaddress1"] = Company.Email;
            DataverseEntity["address1_city"] = Company.City;
            DataverseEntity["address1_composite"] = Company.Address;
            DataverseEntity["primarycontactid"] = Company.CompanyContact.HasValue && Company.CompanyContact.Value != Guid.Empty ? new EntityReference("contact", Company.CompanyContact.Value) : null;

            return DataverseEntity;
        }
        public static Company ToEntity(this CreateCompanyDto Dto)
        {
            return new Company
            {
                Name = Dto.Name,
                Email = Dto.Email,
                PhoneNumber = Dto.PhoneNumber,
                City = Dto.City,
                Address = Dto.Address
            };
        }

        // 3. Van interne Company Entity naar de uiteindelijke CompanyDto (Terug naar Blazor)
        public static CompanyDto ToDto(this Company Entity)
        {

            return new CompanyDto
            {
                // Als de Guid null is, maken we er een Empty Guid van zodat de UI niet crasht
                CompanyId = Entity.CompanyId ?? Guid.Empty,

                // Defensieve fallback naar string.Empty als Dataverse een leeg veld teruggaf
                CompanyName = Entity.Name ?? string.Empty,
                EmailAddress = Entity.Email ?? string.Empty,
                PhoneNumber = Entity.PhoneNumber ?? string.Empty,
                City = Entity.City ?? string.Empty,
                Address = Entity.Address ?? string.Empty
            };

        }

        public static Company ToDomainEntity(this Entity DataverseEntity)
        {
            if (DataverseEntity == null) return null;

            return new Company
            {
                // DataverseEntity.Id bevat altijd de unieke GUID van het record
                CompanyId = DataverseEntity.Id,

                // Gebruik GetAttributeValue met de exacte schema-namen uit je QueryExpression
                Name = DataverseEntity.GetAttributeValue<string>("name"),
                PhoneNumber = DataverseEntity.GetAttributeValue<string>("telephone1"),
                Email = DataverseEntity.GetAttributeValue<string>("emailaddress1"),
                City = DataverseEntity.GetAttributeValue<string>("address1_city"),
                Address = DataverseEntity.GetAttributeValue<string>("address1_composite"),

                // Voor een EntityReference (lookup) halen we de Id op als deze bestaat
                CompanyContact = DataverseEntity.GetAttributeValue<EntityReference>("primarycontactid")?.Id
            };
        }

        public static CustomerDto ToCustomerDto(this Company Company)
        {
            return new CustomerDto
            {
                CustomerId = Company.CompanyId.Value,
                CustomerName = Company.Name
            };
        }
    }
}

