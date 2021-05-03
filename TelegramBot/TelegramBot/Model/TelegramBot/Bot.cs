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
        private readonly string _token;
        private readonly string _url;
        private static TelegramBotClient _client;
        private string _date;
        private string _currency;

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
                _date = null;
                _currency = null;
                ShowGreeting(sender, e);
            }

            if (!isMessageStart && isDateEmpty && isCurrencyEmpty)
            {
                await GetDate(sender, e);
            }

            if (!isMessageStart && !isDateEmpty && isCurrencyEmpty)
            {
                await ShowResponse(sender, e);
            }

            if (!isMessageStart && !isDateEmpty && isCurrencyEmpty)
            {
                await PromptToStart(sender, e);
            }

            if (isMessageStart && !isDateEmpty && isCurrencyEmpty)
            {
                await PromptToStart(sender, e);
            }
        }

        private async void ShowGreeting(object sender, MessageEventArgs e)
        {
            await _client.SendTextMessageAsync
            (
                chatId: e.Message.Chat,
                text: "Курс обмена валюты по отношению к гривне.\n" +
                      "Введите дату в формате:\nдд.ММ.гггг\n"
            );
        }

        private async Task GetDate(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                _date = e.Message.Text;
            }

            await ShowMessageGetCurrency(sender, e);
        }

        private async Task ShowMessageGetCurrency(object sender, MessageEventArgs e)
        {
            await _client.SendTextMessageAsync
            (
                chatId: e.Message.Chat,
                text: "Ведите 3-х значный код интересующей вас валюты:\nНапример USD"
            );
        }

        private async Task ShowResponse(object sender, MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                _currency = e.Message.Text;
            }

            var response = Response(_date, _currency);
            await _client.SendTextMessageAsync(e.Message.Chat, $"{response}");
        }

        private async Task PromptToStart(object sender, MessageEventArgs e)
        {
            await _client.SendTextMessageAsync(e.Message.Chat, $"/start");
            _date = null;
            _currency = null;
        }

        private string Response(string date, string currency)
        {
            var generatingResponse = new GeneratingResponse();
            var response = generatingResponse.Response(date, currency, _url);
            return response.ToString();
        }
    }
}