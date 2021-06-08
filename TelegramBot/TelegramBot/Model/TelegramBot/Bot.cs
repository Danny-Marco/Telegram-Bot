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
        private readonly string _URL_API;
        private readonly TelegramBotClient _client;
        private IResponseForBot _response;

        public Bot(string token, string apiUrl)
        {
            _token = token;
            _URL_API = apiUrl;
            _client = new TelegramBotClient(_token);
        }

        public void Start()
        {
            var me = _client.GetMeAsync().Result;
            Console.WriteLine($"Бот {me.FirstName} запустился. Нажмите любую клавишу для выхода");
            _client.StartReceiving();
            _client.OnMessage += BotOnMessage;

            Console.ReadKey();
            _client.StopReceiving();
        }

        private void BotOnMessage(object sender, MessageEventArgs e)
        {
            var handler = new MessageHandler(_URL_API, e);
            _response = handler.Processing();
            ShowResponse(e);
        }

        private async void ShowResponse(MessageEventArgs e)
        {
            await _client.SendTextMessageAsync(e.Message.Chat, $"{_response}");
        }
    }
}