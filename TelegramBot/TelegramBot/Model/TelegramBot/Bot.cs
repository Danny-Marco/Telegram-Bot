using System;
using System.Globalization;
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

        private async void BotOnMessage(object sender, MessageEventArgs e)
        {
            var message = e.Message;
            bool isMessageStart = message.Text == "/start";
            bool isDateEmpty = _date == null;
            bool isCurrencyEmpty = _currency == null;

            if (message == null || message.Type != MessageType.Text) return;

            if (isMessageStart)
            {
                ShowGreeting(e);
                _date = null;
                _currency = null;
                _response = null;
                _stringJson = null;
            }

            if (!isMessageStart && isDateEmpty && isCurrencyEmpty)
            {
                await SetDate(e);

                if (IsDateCorrect(_date) && !IsDateLaterToday(_date) && IsDateInLastFourYears(_date))
                {
                    _date = ParseDate(_date);
                    await SetStringJson();
                    await ShowMessageGetCurrency(e);
                }
                else
                {
                    SetResponseIfDateIncorrect();
                }
            }

            if (!isMessageStart && !isDateEmpty && isCurrencyEmpty)
            {
                if (e.Message.Text == null) return;
                _currency = e.Message.Text;
                _response = CreateResponse(_currency);
            }
            
            if (_response != null)
            {
                await ShowResponse(e);
                await PromptToStart(e);
            }
        }

        private async Task SetStringJson()
        {
            var stringJson = new StringJson(_url, _date);
            _stringJson = stringJson.GetStringJson();
        }

        private void SetResponseIfDateIncorrect()
        {
            if (!IsDateCorrect(_date))
            {
                _response = new NegativeResponse("Введённая дата не корректна!");
            }

            else if (IsDateLaterToday(_date))
            {
                _response = new NegativeResponse("Введённая дата не может быть позже текущей!");
            }
            
            else if (!IsDateInLastFourYears(_date))
            {
                _response = new NegativeResponse("Можно получить курс только за последние 4 года!");
            }
        }

        private string ParseDate(string date)
        {
            try
            {
                var dateValue = DateTime.Parse(date);
                return dateValue.ToString("dd/MM/yyyy");
            }
            catch (FormatException)
            {
                throw new FormatException("Неверная дата");
            }
        }
        
        private bool IsDateCorrect(string date)
        {
            try
            {
                date = ParseDate(date);
            }
            catch
            {
                return false;
            }

            var dateFormat = "dd.MM.yyyy";
            DateTime outputDate;

            bool isDateCorrect = DateTime.TryParseExact(
                date,
                dateFormat,
                DateTimeFormatInfo.InvariantInfo,
                DateTimeStyles.None,
                out outputDate);

            return isDateCorrect;
        }

        private bool IsDateLaterToday(string date)
        {
            try
            {
                date = ParseDate(date);
            }
            catch
            {
                return false;
            }

            var today = DateTime.Now;
            DateTime inputDate = Convert.ToDateTime(date);
            bool isInputDateNotLaterThanNow = inputDate > today;
            return isInputDateNotLaterThanNow;
        }
        
        private bool IsDateInLastFourYears(string date)
        {
            try
            {
                date = ParseDate(date);
            }
            catch
            {
                return false;
            }

            var inputDate = Convert.ToDateTime(date);
            var startDate = DateTime.Now.AddYears(-4);
            return inputDate > startDate;
        }

        private async void ShowGreeting(MessageEventArgs e)
        {
            await _client.SendTextMessageAsync
            (
                chatId: e.Message.Chat,
                text: $"Курс обмена валюты по отношению к гривне.\n" +
                      "Введите дату в формате: дд.мм.гггг\n" +
                      $"Например: {DateTime.Now:dd.MM.yyyy}"
            );
        }

        private async Task SetDate(MessageEventArgs e)
        {
            if (e.Message.Text != null)
            {
                _date = e.Message.Text;
            }
        }

        private async Task ShowMessageGetCurrency(MessageEventArgs e)
        {
            await _client.SendTextMessageAsync
            (
                chatId: e.Message.Chat,
                text: "Ведите 3-х значный код интересующей вас валюты:\nНапример usd"
            );
        }

        private async Task ShowResponse(MessageEventArgs e)
        {
            await _client.SendTextMessageAsync(e.Message.Chat, $"{_response}");
        }

        private IResponseForBot CreateResponse(string currency)
        {
            var response = new ParseJsonData();
            return response.CreateResponseByJsonData(_stringJson, currency);
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