namespace CloudPhotoStorage.API.DTOs
{
    public record ImageWithCategoryDto
    {
        public required string ImageName { get; set; }
        public required string Category { get; set; }
    }
}
