using System.Diagnostics;
using Microsoft.AspNetCore.Mvc;
using Models;
using Services;

namespace broker.Controllers;

[Route("In")]
internal class InController(MessageService messageService) : Controller
{
   
    [HttpPost]
    public async Task<IActionResult> PostMsg(Message msg)
    {
        var isOk = await messageService.Add(msg);
        if (isOk)
            return Ok();
        return BadRequest();
    }
}
