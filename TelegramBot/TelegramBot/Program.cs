using System;
using TelegramBot.Model;

namespace TelegramBot
{
    class Program
    {
        static void Main(string[] args)
        {
            var date = "30.04.2021";
            var currency = "eur";
            var handler = new JsonHandler();
            var rate = handler.CreateResponseFromBankAPI(date, currency);
            
            Console.WriteLine(rate.ToString());
        }
    }
}