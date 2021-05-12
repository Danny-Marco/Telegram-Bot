using System;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using TelegramBot.Model.Response;
using TelegramBot.Model.ValidateInputData;

namespace TelegramBot.Model.TelegramBot
{
    public class Bot
    {
        private static TelegramBotClient _client;
        private readonly string _token;
        private readonly string _url;
        private string _stringJson;
        private IResponseForBot _response;

        public Bot(string token, string url)
        {
            _token = token;
            _url = url;
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
            var message = e.Message;

            if (message == null || message.Type != MessageType.Text) return;

            if (message.Text == "/start")
            {
                ShowGreeting(e);
            }

            else
            {
                var currency = message.Text.Split(" ").First();
                var date = message.Text.Split(" ").Last();
                var validateDate = new ValidateDate(date);

                if (validateDate.IsDateCorrect())
                {
                    date = DateTime.Parse(date).ToString("dd/MM/yyyy");
                    SetStringJson(date);
                    _response = CreateResponse(currency);
                }
                else
                {
                    _response = validateDate.CreateResponseIfDateIncorrect();
                }
            }
            
            if (_response != null)
            {
                ShowResponse(e);
            }
        }
        
        private async void ShowGreeting(MessageEventArgs e)
        {
            await _client.SendTextMessageAsync
            (
                chatId: e.Message.Chat,
                text: $"Курс обмена валюты по отношению к гривне.\n" +
                      "Введите данные в формате: код валюты дата\n" +
                      $"Например: usd {DateTime.Now:dd.MM.yyyy}"
            );
        }
        
        private async void ShowResponse(MessageEventArgs e)
        {
            await _client.SendTextMessageAsync(e.Message.Chat, $"{_response}");
        }

        private IResponseForBot CreateResponse(string currency)
        {
            var response = new ParseJsonData();
            return response.CreateResponseByJsonData(_stringJson, currency);
        }

        private void SetStringJson(string date)
        {
            var stringJson = new StringJson(_url, date);
            _stringJson = stringJson.GetStringJson();
        }
    }
}