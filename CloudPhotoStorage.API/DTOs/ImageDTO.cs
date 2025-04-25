namespace CloudPhotoStorage.API.DTOs
{
    public class ImageDTO
    {
        public string FileName { get; set; }
        public DateTime? UploadDate { get; set; }
        public string UserLogin { get; set; }
        public string CategoryName { get; set; }
    }
}
