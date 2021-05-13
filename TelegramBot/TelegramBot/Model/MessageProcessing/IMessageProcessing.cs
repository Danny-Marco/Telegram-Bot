using Telegram.Bot.Args;
using TelegramBot.Model.Response;

namespace TelegramBot.Model.MessageProcessing
{
    public interface IMessageProcessing
    {
        IResponseForBot Response(MessageEventArgs eventArgs);
    }
}