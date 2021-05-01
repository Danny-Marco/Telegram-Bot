using System;
using System.Threading.Tasks;
using TelegramBot.Model.Response;

namespace TelegramBot
{
    class Program
    {
        static async Task Main(string[] args)
        {
            var url = "https://api.privatbank.ua/p24api/exchange_rates?json&date=";
            var date = "29.04.2021";
            var currency = "usd";

            var generatingResponse = new GeneratingResponse();
            var response = generatingResponse.Response(date, currency, url);
            Console.WriteLine(response.ToString());
        }
    }
}