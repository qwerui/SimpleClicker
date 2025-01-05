using ClickerBackend.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Serilog;

namespace ClickerBackend.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    [Authorize]
    public class UserController : ControllerBase
    {
        IUserService _userService;

        public UserController(IUserService userService) 
        {
            _userService = userService;
        }

        [HttpGet]
        [Route("connect")]
        public async Task<IActionResult> Get([FromQuery] string userId)
        {
            Log.Information("Connect : " + userId);

            DateTimeOffset? result;
            try
            {
                result = await _userService.FindLastConnectById(userId);
            }
            catch
            {
                return BadRequest();
            }

            if(result == null)
            {
                return NotFound();
            }

            return Ok(result);
        }

        [HttpPut]
        [Route("connect/{userId}")]
        public async Task<IActionResult> Put([FromRoute] string userId)
        {
            try
            {
                await _userService.UpdateConnect(userId);
            }
            catch
            {
                return BadRequest();
            }

            Log.Information($"{userId} connected");

            return Ok();
        }

        [HttpGet]
        public async Task<IActionResult> Check([FromQuery] string userId)
        {
            var count = await _userService.CheckUserExists(userId);

            if(count <= 0)
                return BadRequest();
            return Ok();
        }

        [HttpPost]
        public async Task<IActionResult> Post([FromForm] string userId)
        {
            await _userService.CreateUser(userId);

            Log.Information($"New user {userId} created");

            return Ok();
        }
    }
}
