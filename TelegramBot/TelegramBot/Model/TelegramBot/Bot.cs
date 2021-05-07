using System;
using System.Threading.Tasks;
using Telegram.Bot;
using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using TelegramBot.Model.Response;

namespace TelegramBot.Model.TelegramBot
{
    public class Bot
    {
        private static TelegramBotClient _client;
        private readonly string _token;
        private readonly string _url;
        private string _date;
        private string _currency;
        private IResponseForBot _response;

        public Bot(string token, string url)
        {
            _token = token;
            _url = url;
            _client = new TelegramBotClient(_token);
        }

        public void Start()
        {
            Console.WriteLine("Бот запустился. Нажмите любую клавишу для выхода");
            _client.StartReceiving();
            _client.OnMessage += BotOnMessage;

            Console.ReadKey();
            _client.StopReceiving();
        }

        private async void BotOnMessage(object sender, MessageEventArgs e)
        {
            var message = e.Message;
            bool isMessageStart = message.Text == "/start";
            bool isDateEmpty = _date == null;
            bool isCurrencyEmpty = _currency == null;

            if (message == null || message.Type != MessageType.Text)
            {
                return;
            }

            if (isMessageStart)
            {
                ShowGreeting(e);
                _date = null;
                _currency = null;
                _response = null;
            }

            if (!isMessageStart && isDateEmpty && isCurrencyEmpty)
            {
                await SetDate(e);
            }

            if (!isMessageStart && !isDateEmpty && isCurrencyEmpty)
            {
                if (e.Message.Text == null) return;
                _currency = e.Message.Text;
                _response = SetResponse(_date, _currency);
            }
            
            if (_response != null)
            {
                await ShowResponse(e);
                await PromptToStart(e);
            }
        }

        private async void ShowGreeting(MessageEventArgs e)
        {
            await _client.SendTextMessageAsync
            (
                chatId: e.Message.Chat,
                text: $"Курс обмена валюты по отношению к гривне.\n" +
                      "Введите дату в формате: дд.мм.гггг\n" +
                      $"На пример {DateTime.Now:dd.MM.yyyy}"
            );
        }

        private async Task SetDate(MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                _date = e.Message.Text;
            }

            await ShowMessageGetCurrency(e);
        }

        private async Task ShowMessageGetCurrency(MessageEventArgs e)
        {
            await _client.SendTextMessageAsync
            (
                chatId: e.Message.Chat,
                text: "Ведите 3-х значный код интересующей вас валюты:\nНапример USD"
            );
        }

        private async Task ShowResponse(MessageEventArgs e)
        {
            // if (e.Message.Text != null)
            // {
            //     _currency = e.Message.Text;
            // }
            //
            // _response = Response(_date, _currency);
            await _client.SendTextMessageAsync(e.Message.Chat, $"{_response.ToString()}");
        }

        private IResponseForBot SetResponse(string date, string currency)
        {
            var createResponse = new CreateResponse();
            return createResponse.Response(date, currency, _url);
        }
        
        private async Task PromptToStart(MessageEventArgs e)
        {
            await _client.SendTextMessageAsync(e.Message.Chat, $"/start");
            _date = null;
            _currency = null;
            _response = null;
        }
    }
}