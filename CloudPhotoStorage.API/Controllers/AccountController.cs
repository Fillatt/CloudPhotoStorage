using CloudPhotoStorage.DataBase;
using CloudPhotoStorage.DataBase.Models;
using CloudPhotoStorage.API.DTOs;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace CloudPhotoStorage.API.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private readonly ILogger<AccountController> _logger;

        public AccountController(ApplicationContext context, ILogger<AccountController> logger)
        {
            _context = context;
            _logger = logger;
        }

        // POST api/account/registration
        // Регистрация нового пользователя
        [HttpPost]
        [Route("api/account/registration")]
        public async Task<IActionResult> RegisterAsync()
        {
            try
            {
                var userDto = await HttpContext.Request.ReadFromJsonAsync<UserDTO>();

                // Проверка на существующего пользователя
                if (await _context.Users.AnyAsync(u => u.Login == userDto.Login))
                {
                    return Conflict("Пользователь с таким логином уже существует");
                }

                // Генерация соли и хеша пароля
                var salt = GenerateSalt();
                var passwordHash = HashPassword(userDto.Password, salt);

                var user = new User
                {
                    Login = userDto.Login,
                    PasswordHash = passwordHash,
                    PasswordSalt = Convert.ToBase64String(salt),
                    //RoleID = userDto.RoleID 
                };

                _context.Users.Add(user);
                await _context.SaveChangesAsync();

                // Логирование в историю посещений
                _context.LoginHistories.Add(new LoginHistory
                {
                    UserId = user.UserId,
                    LoginDate = DateTime.UtcNow
                });
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Пользователь успешно зарегистрирован" });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при регистрации пользователя");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        // GET api/account/login
        // Аутентификация пользователя
        [HttpGet]
        [Route("api/account/login")]
        public async Task<IActionResult> Login([FromQuery] string login, [FromQuery] string password)
        {
            try
            {
                var user = await _context.Users.FirstOrDefaultAsync(u => u.Login == login);
                if (user == null)
                {
                    return NotFound("Пользователь не найден");
                }

                // Проверка пароля
                var salt = Convert.FromBase64String(user.PasswordSalt);
                var inputHash = HashPassword(password, salt);

                if (inputHash != user.PasswordHash)
                {
                    return Unauthorized("Неверный пароль");
                }

                // Логирование в историю посещений
                _context.LoginHistories.Add(new LoginHistory
                {
                    UserId = user.UserId,
                    LoginDate = DateTime.UtcNow
                });
                await _context.SaveChangesAsync();

                return Ok(new { Message = "Аутентификация успешна", RoleID = user.RoleID });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при аутентификации пользователя");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        private static byte[] GenerateSalt()
        {
            byte[] salt = new byte[16];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(salt);
            }
            return salt;
        }

        private static string HashPassword(string password, byte[] salt)
        {
            using (var sha256 = SHA256.Create())
            {
                var saltedPassword = Encoding.UTF8.GetBytes(password).Concat(salt).ToArray();
                var hash = sha256.ComputeHash(saltedPassword);
                return Convert.ToBase64String(hash);
            }
        }
    }
}