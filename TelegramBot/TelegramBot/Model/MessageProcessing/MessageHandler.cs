using Telegram.Bot.Args;
using Telegram.Bot.Types.Enums;
using TelegramBot.Model.Response;

namespace TelegramBot.Model.MessageProcessing
{
    public class MessageHandler
    {
        private readonly MessageEventArgs _eventArgs;
        private readonly string _url;

        public MessageHandler(string url, MessageEventArgs eventArgs)
        {
            _url = url;
            _eventArgs = eventArgs;
        }

        public IResponseForBot Processing()
        {
            var message = _eventArgs.Message;
            if (message == null || message.Type != MessageType.Text) return null;

            if (message.Text == "/start")
            {
                return CreateResponse(new StartMessageProcessing());
            }

            if (message.Text != "/start")
            {
                return CreateResponse(new DateCurrencyProcessing(_url));
            }
            return null;
        }

        private IResponseForBot CreateResponse(IMessageProcessing processing)
        {
            return processing.Response(_eventArgs);
        }
    }
}