using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using WebApplication1.Data;
using WebApplication1.Dtos;
using WebApplication1.Interfaces;
using WebApplication1.Models;
using UsersCreateDto = WebApplication1.Dtos.UsersCreateDto;

namespace WebApplication1.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly AppDbContext _context;

        public UsersController(IUserService userService)
        {
            _userService = userService;
        }
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            bool success = await _userService.Login(loginDto.Email, loginDto.Password);

            if (!success)
            {
                return Unauthorized("E-posta veya şifre hatalı.");
            }

            return Ok("Giriş başarılı.");
        }



        // GET: api/Users/5
        [HttpGet("{id}")]
        public async Task<ActionResult<Users>> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);

            if (user == null)
                return NotFound();

            return Ok(user);
        }

        // POST: api/Users
        [HttpPost]
        public async Task<ActionResult<Users>> PostUsers([FromBody] UsersCreateDto userDto)
        {
            var user = new Users
            {
                Name = userDto.Name,
                Username = userDto.Username,
                Email = userDto.Email,
                Password = (string)_userService.HashPassword(userDto.Password) // Şifre hashleniyor
            };

            await _userService.AddNewUser(user);

            return Ok(user);//CreatedAtAction(nameof(GetUsers), new { id = user.Id }, user);
        }

        private object GetUsers()
        {
            throw new NotImplementedException();
        }

        // PUT: api/Users/5
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, Users updatedUser)
        {
            if (id != updatedUser.Id)
                return BadRequest("ID uyuşmuyor.");

            var result = await _userService.UpdateUser(updatedUser);

            if (!result)
                return NotFound();

            return NoContent();
        }

        // DELETE: api/Users/5
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deleted = await _userService.DeleteUserById(id);

            if (!deleted)
                return NotFound();

            return NoContent();
        }


    }
}

