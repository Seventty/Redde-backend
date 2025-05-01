namespace Redde.Application.DTOs.Company;

public class CompanyRequest
{
    public int RNC { get; set; }
    public string Name { get; set; } = null!;
    public string CommercialName { get; set; } = null!;

    public int CategoryId { get; set; }
    public int PaymentSchemeId { get; set; }
    public int StateId { get; set; }
    public int EconomicActivityId { get; set; }
    public int GovernmentBranchId { get; set; }
}
