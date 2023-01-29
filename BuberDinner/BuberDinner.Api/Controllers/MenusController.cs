using BuberDinner.Contracts.Menus;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace BuberDinner.Api.Controllers;

[Route("hosts/{hostId}/menus")]
public class MenusController : ApiController
{
    [HttpPost]
    public IActionResult CreateMenu(CreateMenuRequest request, string hostId)
    {
        return Ok(request);
    }
}
