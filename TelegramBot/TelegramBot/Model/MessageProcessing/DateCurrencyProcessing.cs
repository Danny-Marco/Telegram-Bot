using System;
using System.Linq;
using Telegram.Bot.Args;
using TelegramBot.Model.Response;
using TelegramBot.Model.TelegramBot;
using TelegramBot.Model.ValidateInputData;

namespace TelegramBot.Model.MessageProcessing
{
    public class DateCurrencyProcessing : IMessageProcessing
    {
        private readonly Bot _bot;

        public DateCurrencyProcessing(ref Bot bot)
        {
            _bot = bot;
        }

        public void Response(MessageEventArgs eventArgs)
        {
            var message = eventArgs.Message.Text;
            var currency = message.Split(" ").First();
            var date = message.Split(" ").Last();

            var validateDate = new ValidateDate(date);

            if (validateDate.IsDateCorrect())
            {
                _bot.Response = CreateResponse(currency, date);
            }
            else
            {
                _bot.Response = validateDate.CreateResponseIfDateIncorrect();
            }
        }

        private IResponseForBot CreateResponse(string currency, string date)
        {
            date = DateTime.Parse(date).ToString("dd/MM/yyyy");
            var stringJson = GetStringJson(date);
            var response = new ParseJsonData();
            return response.CreateResponseByJsonData(stringJson, currency);
        }

        private string GetStringJson(string date)
        {
            var stringJson = new StringJson(_bot.API_URL, date);
            return stringJson.GetStringJson();
        }
    }
}