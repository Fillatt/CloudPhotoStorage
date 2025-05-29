    namespace CloudPhotoStorage.API.DTOs
    {
        public record ImageDTO
        {
            public required byte[] ImageData { get; set; }
            public required string Name { get; set; }
            public required DateTime? UploadDate { get; set; }
            public required string CategoryName { get; set; }

        }
    }
