using System.Threading.Tasks;
using TelegramBot.Model.TelegramBot;

namespace TelegramBot
{
    class Program
    {
        private const string TOKEN = "1691813196:AAE1-35CGXuitIIhbqKYAdlurGZurwBu1pg";
        private const string _url = "https://api.privatbank.ua/p24api/exchange_rates?json&date=";

        static async Task Main(string[] args)
        {
            var bot = new Bot(TOKEN, _url);
            bot.Start();
        }
    }
}