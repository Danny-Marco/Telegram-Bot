using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using TelegramBot.Model.TelegramBot;

namespace TelegramBot.Model.MessageProcessing
{
    public class MessageHandler
    {
        private readonly MessageEventArgs _eventArgs;
        private readonly Bot _bot;

        public MessageHandler(ref Bot bot, MessageEventArgs eventArgs)
        {
            _eventArgs = eventArgs;
            _bot = bot;
        }

        public void Processing(ref Bot bot)
        {
            var message = _eventArgs.Message;
            if (message == null || message.Type != MessageType.Text) return;

            if (message.Text == "/start")
            {
                CreateResponse(new StartMessageProcessing(ref bot));
            }

            if (message.Text != "/start")
            {
                CreateResponse(new DateCurrencyProcessing(ref bot));
            }

            if (bot.Response != null)
            {
                ShowResponse(_eventArgs);
            }
        }

        private void CreateResponse(IMessageProcessing processing)
        {
            processing.Response(_eventArgs);
        }

        private async void ShowResponse(MessageEventArgs e)
        {
            await _bot.Client.SendTextMessageAsync(e.Message.Chat, $"{_bot.Response}");
        }
    }
}