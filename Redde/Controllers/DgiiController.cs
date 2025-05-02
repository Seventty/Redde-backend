using Microsoft.AspNetCore.Mvc;

[ApiController]
[Route("api/[controller]")]
public class DgiiController : ControllerBase
{
    private readonly DgiiService _dgiiService;

    public DgiiController(DgiiService dgiiService)
    {
        _dgiiService = dgiiService;
    }

    [HttpGet("{rnc}")]
    public async Task<IActionResult> Get(string rnc)
    {
        var result = await _dgiiService.ConsultarRNC(rnc);
        return Ok(result);
    }
}
