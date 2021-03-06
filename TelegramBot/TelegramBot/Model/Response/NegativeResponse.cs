namespace TelegramBot.Model.Response
{
    public class NegativeResponse : IResponseForBot
    {
        private readonly string _message;

        public NegativeResponse(string message)
        {
            _message = message;
        }

        public override string ToString()
        {
            return _message;
        }
    }
}