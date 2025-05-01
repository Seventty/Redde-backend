using Redde.Domain.Entities;
using Redde.Domain.Entities.CompanyEntities;

public class Company
{
    public int Id { get; set; }
    public int RNC { get; set; }
    public string Name { get; set; } = null!;
    public string CommercialName { get; set; } = null!;
    public string Phone { get; set; } = null!;

    public int OwnerId { get; set; }
    public User Owner { get; set; } = null!;

    public int CategoryId { get; set; }
    public CompanyCategory Category { get; set; } = null!;

    public int PaymentSchemeId { get; set; }
    public PaymentScheme PaymentScheme { get; set; } = null!;

    public int StateId { get; set; }
    public CompanyState State { get; set; } = null!;

    public int EconomicActivityId { get; set; }
    public EconomicActivity EconomicActivity { get; set; } = null!;

    public int GovernmentBranchId { get; set; }
    public GovernmentBranch GovernmentBranch { get; set; } = null!;
}
