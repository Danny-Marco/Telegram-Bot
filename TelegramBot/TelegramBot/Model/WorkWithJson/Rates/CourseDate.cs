using System.Collections.Generic;
using System.Text.Json.Serialization;

namespace TelegramBot.Model
{
    public class CourseDate
    {
        [JsonPropertyName("date")]
        public string Date { get; set; }
        
        [JsonPropertyName("bank")]
        public string Bank { get; set; }
        
        [JsonPropertyName("baseCurrency")]
        public int BaseCurrency { get; set; }
        
        [JsonPropertyName("baseCurrencyLit")]
        public string BaseCurrencyLit { get; set; }
        
        [JsonPropertyName("exchangeRate")]
        public List<ExchangeRate> ExchangeRate { get; set; }
    }
}