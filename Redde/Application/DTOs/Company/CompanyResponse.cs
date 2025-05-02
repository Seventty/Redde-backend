namespace Redde.Application.DTOs.Company;

public class CompanyResponse
{
    public int Id { get; set; }
    public int RNC { get; set; }
    public string Name { get; set; } = null!;
    public string CommercialName { get; set; } = null!;

    public CatalogResponse Category { get; set; } = null!;
    public CatalogResponse PaymentScheme { get; set; } = null!;
    public CatalogResponse State { get; set; } = null!;
    public CatalogResponse EconomicActivity { get; set; } = null!;
    public CatalogResponse GovernmentBranch { get; set; } = null!;
}
