using CloudPhotoStorage.API.DTOs;
using CloudPhotoStorage.DataBase;
using CloudPhotoStorage.DataBase.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace CloudPhotoStorage.API.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ImagesController : ControllerBase
    {

        private readonly ApplicationContext _context;
        private readonly ILogger<ImagesController> _logger;
        private readonly string _storagePath = Path.Combine(Directory.GetCurrentDirectory(), "Uploads");

        public ImagesController(ApplicationContext context, ILogger<ImagesController> logger)
        {
            _context = context;
            _logger = logger;

            // Создаем папку для хранения, если не существует
            if (!Directory.Exists(_storagePath))
            {
                Directory.CreateDirectory(_storagePath);
            }

        }
        // GET /api/images/get/all
        // Возвращает список всех изображений с информацией о пользователях и категориях
        [HttpGet]
        [Route("api/images/get/all")]
        public async Task<ActionResult<IEnumerable<ImageDTO>>> GetImages(CancellationToken cancellationToken)
        {
            return await _context.Images
                .Join(_context.Users,
                    image => image.UserId,
                    user => user.UserId,
                    (image, user) => new { image, user })
                .Join(_context.Categories,
                    temp => temp.image.CategoryId,
                    category => category.CategoryId,
                    (temp, category) => new ImageDTO
                    {
                        FileName = temp.image.FileName,
                        UploadDate = temp.image.UploadDate,
                        UserLogin = temp.user.Login,
                        CategoryName = category.CategoryName
                    })
                .ToListAsync(cancellationToken);
        }
        // GET /api/images/get/{id}
        // Возвращает конкретное изображение по ID
        [HttpGet]
        [Route("api/images/get/{id:int}")]
        public async Task<IActionResult> GetImage(int id)
        {
            var image = await _context.Images
                .FirstOrDefaultAsync(i => i.ImageId == id);

            if (image == null || !System.IO.File.Exists(image.FilePath))
                return NotFound();

            return PhysicalFile(image.FilePath, "application/octet-stream", image.FileName);
        }

        // POST /api/images/add
        // Добавляет новое изображение
        [HttpPost]
        [Route("api/images/add")]
        public async Task<ActionResult<ImageDTO>> AddImage(
            IFormFile file,
            [FromForm] int userId,
            [FromForm] int categoryId)
        {
            // Валидация
            if (file == null || file.Length == 0)
                return BadRequest("File is required");

            // Проверка пользователя и категории
            var user = await _context.Users.FindAsync(userId);
            var category = await _context.Categories.FindAsync(categoryId);

            if (user == null || category == null)
                return BadRequest("Invalid user or category");

            try
            {
                // Генерация уникального имени файла
                var fileName = $"{Guid.NewGuid()}{Path.GetExtension(file.FileName)}";
                var filePath = Path.Combine(_storagePath, fileName);

                // Сохранение файла
                using (var stream = System.IO.File.Create(filePath))
                {
                    await file.CopyToAsync(stream);
                }

                // Создание записи в БД
                var image = new Image
                {
                    FileName = file.FileName,
                    FilePath = filePath,
                    UploadDate = DateTime.UtcNow,
                    UserId = userId,
                    CategoryId = categoryId
                };

                _context.Images.Add(image);
                await _context.SaveChangesAsync();

                // Логирование в историю
                _context.LoginHistories.Add(new LoginHistory
                {
                    UserId = userId,
                    LoginDate = DateTime.UtcNow
                });
                await _context.SaveChangesAsync();

                return CreatedAtAction(nameof(GetImage), new { id = image.ImageId }, new ImageDTO
                {
                    FileName = image.FileName,
                    UploadDate = image.UploadDate,
                    UserLogin = user.Login,
                    CategoryName = category.CategoryName
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error uploading image");
                return StatusCode(500, "Internal server error");
            }
        }

        // DELETE /api/images/delete/{id}
        // Удаляет изображение и перемещает его в корзину
        [HttpDelete]
        [Route("api/images/delete/{id:int}")]
        public async Task<ActionResult<WasteBasketDTO>> DeleteImage(int id, [FromQuery] int userId)
        {
            var image = await _context.Images.FindAsync(id);
            if (image == null)
                return NotFound();

            try
            {
                // Перемещение в корзину
                var wasteItem = new WasteBasket
                {
                    UserId = userId,
                    DeleteDate = DateTime.UtcNow
                };

                _context.WasteBaskets.Add(wasteItem);
                _context.Images.Remove(image);
                await _context.SaveChangesAsync();

                return new WasteBasketDTO
                {
                    FileName = image.FileName,
                    UserLogin = (await _context.Users.FindAsync(userId)).Login,
                    DeleteDate = wasteItem.DeleteDate
                };
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting image");
                return StatusCode(500, "Internal server error");
            }
        }
    }
}



