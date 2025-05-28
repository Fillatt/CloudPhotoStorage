namespace CloudPhotoStorage.API.DTOs
{
    public class WasteBasketDTO
    {
        public required string FileName { get; set; }
        public required string UserLogin { get; set; }
        public required DateTime? DeleteDate { get; set; }
    }
}
