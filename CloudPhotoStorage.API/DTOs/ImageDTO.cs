    namespace CloudPhotoStorage.API.DTOs
    {
        public record ImageDTO
        {

            public byte[] ImagePath { get; init; }
            public string Name { get; init; }
            public DateTime? UploadDate { get; init; }
            public string UserLogin { get; init; }
            public string CategoryName { get; init; }

        }
    }
