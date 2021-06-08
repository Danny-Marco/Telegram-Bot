using System.Text.Json.Serialization;

namespace TelegramBot.Model
{
    public class DataFromFileObject
    {
        [JsonPropertyName("token")]
        public string token { get; set; }
        
        [JsonPropertyName("urlApi")]
        public string urlApi { get; set; }
    }
}