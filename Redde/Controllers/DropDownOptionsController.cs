using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Redde.Application.DTOs.DropDownOptions;
using Redde.Infraestructure.Persistence;

[ApiController]
[Route("api/[controller]")]
public class DropDownOptionsController : ControllerBase
{
    private readonly ApplicationDbContext _context;

    public DropDownOptionsController(ApplicationDbContext context)
    {
        _context = context;
    }

    [HttpGet("company-categories")]
    public async Task<ActionResult<IEnumerable<DropDownOptionsDTO>>> GetCompanyCategories()
    {
        return await _context.CompanyCategories
            .Select(c => new DropDownOptionsDTO { Id = c.Id, Name = c.Name })
            .ToListAsync();
    }

    [HttpGet("payment-schemes")]
    public async Task<ActionResult<IEnumerable<DropDownOptionsDTO>>> GetPaymentSchemes()
    {
        return await _context.PaymentSchemes
            .Select(p => new DropDownOptionsDTO { Id = p.Id, Name = p.Name })
            .ToListAsync();
    }

    [HttpGet("company-states")]
    public async Task<ActionResult<IEnumerable<DropDownOptionsDTO>>> GetCompanyStates()
    {
        return await _context.CompanyStates
            .Select(s => new DropDownOptionsDTO { Id = s.Id, Name = s.Name })
            .ToListAsync();
    }

    [HttpGet("economic-activities")]
    public async Task<ActionResult<IEnumerable<DropDownOptionsDTO>>> GetEconomicActivities()
    {
        return await _context.EconomicActivities
            .Select(e => new DropDownOptionsDTO { Id = e.Id, Name = e.Name })
            .ToListAsync();
    }

    [HttpGet("government-branches")]
    public async Task<ActionResult<IEnumerable<DropDownOptionsDTO>>> GetGovernmentBranches()
    {
        return await _context.GovernmentBranches
            .Select(g => new DropDownOptionsDTO { Id = g.Id, Name = g.Name })
            .ToListAsync();
    }
}
