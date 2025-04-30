namespace CloudPhotoStorage.API.DTOs
{
    public record UserDTO
    {
        public string LoginString { get; init; }
        public string PasswordHash { get; init; }
        public string PasswordSalt { get; init; }
        public string RoleID { get; init; }
    }
}