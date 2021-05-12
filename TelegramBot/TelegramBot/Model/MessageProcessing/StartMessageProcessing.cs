using System;
using Telegram.Bot;
using Telegram.Bot.Args;
using TelegramBot.Model.Response;
using TelegramBot.Model.TelegramBot;

namespace TelegramBot.Model.MessageProcessing
{
    public class StartMessageProcessing : IMessageProcessing
    {
        private readonly TelegramBotClient _client;
        private readonly Bot _bot;

        public StartMessageProcessing(ref Bot bot)
        {
            _bot = bot;
        }

        public void Response(MessageEventArgs eventArgs)
        {
            _bot.Response = new NegativeResponse(
                "Курс обмена валюты по отношению к гривне.\n" +
                "Введите данные в формате: код валюты дата\n" +
                $"Например: usd {DateTime.Now:dd.MM.yyyy}");
        }
    }
}