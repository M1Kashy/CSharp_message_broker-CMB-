using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace broker.Controllers;

[Route("Out")]
public class OutController(MessageService messageService) : Controller
{
   
    [HttpGet("{id:Guid}")]
    public async Task<IActionResult> GetMsg(Guid id)
    {
        var msg = await messageService.Get(id);
        if (msg is not null)
            return Content(msg);
        return BadRequest("Queue isn't exist");
    }
}
