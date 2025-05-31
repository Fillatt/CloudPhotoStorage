namespace CloudPhotoStorage.API.DTOs
{
    public record ImageWithCategoryAndUploadDateDto
    {
        public required string ImageName { get; set; }
        public required string Category { get; set; }
        public required DateTime? UploadDate { get; set; }
    }
}
