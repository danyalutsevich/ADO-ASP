using Microsoft.AspNetCore.Mvc;

namespace ASP.Controllers;

[Route("api/[controller]")]
[ApiController]
public class RatesController : ControllerBase
{
    [HttpGet]
    public object Get()
    {
        return new { result = "GET" };
    }

    [HttpPost]
    public object Post()
    {
        return new { result = "POST" };
    }
    
    [HttpPut]
    public object Put()
    {
        return new { result = "PUT" };
    }
    
    public object Default()
    {
        return new { result = "Default" };
    }
}