using CloudPhotoStorage.API.DTOs;
using CloudPhotoStorage.API.Services;
using CloudPhotoStorage.DataBase;
using CloudPhotoStorage.DataBase.Models;
using CloudPhotoStorage.DataBase.Repositories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CloudPhotoStorage.API.Controllers
{
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly ApplicationContext _context;
        private UserRepo _userRepo;
        private readonly ILogger<AccountController> _logger;

        public AccountController(ApplicationContext context, UserRepo userRepo, ILogger<AccountController> logger)
        {
            _context = context;
            _logger = logger;
            _userRepo = userRepo;
        }

        // POST api/account/registration
        // Регистрация нового пользователя
        [HttpPost]
        [Route("api/account/registration")]
        public async Task<IActionResult> RegisterAsync(CancellationToken cancellationToken)
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
                byte[] passwordHash = [];
                byte[] passwordSalt = [];
                PasswordHasher.CreatePasswordHash(userDto.Password, out passwordHash, out passwordSalt);

                var user = new User
                {
                    Login = userDto.Login,
                    PasswordHash = passwordHash,
                    PasswordSalt = passwordSalt,
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
        [HttpPost]
        [Route("api/account/login")]
        public async Task<IActionResult> Login(CancellationToken cancellationToken)
        {
            try
            {
                var userDto = await HttpContext.Request.ReadFromJsonAsync<UserDTO>();

                var user = await _userRepo.GetUserByLogin(userDto.Login, new CancellationToken());
                if (user != null)
                {
                    // Проверка пароля

                    var passwordHash = user.PasswordHash;
                    var passwordSalt = user.PasswordSalt;

                    if (!PasswordHasher.VerifyPasswordHash(userDto.Password, passwordHash, passwordSalt))
                    {
                        return Unauthorized("Неверный пароль");
                    }
                    else
                    {
                        // Логирование в историю посещений
                        _context.LoginHistories.Add(new LoginHistory
                        {
                            UserId = user.UserId,
                            LoginDate = DateTime.UtcNow
                        });

                        return Ok(new { Message = "Аутентификация успешна" });
                    }
                }
                else
                {
                    string message = $"Пользователь с именем \"{userDto.Login}\" не существует.";
                    return Unauthorized(message);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при аутентификации пользователя");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }
    }
}