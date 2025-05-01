using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Redde.Application.DTOs.Company;
using Redde.Application.Interfaces;
using System.Security.Claims;

namespace Redde.Controllers;

[ApiController]
[Route("api/[controller]")]
public class CompanyController(ICompanyService companyService) : ControllerBase
{
    private readonly ICompanyService _companyService = companyService;

    [HttpPost]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> Create([FromBody] CompanyRequest request)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _companyService.CreateAsync(userId, request);
        return Ok(new { message = "Empresa registrada exitosamente" });
    }

    [HttpGet]
    [Authorize(Roles = "Admin,Owner")]
    public async Task<IActionResult> GetAll()
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var role = User.FindFirstValue(ClaimTypes.Role);
        var companies = await _companyService.GetAllAsync(userId, role!);
        return Ok(companies);
    }

    [HttpGet("{id}")]
    [Authorize(Roles = "Admin,Owner")]
    public async Task<IActionResult> GetById(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        var role = User.FindFirstValue(ClaimTypes.Role);
        var company = await _companyService.GetByIdAsync(id, userId, role!);
        return Ok(company);
    }

    [HttpPut("{id}")]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> Update(int id, [FromBody] CompanyRequest request)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _companyService.UpdateAsync(id, userId, request);
        return Ok(new { message = "Empresa actualizada correctamente" });
    }

    [HttpDelete("{id}")]
    [Authorize(Roles = "Owner")]
    public async Task<IActionResult> Delete(int id)
    {
        var userId = int.Parse(User.FindFirstValue(ClaimTypes.NameIdentifier)!);
        await _companyService.DeleteAsync(id, userId);
        return Ok(new { message = "Empresa eliminada correctamente" });
    }
}
