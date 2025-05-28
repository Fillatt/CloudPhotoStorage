    namespace CloudPhotoStorage.API.DTOs
    {
        public record ImageDTO
        {
            public required byte[] ImagePath { get; set; }
            public required string Name { get; set; }
            public required DateTime? UploadDate { get; set; }
            public required string UserLogin { get; set; }
            public required string CategoryName { get; set; }

        }
    }
