using CloudPhotoStorage.API.DTOs;
using CloudPhotoStorage.DataBase.Models;
using CloudPhotoStorage.DataBase.Repositories;
using Microsoft.AspNetCore.Mvc;
using CloudPhotoStorage.API.Services;

namespace CloudPhotoStorage.API.Controllers
{

    [ApiController]
    public class ImagesController : ControllerBase
    {
        private readonly ILogger<ImagesController> _logger;
        private readonly ImageRepo _imageRepo;
        private readonly UserRepo _userRepo;
        private readonly CategoryRepo _categoryRepo;
        private readonly WasteBasketRepo _wasteBasketRepo;
        private readonly LoginHistoryRepo _loginHistoryRepo;

        public ImagesController(
            ILogger<ImagesController> logger,
            ImageRepo imageRepo,
            UserRepo userRepo,
            CategoryRepo categoryRepo,
            WasteBasketRepo wasteBasketRepo,
            LoginHistoryRepo loginHistoryRepo)
        {
            _logger = logger;
            _imageRepo = imageRepo;
            _userRepo = userRepo;
            _categoryRepo = categoryRepo;
            _wasteBasketRepo = wasteBasketRepo;
            _loginHistoryRepo = loginHistoryRepo;
        }

        /// <summary>
        /// Получить все изображения конкретного пользователя
        /// </summary>
        [HttpPost]
        [Route("api/images/get-by-user")]
        public async Task<ActionResult<List<ImageDTO>>> GetImagesByUser(CancellationToken cancellationToken)
        {
            try
            {
                var userDto = await HttpContext.Request.ReadFromJsonAsync<UserDTO>();
                var user = await _userRepo.GetUserByLoginAsync(userDto.Login, cancellationToken);

                if (user == null)
                {
                    return NotFound("Пользователь не найден");
                }

                // Проверка пароля
                var passwordHash = user.PasswordHash;
                var passwordSalt = user.PasswordSalt;

                if (!PasswordHasher.VerifyPasswordHash(userDto.Password, passwordHash, passwordSalt))
                {
                    return Unauthorized("Неверный пароль");
                }

                var images = await _imageRepo.GetByUserIdAsync(user.UserId, cancellationToken);

                if (images == null || !images.Any())
                {
                    return NotFound("Изображения не найдены");
                }

                var result = new List<ImageDTO>();
                foreach (var image in images)
                {
                    var category = await _categoryRepo.GetByIdAsync(image.CategoryId, cancellationToken);

                    result.Add(new ImageDTO
                    {
                        ImageData = image.ImageBytes,
                        Name = image.ImageName,
                        UploadDate = image.UploadDate,
                        CategoryName = category?.CategoryName ?? "Без категории"
                    });
                }
                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении изображений пользователя");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        /// <summary>
        /// Получить изображение по ID
        /// </summary>
        [HttpGet]
        [Route("api/images/get/{id}")]
        public async Task<IActionResult> GetImageById(Guid id, CancellationToken cancellationToken)
        {
            try
            {
                var image = await _imageRepo.GetByIdAsync(id, cancellationToken);
                if (image == null)
                {
                    return NotFound();
                }

                return File(image.ImageBytes, "application/octet-stream", image.ImageName);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при получении изображения с ID {id}");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        /// <summary>
        /// Загрузить новое изображение
        /// </summary>
        [HttpPost]
        [Route("api/images/post")]
        public async Task<ActionResult<ImageDTO>> UploadImage(CancellationToken cancellationToken)
        {
            try
            {
                var userDto = await HttpContext.Request.ReadFromJsonAsync<UserDTO>();
                var imageDto = await HttpContext.Request.ReadFromJsonAsync<ImageDTO>();
                if (imageDto?.ImageData == null || imageDto.ImageData.Length == 0)
                {
                    return BadRequest("Изображение обязательно");
                }

                Guid userId = (Guid)await _userRepo.GetIdByLoginAsync(userDto.Login, cancellationToken);
                Guid categoryId = (Guid)await _categoryRepo.GetIdByNameAsync(imageDto.CategoryName, cancellationToken);

                var user = await _userRepo.GetByIdAsync(userId, cancellationToken);
                var category = await _categoryRepo.GetByIdAsync(categoryId, cancellationToken);
                
                if (user == null)
                {
                    return NotFound("Пользователь не найден");
                }

                if (category == null)
                {
                    return NotFound("Категория не найдена");
                }
                
                // Проверка пароля
                var passwordHash = user.PasswordHash;
                var passwordSalt = user.PasswordSalt;

                if (!PasswordHasher.VerifyPasswordHash(userDto.Password, passwordHash, passwordSalt))
                {
                    return Unauthorized("Неверный пароль");
                }

                var image = new Image
                {
                    ImageId = Guid.NewGuid(),
                    ImageName = imageDto.Name,
                    ImageBytes = imageDto.ImageData,
                    UploadDate = imageDto.UploadDate ?? DateTime.UtcNow,
                    UserId = userId,
                    CategoryId = categoryId
                };

                await _imageRepo.AddAsync(image, cancellationToken);

                await _loginHistoryRepo.AddAsync(new LoginHistory
                {
                    LoginId = Guid.NewGuid(),
                    UserId = userId,
                    LoginDate = DateTime.UtcNow
                }, cancellationToken);

                return Ok();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при загрузке изображения");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        /// <summary>
        /// Удалить изображение
        /// </summary>
        [Route("api/images/delete/{id}")]
        public async Task<IActionResult> DeleteImage(Guid id, [FromQuery] Guid userId, CancellationToken cancellationToken)
        {
            try
            {
                var image = await _imageRepo.GetByIdAsync(id, cancellationToken);
                if (image == null)
                {
                    return NotFound("Изображение не найдено");
                }

                var user = await _userRepo.GetByIdAsync(userId, cancellationToken);
                if (user == null)
                {
                    return NotFound("Пользователь не найден");
                }

                await _wasteBasketRepo.AddAsync(new WasteBasket
                {
                    WasteBasketId = Guid.NewGuid(),
                    ImageId = image.ImageId,
                    UserId = userId,
                    DeleteDate = DateTime.UtcNow
                }, cancellationToken);

                await _imageRepo.DeleteAsync(image, cancellationToken);

                return NoContent();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Ошибка при удалении изображения с ID {id}");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        /// <summary>
        /// Получить имена изображений с категориями указанного пользователя
        /// </summary>
        [HttpPost]
        [Route("api/images/get/names-with-categories")]
        public async Task<ActionResult<List<ImageWithCategoryDto>>> GetImageNamesWithCategories(CancellationToken cancellationToken)
        {
            try
            {
                var userDto = await HttpContext.Request.ReadFromJsonAsync<UserDTO>(); 
                Guid userId = (Guid)await _userRepo.GetIdByLoginAsync(userDto.Login, cancellationToken);
                var user = await _userRepo.GetByIdAsync(userId, cancellationToken);
                if (user == null)
                {
                    return NotFound("Пользователь не найден");
                }
                // Проверка пароля
                var passwordHash = user.PasswordHash;
                var passwordSalt = user.PasswordSalt;

                if (!PasswordHasher.VerifyPasswordHash(userDto.Password, passwordHash, passwordSalt))
                {
                    return Unauthorized("Неверный пароль");
                }

                var imagesDict = await _imageRepo.GetUserImagesWithCategoriesAsync(user.Login, cancellationToken);

                var result = imagesDict
                    .Select(x => new ImageWithCategoryDto
                    {
                        ImageName = x.Key,
                        Category = x.Value
                    })
                    .ToList();

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении имен изображений с категориями");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }
    }
}