using System.ComponentModel.DataAnnotations;
using ActivityAssistent.Shared.Dtos.Companies;

namespace ActivityAssistent.Test;

public class UnitTest1
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
    public void CreateCompanyDto_MissingName_IsInvalid()
    {
        var dto = new CreateCompanyDto
        {
            Email = "test@example.com",
            PhoneNumber = "0612345678",
            City = "Utrecht",
            Address = "Example Street 1"
        };

        var results = new List<ValidationResult>();
        var context = new ValidationContext(dto);
        var isValid = Validator.TryValidateObject(dto, context, results, true);

        Assert.False(isValid);
        Assert.Contains(results, result => result.ErrorMessage == "Company Name is required.");
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
}
