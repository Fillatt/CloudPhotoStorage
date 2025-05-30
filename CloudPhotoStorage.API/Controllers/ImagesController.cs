using CloudPhotoStorage.API.DTOs;
using CloudPhotoStorage.DataBase.Models;
using CloudPhotoStorage.DataBase.Repositories;
using Microsoft.AspNetCore.Mvc;
using CloudPhotoStorage.API.Services;
using System.Text;
using System.Text.Json.Serialization.Metadata;
using System.Xml.Linq;
using System.IO;
using System;

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
        public async Task<ActionResult> GetImagesByUser(CancellationToken cancellationToken)
        {
            try
            {
                var userDto = await HttpContext.Request.ReadFromJsonAsync<UserDTO>();
                var user = await _userRepo.GetUserByLogin(userDto.Login, cancellationToken);

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

                var images = await _imageRepo.GetImagesByUserId(user.UserId, cancellationToken);

                if (images == null || !images.Any())
                {
                    return NotFound("Изображения не найдены");
                }

                var result = new List<ImageDTO>();
                foreach (var image in images)
                {
                    var category = await _categoryRepo.GetCategoryById(image.CategoryId, cancellationToken);

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

                var image = await _imageRepo.GetImageByName(getImageDTO.ImageName, new CancellationToken());
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
                    var imagesList = await _imageRepo.GetByUserIdAsync(user.UserId, cancellationToken);
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

                    await _loginHistoryRepo.AddLoginHistoryAsync(new LoginHistory
                    {
                        LoginId = Guid.NewGuid(),
                        UserId = (Guid)userId,
                        LoginDate = DateTime.UtcNow
                    }, cancellationToken);

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
        //[Route("api/images/delete/{id}")]
        //public async task<iactionresult> deleteimage(guid id, [fromquery] guid userid, cancellationtoken cancellationtoken)
        //{
        //    try
        //    {
        //        var image = await _imagerepo.getimagebyidasync(id, cancellationtoken);
        //        if (image == null)
        //        {
        //            return notfound("изображение не найдено");
        //        }

        //        var user = await _userrepo.getuserbyidasync(userid, cancellationtoken);
        //        if (user == null)
        //        {
        //            return notfound("пользователь не найден");
        //        }

        //        await _wastebasketrepo.addasync(new wastebasket
        //        {
        //            wastebasketid = guid.newguid(),
        //            imageid = image.imageid,
        //            userid = userid,
        //            deletedate = datetime.utcnow
        //        }, cancellationtoken);

        //        await _imagerepo.deleteasync(image, cancellationtoken);

        //        return nocontent();
        //    }
        //    catch (exception ex)
        //    {
        //        _logger.logerror(ex, $"ошибка при удалении изображения с id {id}");
        //        return statuscode(500, "внутренняя ошибка сервера");
        //    }
        //}

        /// <summary>
        /// Получить имена изображений с категориями указанного пользователя
        /// </summary>
        [HttpPost]
        [Route("api/images/get/names-with-categories")]
        public async Task<ActionResult<List<ImageWithCategoryAndUploadDateDto>>> GetImageNamesWithCategoriesAndDate(CancellationToken cancellationToken)
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

                var imagesList = await _imageRepo.GetByUserIdAsync(user.UserId, cancellationToken);

                var imageDtos = await Task.WhenAll(imagesList.Select(async x =>
                new ImageWithCategoryAndUploadDateDto
                {
                    ImageName = x.ImageName,
                    Category = await _categoryRepo.GetCategoryNameByIdAsync(x.CategoryId, cancellationToken),
                    UploadDate = x.UploadDate
                }));

                var result = imageDtos.ToList();
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