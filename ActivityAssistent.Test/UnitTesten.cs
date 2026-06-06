using System.ComponentModel.DataAnnotations;
using ActivityAssistent.Shared.Dtos.Companies;

namespace ActivityAssistent.Test;

public class UnitTesten
{
    [Fact]
    public void CompanyDto_InitializesWithDefaults()
    {
        var dto = new CompanyDto();

        Assert.Equal(Guid.Empty, dto.CompanyId);
        Assert.Equal(string.Empty, dto.CompanyName);
        Assert.Equal(string.Empty, dto.EmailAddress);
        Assert.Equal(string.Empty, dto.PhoneNumber);
        Assert.Equal(string.Empty, dto.City);
        Assert.Equal(string.Empty, dto.Address);
    }

    [Fact]
    public void CreateCompanyDto_InvalidEmail_IsInvalid()
    {
        var dto = new CreateCompanyDto
        {
            Name = "Acme",
            Email = "not-an-email",
            PhoneNumber = "0612345678",
            City = "Utrecht",
            Address = "Example Street 1"
        };

        var results = new List<ValidationResult>();
        var context = new ValidationContext(dto);
        var isValid = Validator.TryValidateObject(dto, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, result => result.ErrorMessage == "Please enter a valid email address.");
    }

    [Fact]
    public void CreateCompanyDto_WithValidData_IsValid()
    {
        var dto = new CreateCompanyDto
        {
            Name = "Acme BV",
            Email = "info@acme.nl",
            PhoneNumber = "+31 6-12345678",
            City = "Utrecht",
            Address = "Example Street 1"
        };

        var results = new List<ValidationResult>();
        var context = new ValidationContext(dto);
        var isValid = Validator.TryValidateObject(dto, context, results, true);

        Assert.True(isValid);
        Assert.Empty(results);
    }

    [Fact]
    public void CreateCompanyDto_NameLongerThan100Chars_IsInvalid()
    {
        var dto = new CreateCompanyDto
        {
            Name = new string('A', 101),
            Email = "test@example.com",
            PhoneNumber = "0612345678",
            City = "Utrecht",
            Address = "Example Street 1"
        };

        var results = new List<ValidationResult>();
        var context = new ValidationContext(dto);
        var isValid = Validator.TryValidateObject(dto, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, result => result.ErrorMessage == "Company Name cannot exceed 100 characters.");
    }

    [Fact]
    public void CreateCompanyDto_InvalidPhoneNumber_IsInvalid()
    {
        var dto = new CreateCompanyDto
        {
            Name = "Acme",
            Email = "test@example.com",
            PhoneNumber = "ABC123",
            City = "Utrecht",
            Address = "Example Street 1"
        };

        var results = new List<ValidationResult>();
        var context = new ValidationContext(dto);
        var isValid = Validator.TryValidateObject(dto, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, result => result.ErrorMessage == "Please enter a valid phone number.");
    }

    [Fact]
    public void CreateCompanyDto_MultipleRequiredFieldsMissing_ReturnsAllRequiredErrors()
    {
        var dto = new CreateCompanyDto();

        var results = new List<ValidationResult>();
        var context = new ValidationContext(dto);
        var isValid = Validator.TryValidateObject(dto, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, result => result.ErrorMessage == "Company Name is required.");
        Assert.Contains(results, result => result.ErrorMessage == "Email Address is required.");
        Assert.Contains(results, result => result.ErrorMessage == "Phone Number is required.");
        Assert.Contains(results, result => result.ErrorMessage == "City is required.");
        Assert.Contains(results, result => result.ErrorMessage == "Address is required.");
    }

    [Fact]
    public void Company_DatabaseModel_MapsExpectedDefaultsAndTypes()
    {
        var entity = new Company();

        Assert.Null(entity.CompanyId);
        Assert.Null(entity.Name);
        Assert.Null(entity.KvkNumber);
        Assert.Null(entity.Email);
        Assert.Null(entity.PhoneNumber);
        Assert.Null(entity.CreatedOn);
        Assert.Null(entity.City);
        Assert.Null(entity.Address);
        Assert.Null(entity.CompanyContact);
    }

    [Fact]
    public void CompanyStore_CreateThenGetById_ReturnsCompany()
    {
        var store = new InMemoryCompanyStore();
        var company = new Company
        {
            Name = "Acme",
            Email = "info@acme.nl",
            PhoneNumber = "0612345678",
            City = "Utrecht",
            Address = "Example Street 1"
        };

        var createdId = store.Create(company);
        var fromStore = store.GetById(createdId);

        Assert.NotNull(fromStore);
        Assert.Equal(createdId, fromStore!.CompanyId);
        Assert.Equal("Acme", fromStore.Name);
        Assert.Equal("info@acme.nl", fromStore.Email);
    }

    [Fact]
    public void CompanyStore_CreateThenDelete_RemovesCompany()
    {
        var store = new InMemoryCompanyStore();
        var createdId = store.Create(new Company { Name = "Acme" });

        var deleted = store.Delete(createdId);
        var fromStore = store.GetById(createdId);

        Assert.True(deleted);
        Assert.Null(fromStore);
        Assert.Equal(0, store.Count);
    }

    private sealed class InMemoryCompanyStore
    {
        private readonly Dictionary<Guid, Company> _companies = new();

        public int Count => _companies.Count;

        public Guid Create(Company company)
        {
            var id = Guid.NewGuid();
            company.CompanyId = id;
            _companies[id] = company;
            return id;
        }

        public Company? GetById(Guid companyId)
        {
            _companies.TryGetValue(companyId, out var company);
            return company;
        }

        public bool Delete(Guid companyId)
        {
            return _companies.Remove(companyId);
        }
    }
}
