using CloudPhotoStorage.API.DTOs;
using CloudPhotoStorage.API.Services;
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

        public ImagesController(
            ILogger<ImagesController> logger,
            ImageRepo imageRepo,
            UserRepo userRepo,
            CategoryRepo categoryRepo)
        {
            _logger = logger;
            _imageRepo = imageRepo;
            _userRepo = userRepo;
            _categoryRepo = categoryRepo;
        }


        /// <summary>
        /// Получить изображение по имени
        /// </summary>
        [HttpPost]
        [Route("api/image/get")]
        public async Task<ActionResult> GetImageByName(CancellationToken cancellationToken)
        {
            try
            {
                var getImageDTO = await HttpContext.Request.ReadFromJsonAsync<GetImageDTO>();
                var user = await _userRepo.GetUserByLogin(getImageDTO.Login, cancellationToken);

                if (user == null)
                {
                    return NotFound("Пользователь не найден");
                }

                // Проверка пароля
                var passwordHash = user.PasswordHash;
                var passwordSalt = user.PasswordSalt;

                if (!PasswordHasher.VerifyPasswordHash(getImageDTO.Password, passwordHash, passwordSalt))
                {
                    return Unauthorized("Неверный пароль");
                }

                var image = await _imageRepo.GetImageByName(getImageDTO.ImageName, user.Login, new CancellationToken());
                var category = await _categoryRepo.GetCategoryById(image.CategoryId, new CancellationToken());

                ImageDTO imageDTO = new ImageDTO
                {
                    Name = image.ImageName,
                    CategoryName = category.CategoryName,
                    UploadDate = image.UploadDate,
                    ImageData = image.ImageBytes
                };

                return Ok(imageDTO);
            }
            catch (Exception ex)
            {
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        /// <summary>
        /// Загрузить новое изображение
        /// </summary>
        [HttpPost]
        [Route("api/images/post")]
        public async Task<ActionResult> UploadImage(CancellationToken cancellationToken)
        {
            try
            {
                var form = await HttpContext.Request.ReadFormAsync();
                var userDto = new UserDTO
                {
                    Login = form["Login"],
                    Password = form["Password"]
                };

                var file = form.Files.GetFile("ImageData");
                byte[] imageBytes = [];
                var binaryReader = new BinaryReader(file.OpenReadStream());
                imageBytes = binaryReader.ReadBytes((int)file.Length);

                var imageDto = new ImageDTO
                {
                    CategoryName = form["CategoryName"],
                    ImageData = imageBytes,
                    Name = form["Name"],
                    UploadDate = DateTime.Parse(form["UploadDate"]).ToUniversalTime(),
                };

                if (imageDto?.ImageData == null || imageDto.ImageData.Length == 0)
                {
                    return BadRequest("Изображение обязательно");
                }

                var userId = await _userRepo.GetUserIdByLogin(userDto.Login, cancellationToken);
                var categoryId = await _categoryRepo.GetCategoryIdByName(imageDto.CategoryName, cancellationToken);

                var user = await _userRepo.GetUserById(userId, cancellationToken);
                var category = await _categoryRepo.GetCategoryById(categoryId, cancellationToken);

                if (user != null && category != null)
                {
                    // Проверка пароля
                    var passwordHash = user.PasswordHash;
                    var passwordSalt = user.PasswordSalt;

                    if (!PasswordHasher.VerifyPasswordHash(userDto.Password, passwordHash, passwordSalt))
                    {
                        return Unauthorized("Неверный пароль");
                    }

                    // Проверка уникальности имени изображения
                    var imagesList = await _imageRepo.GetImagesByUserLogin(user.Login, cancellationToken);
                    if (imagesList.Any(i => i.ImageName == imageDto.Name))
                    {
                        return BadRequest("Изображение с таким именем уже существует");
                    }

                    var image = new Image
                    {
                        ImageId = Guid.NewGuid(),
                        ImageName = imageDto.Name,
                        ImageBytes = imageDto.ImageData,
                        UploadDate = (DateTime)imageDto.UploadDate,
                        UserId = (Guid)userId,
                        CategoryId = (Guid)categoryId
                    };

                    await _imageRepo.AddImage(image, cancellationToken);
                    return Ok();
                }
                else return NotFound();
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
        [HttpPost]
        [Route("api/images/delete")]
        public async Task<ActionResult> DeleteImage(CancellationToken cancellationToken)
        {
            try
            {
                var getImageDto = await HttpContext.Request.ReadFromJsonAsync<GetImageDTO>();
                var user = await _userRepo.GetUserByLogin(getImageDto.Login, cancellationToken);

                if (user != null)
                {
                    // Проверка пароля
                    var passwordHash = user.PasswordHash;
                    var passwordSalt = user.PasswordSalt;

                    if (!PasswordHasher.VerifyPasswordHash(getImageDto.Password, passwordHash, passwordSalt))
                    {
                        return Unauthorized("Неверный пароль");
                    }

                    var image = await _imageRepo.GetImageByName(getImageDto.ImageName, user.Login, cancellationToken);

                    if (image == null)
                    {
                        return BadRequest("Изображение не найдено");
                    }

                    await _imageRepo.DeleteImage(image, cancellationToken);
                    return Ok();
                }
                else return NotFound();
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при удалении изображения");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }

        /// <summary>
        /// Получить имена изображений с категориями указанного пользователя
        /// </summary>
        [HttpPost]
        [Route("api/images/get/names-with-categories")]
        public async Task<ActionResult<List<ImageWithCategoryDTO>>> GetImageNamesWithCategories(CancellationToken cancellationToken)
        {
            try
            {
                var userDto = await HttpContext.Request.ReadFromJsonAsync<UserDTO>();
                Guid userId = (Guid)await _userRepo.GetUserIdByLogin(userDto.Login, cancellationToken);
                var user = await _userRepo.GetUserById(userId, cancellationToken);
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

                var imagesList = await _imageRepo.GetImagesByUserLogin(user.Login, cancellationToken);

                List<ImageWithCategoryDTO> imagesInfo = new();
                foreach (var image in imagesList)
                {
                    var category = await _categoryRepo.GetCategoryById(image.CategoryId, cancellationToken);
                    imagesInfo.Add(new ImageWithCategoryDTO
                    {
                        ImageName = image.ImageName,
                        Category = category.CategoryName,
                        UploadDate = image.UploadDate
                    });
                }

                return Ok(imagesInfo);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Ошибка при получении имен изображений с категориями");
                return StatusCode(500, "Внутренняя ошибка сервера");
            }
        }
    }
}
