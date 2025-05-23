using CloudPhotoStorage.API.DTOs;
using CloudPhotoStorage.DataBase.Models;
using CloudPhotoStorage.DataBase.Repositories;
using Microsoft.AspNetCore.Mvc;

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
        /// Получить все изображения
        /// </summary>
        [HttpGet]
        [Route("api/images/get")]
        public async Task<ActionResult<IEnumerable<ImageDTO>>> GetAllImages(CancellationToken cancellationToken)
        {
            try
            {
                var images = await _imageRepo.GetAllAsync(cancellationToken);

                if (images == null || !images.Any())
                {
                    return NotFound("Изображения не найдены");
                }

                var result = new List<ImageDTO>();
                foreach (var image in images)
                {
                    var user = await _userRepo.GetByIdAsync(image.UserId, cancellationToken);
                    var category = await _categoryRepo.GetByIdAsync(image.CategoryId, cancellationToken);

                    result.Add(new ImageDTO
                    {
                        ImagePath = image.ImageBytes,
                        Name = image.ImageName,
                        UploadDate = image.UploadDate,
                        UserLogin = user?.Login ?? "Неизвестно",
                        CategoryName = category?.CategoryName ?? "Без категории"
                    });
                }

                return Ok(result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении изображений");
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
        public async Task<ActionResult<ImageDTO>> UploadImage([FromBody] ImageDTO imageDto, CancellationToken cancellationToken)
        {
            try
            {
                if (imageDto.ImagePath == null || imageDto.ImagePath.Length == 0)
                {
                    return BadRequest("Изображение обязательно");
                }

                Guid userId = Guid.NewGuid(); // Заменить на реальный ID пользователя
                Guid categoryId = Guid.NewGuid(); // Заменить на реальный ID категории

                var user = await _userRepo.GetByIdAsync(userId, cancellationToken);
                var category = await _categoryRepo.GetByIdAsync(categoryId, cancellationToken);

                if (user == null || category == null)
                {
                    return BadRequest("Неверный пользователь или категория");
                }

                var image = new Image
                {
                    ImageId = Guid.NewGuid(),
                    ImageName = imageDto.Name,
                    ImageBytes = imageDto.ImagePath,
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

                return CreatedAtAction(nameof(GetImageById), new { id = image.ImageId }, new ImageDTO
                {
                    ImagePath = image.ImageBytes,
                    Name = image.ImageName,
                    UploadDate = image.UploadDate,
                    UserLogin = user.Login,
                    CategoryName = category.CategoryName
                });
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
                    return NotFound();
                }

                var user = await _userRepo.GetByIdAsync(userId, cancellationToken);
                if (user == null)
                {
                    return BadRequest("Неверный пользователь");
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
        public async Task<ActionResult<Dictionary<string, string>>> GetImageNamesWithCategories(CancellationToken cancellationToken)
        {
            try
            {
                var userName = await new StreamReader(HttpContext.Response.Body).ReadToEndAsync();
                var result = await _imageRepo.GetUserImagesWithCategoriesAsync(userName, cancellationToken);
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