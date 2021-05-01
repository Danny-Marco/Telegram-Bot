using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;
using TelegramBot.Model.Response;

namespace TelegramBot.Model
{
    public class ParseJsonData
    {
        public IResponseForBot ResponseByJsonData(string jsonString, string currency)
        {
            currency = currency.ToUpper();
            return ProcessingResponseByJsonData(jsonString, currency);
        }
        
        private IResponseForBot ProcessingResponseByJsonData(string jsonString, string currency)
        {
            IResponseForBot response;
            try
            {
                var rates = GetExchangeRatesFromBankApi(jsonString);
                if (!rates.Any(x => x.Currency == currency))
                {
                    return new NegativeResponse(
                        $"Данные по валюте {currency} не найдены.\nПроверьте корректность кода валюты.");
                }

                ExchangeRate exchangeRate = GetRateByInputCurrency(currency, rates);
                response = new PositiveResponse(exchangeRate);
            }
            catch
            {
                response = new NegativeResponse("Не удалось получить данные");
            }

            return response;
        }
        
        private List<ExchangeRate> GetExchangeRatesFromBankApi(string jsonString)
        {
            var deserializeRates = JsonConvert.DeserializeObject<CourseDate>(jsonString);
            var rates = deserializeRates.ExchangeRate;
            return rates;
        }
        
        private ExchangeRate GetRateByInputCurrency(string currency, List<ExchangeRate> rates)
        {
            ExchangeRate changeRate = null;
            try
            {
                foreach (var rate in rates)
                {
                    if (rate.Currency == currency)
                    {
                        return rate;
                    }
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
            }

            return changeRate;
        }
    }
}