namespace CloudPhotoStorage.API.DTOs
{
    public record ImageNameDTO
    {
       public Guid Id {  get; init; }
       public string ImageName { get; init; }
    }
}
