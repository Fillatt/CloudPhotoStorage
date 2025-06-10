namespace CloudPhotoStorage.API.DTOs
{
    public record ImageWithCategoryDTO
    {
        public required string ImageName { get; set; }
        public required string Category { get; set; }
        public required DateTime? UploadDate { get; set; }
    }
}
