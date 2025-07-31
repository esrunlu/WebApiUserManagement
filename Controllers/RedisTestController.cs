using Microsoft.AspNetCore.Mvc;
using WebApplication1.Services;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class RedisTestController : ControllerBase
    {
        private readonly RedisService _redisService;

        public RedisTestController(RedisService redisService)
        {
            _redisService = redisService;
        }

        [HttpPost("set")]
        public async Task<IActionResult> SetValue(string key, string value)
        {
            await _redisService.SetStringAsync(key, value);
            return Ok("Set successful");
        }

        [HttpGet("get")]
        public async Task<IActionResult> GetValue(string key)
        {
            var value = await _redisService.GetStringAsync(key);
            return Ok(value);
        }
    }
}

