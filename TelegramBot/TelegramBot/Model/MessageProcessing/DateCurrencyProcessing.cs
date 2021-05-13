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
        private readonly string _url;

        public DateCurrencyProcessing(string url)
        {
            _url = url;
        }

        public IResponseForBot Response(MessageEventArgs eventArgs)
        {
            IResponseForBot response;
            var message = eventArgs.Message.Text;
            var currency = message.Split(" ").First();
            var date = message.Split(" ").Last();

            var validateDate = new ValidateDate(date);

            if (validateDate.IsDateCorrect())
            {
                response = CreateResponse(currency, date);
            }
            else
            {
                response = validateDate.CreateResponseIfDateIncorrect();
            }

            return response;
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
            var stringJson = new StringJson(_url, date);
            return stringJson.GetStringJson();
        }
    }
}