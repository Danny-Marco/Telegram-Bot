using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using TelegramBot.Model.MessageProcessing;
using TelegramBot.Model.Response;

namespace TelegramBot.Model.TelegramBot
{
    public class Bot
    {
        private readonly string _token;

        private string API_URL { get; }

        private TelegramBotClient Client { get; }

        private IResponseForBot Response { get; set; }

        public Bot(string token, string apiUrl)
        {
            _token = token;
            API_URL = apiUrl;
            Client = new TelegramBotClient(_token);
        }

        public void Start()
        {
            var me = Client.GetMeAsync().Result;
            Console.WriteLine($"Бот {me.FirstName} запустился. Нажмите любую клавишу для выхода");
            Client.StartReceiving();
            Client.OnMessage += BotOnMessage;

            Console.ReadKey();
            Client.StopReceiving();
        }

        private void BotOnMessage(object sender, MessageEventArgs e)
        {
            var handler = new MessageHandler(API_URL, e);
            Response = handler.Processing();
            ShowResponse(e);
        }
        
        private async void ShowResponse(MessageEventArgs e)
        {
            await Client.SendTextMessageAsync(e.Message.Chat, $"{Response}");
        }
    }
}