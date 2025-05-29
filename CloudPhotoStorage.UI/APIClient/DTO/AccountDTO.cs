using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.Json.Serialization;
using System.Threading.Tasks;

namespace CloudPhotoStorage.UI.APIClient.DTO
{
    public class AccountDTO
    {
        [JsonPropertyName(nameof(Login))]
        public required string Login { get; set; }

        [JsonPropertyName(nameof(Password))]
        public required string Password { get; set; }
    }
}
