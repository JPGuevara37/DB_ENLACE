using Microsoft.AspNetCore.Mvc;
using webapi.Services;

[ApiController]
[Route("api/[controller]")]
public class ExampleController : ControllerBase
{
    private readonly IExampleService _exampleService;

    public ExampleController(IExampleService exampleService)
    {
        _exampleService = exampleService;
    }

    [HttpGet("successfulExample")]
    public IActionResult GetSuccessfulExample()
    {
        var response = _exampleService.GetSuccessfulExample();
        return Ok(response);
    }

    [HttpGet("errorExample")]
    public IActionResult GetErrorExample()
    {
        var response = _exampleService.GetErrorExample();
        return BadRequest(response);
    }
}