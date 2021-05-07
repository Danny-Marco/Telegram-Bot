namespace TelegramBot.Model.Response
{
    public class PositiveResponse : IResponseForBot
    {
        private readonly ExchangeRate _rate;

        public PositiveResponse(ExchangeRate rate)
        {
            _rate = rate;
        }

        public override string ToString()
        {
            return CreateMessage();
        }

        private string CreateMessage()
        {
            string message;
            var messageIfExchangeBetterOne =
                $"Курс обмена гривны к {_rate.Currency}:\n" +
                $"Курс покупки: {_rate.PurchaseRate:0.00}\nКурс продажи: {_rate.SaleRate:0.00}";
            var messageIfExchangeLessOne = $"Курс нацианального банка: {_rate.SaleRateNB:0.000}";

            if (_rate.PurchaseRateNB < 1 & _rate.PurchaseRate == 0)
            {
                message = messageIfExchangeLessOne;
            }
            else
            {
                message = messageIfExchangeBetterOne;
            }

            return message;
        }
    }
}