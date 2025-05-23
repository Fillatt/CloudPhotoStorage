namespace CloudPhotoStorage.API.DTOs
{
    public record UserDTO
    {
        public required string Login { get; set; }
        public required string Password { get; set; }
        public required string Role { get; set; }
    }
}