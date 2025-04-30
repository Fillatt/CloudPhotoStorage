namespace CloudPhotoStorage.API.DTOs
{
    public class WasteBasketDTO
    {
        public string FileName { get; init; }
        public string UserLogin { get; init; }
        public DateTime? DeleteDate { get; init; }
    }
}
