using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using PermissionBasedAuth.Constants;

namespace PermissionBasedAuth.Controllers
{
    [Route("api/[controller]")]
    [Authorize]
    [ApiController]
    public class OrdersController : ControllerBase
    {
        public OrdersController() { }

        [Authorize(Permission.Order.Index)]
        [HttpGet] public IActionResult Get()
        {
            return Ok();
        }
        [Authorize(Permission.Order.Create)]
        [HttpPost]
        public IActionResult Post()
        {
            return Ok();
        }
        [Authorize(Permission.Order.Edit)]
        [HttpPut]
        public IActionResult Put()
        {
            return Ok();
        }

        [Authorize(Permission.Order.Delete)]
        [HttpDelete]
        public IActionResult Delete()
        {
            return Ok();
        }
    }
}
