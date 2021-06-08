using System.Net.Http;

namespace TelegramBot.Model
{
    public class StringJson
    {
        private readonly string _url;
        private readonly string _date;

        public StringJson(string url, string date)
        {
            _url = url;
            _date = date;
        }

        public string GetStringJson()
        {
            using var httpClient = new HttpClient(); 
            return httpClient.GetStringAsync(_url + _date).Result;
        }
    }
}