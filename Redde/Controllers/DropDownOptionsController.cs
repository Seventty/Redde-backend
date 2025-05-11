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
}
