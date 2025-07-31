using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;
using WebApplication1.Dtos;
using WebApplication1.Interfaces;
using WebApplication1.Models;

namespace WebApplication1.Controllers
{
    [Authorize]
    [ApiController]
    [Route("api/[controller]")]
    public class UsersController : ControllerBase
    {
        private readonly IUserService _userService;
        private readonly ILogger<UsersController> _logger;

        public UsersController(IUserService userService, ILogger<UsersController> logger)
        {
            _userService = userService;
            _logger = logger;
        }

        // POST: api/Users/login
        [AllowAnonymous]
        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] LoginDto loginDto)
        {
            var user = await _userService.Login(loginDto);
            if (user == null)
                return Unauthorized("E-posta veya şifre hatalı");

            return Ok("Giriş başarılı.");
        }

        // GET: api/Users
        [HttpGet]
        public async Task<IActionResult> GetAllUsers()
        {
            _logger.LogInformation("🟢 GetAllUsers endpoint called.");
            var users = await _userService.GetAllUsers();
            return Ok(users);
        }

        // GET: api/Users/orderbydate
        [HttpGet("orderbydate")]
        public async Task<IActionResult> GetAllUsersOrderByDate()
        {
            var users = await _userService.GetAllUsersOrderByDate();
            return Ok(users);
        }

        // GET: api/Users/{id}
        [HttpGet("{id}")]
        public async Task<IActionResult> GetUserById(int id)
        {
            var user = await _userService.GetUserById(id);
            if (user == null)
                return NotFound("Kullanıcı bulunamadı.");

            return Ok(user);
        }

        // GET: api/Users/get-profile
        [Authorize(Roles = "User")]
        [HttpGet("get-profile")]
        public IActionResult GetProfile()
        {
            return Ok("Bu sadece User rolündekiler için");
        }

        // POST: api/Users
        [AllowAnonymous]
        [HttpPost]
        public async Task<IActionResult> PostUsers([FromBody] Dtos.UsersCreateDto userDto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);

            string passwordHash = BCrypt.Net.BCrypt.EnhancedHashPassword(userDto.PasswordHash, 13);

            var user = new Users
            {
                Name = userDto.Name,
                Username = userDto.Username,
                Email = userDto.Email,
                PasswordHash = passwordHash,
                CreatedDate = DateTime.UtcNow,
                IsActive = true
            };

            var addedUser = await _userService.AddNewUser(user);
            if (addedUser == null)
                return BadRequest("Kullanıcı eklenemedi.");

            var userRole = new UserRole
            {
                UserId = addedUser.Id,
                RoleId = 1 // Default Role ID
            };

            await _userService.AddUserRole(userRole);

            return Ok(addedUser);
        }

        // PUT: api/Users/{id}
        [HttpPut("{id}")]
        public async Task<IActionResult> PutUser(int id, [FromBody] Users updatedUser)
        {
            if (id != updatedUser.Id)
                return BadRequest("ID uyuşmuyor.");

            var result = await _userService.UpdateUser(updatedUser);
            if (!result)
                return NotFound("Kullanıcı bulunamadı veya güncellenemedi.");

            return NoContent();
        }

        // DELETE: api/Users/{id}
        [HttpDelete("{id}")]
        public async Task<IActionResult> DeleteUser(int id)
        {
            var deleted = await _userService.DeleteUserById(id);
            if (!deleted)
                return NotFound("Kullanıcı bulunamadı veya zaten silinmiş.");

            return NoContent();
        }

        // PUT: api/Users/softdelete/{id}
        [HttpPut("softdelete/{id}")]
        public async Task<IActionResult> SoftDeleteUser(int id)
        {
            var result = await _userService.SoftDeleteUserById(id);
            if (!result)
                return NotFound("Kullanıcı bulunamadı veya zaten pasif.");

            return Ok("Kullanıcı soft delete ile pasif hale getirildi.");
        }

        // GET: api/Users/withroles
        [HttpGet("withroles")]
        public async Task<IActionResult> GetUsersWithRoles()
        {
            var users = await _userService.GetUsersWithRolesFromSP();
            return Ok(users);
        }
    }
}
