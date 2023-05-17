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
        return new { result = HttpContext.Request.Method };
    }

    [HttpPost("rate")]
    public string Rate([FromBody] RequestData data)
    {
        Console.WriteLine(data.Value);
        Console.WriteLine(data.ItemId);
        Console.WriteLine(data.UserId);
        return "Rate";
    }

    public class RequestData
    {
        public string ItemId { get; set; }
        public string Value { get; set; }
        public string UserId { get; set; }
    }
}