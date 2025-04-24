namespace CloudPhotoStorage.API.DTOs
{
    public class ImageDTO
    {
        public int ImageId { get; set; }
        public string FileName { get; set; }
        public DateTime? UploadDate { get; set; }
        public string UserLogin { get; set; }
        public string CategoryName { get; set; }
    }
}
