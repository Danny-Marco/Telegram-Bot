using System;
using Telegram.Bot.Args;
using TelegramBot.Model.Response;

namespace TelegramBot.Model.MessageProcessing
{
    public class StartMessageProcessing : IMessageProcessing
    {
        public IResponseForBot Response(MessageEventArgs eventArgs)
        {
            return new NegativeResponse(
                "Курс обмена валюты по отношению к гривне.\n" +
                "Введите данные в формате: код валюты дата\n" +
                $"Например: usd {DateTime.Now:dd.MM.yyyy}");
        }
    }
}