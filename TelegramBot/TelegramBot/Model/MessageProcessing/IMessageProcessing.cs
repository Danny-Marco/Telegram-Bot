using Telegram.Bot.Args;

namespace TelegramBot.Model.MessageProcessing
{
    public interface IMessageProcessing
    {
        void Response(MessageEventArgs eventArgs);
    }
}