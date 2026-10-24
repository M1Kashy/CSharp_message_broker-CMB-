using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace broker.Controllers;

[Route("In")]
public class InController(MessageService messageService, ILogger<InController> logger) : Controller
{
   
    [HttpPost]
    public async Task<IActionResult> PostMsg([FromBody]Message msg)
    {
        logger.LogInformation($" \t Get msg: {msg.id} - {msg.msg}");
        var isOk = await messageService.Add(msg);
        if (isOk)
            return Accepted();
        return BadRequest();
    }
}
