using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class ExampleController : ControllerBase
    {
        [HttpGet("sync")]
        public IActionResult GetSync()
        {
            Thread.Sleep(3000); // 3 saniye bekler
            return Ok("Bu senkron bir API yanıtıdır. (3 sn bekledi)");
        }

        [HttpGet("async")]
        public async Task<IActionResult> GetAsync()
        {
            await Task.Delay(3000); // 3 saniye bekler ama thread boşta kalır
            return Ok("Bu asenkron bir API yanıtıdır. (3 sn bekledi ama thread boştu)");
        }
    }
}
