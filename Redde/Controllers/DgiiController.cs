using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class DgiiController(DgiiScraperService scraper) : ControllerBase
{
    private readonly DgiiScraperService _scraper = scraper;

    [HttpGet("{rnc}")]
    public async Task<IActionResult> GetDatos(string rnc)
    {
        var result = await _scraper.ConsultarRnc(rnc);
        return Ok(result);
    }
}
