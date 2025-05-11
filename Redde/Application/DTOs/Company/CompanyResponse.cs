namespace Redde.Application.DTOs.Company;

public class CompanyResponse
{
    public int Id { get; set; }
    public string RNC { get; set; } = null!;
    public string Name { get; set; } = null!;
    public string CommercialName { get; set; } = null!;
    public string Category { get; set; } = null!;
    public string PaymentScheme { get; set; } = null!;
    public string State { get; set; } = null!;
    public string EconomicActivity { get; set; } = null!;
    public string GovernmentBranch { get; set; } = null!;
}
