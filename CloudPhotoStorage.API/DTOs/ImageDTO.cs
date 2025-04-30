namespace CloudPhotoStorage.API.DTOs
{
    public record ImageDTO
    {
        public string FileName { get; init; }
        public DateTime? UploadDate { get; init; }
        public string UserLogin { get; init; }
        public string CategoryName { get; init; }
    }
}
