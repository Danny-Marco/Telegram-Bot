using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Net;
using Newtonsoft.Json;
using TelegramBot.Model.Response;

namespace TelegramBot.Model
{
    public class JsonHandler
    {
        public IResponseForBot CreateResponseFromBankAPI(string date, string currency)
        {
            currency = currency.ToUpper();

            if (!DateIsCorrect(date))
            {
                return new NegativeResponse("Введённая дата не корректна!");
            }

            if (InputDateNotLaterToday(date) == false)
            {
                return new NegativeResponse("Введённая дата не может быть позднее текущей!");
            }

            return GetDataFromBankAPI(date, currency);
        }

        private IResponseForBot GetDataFromBankAPI(string date, string currency)
        {
            IResponseForBot response;
            try
            {
                var rates = GetExchangeRatesFromBankApi(date);
                if (!IsHasRatesWithInputCurrency(rates, currency))
                {
                    return new NegativeResponse(
                        $"Данные по валюте {currency} не найдены.\nПроверьте корректность ввода кода валюты.");
                }

                ExchangeRate exchangeRate = GetRateByInputCurrency(currency, rates);
                response = new PositiveResponse(exchangeRate);
            }
            catch
            {
                response = new NegativeResponse("Нет данных");
            }

            return response;
        }

        private List<ExchangeRate> GetExchangeRatesFromBankApi(string date)
        {
            date = ParseDate(date);
            var url = "https://api.privatbank.ua/p24api/exchange_rates?json&date=" + date;
            using var webClient = new WebClient();
            var jsonString = webClient.DownloadString(url);
            var deserializeRates = JsonConvert.DeserializeObject<CourseDate>(jsonString);
            var rates = deserializeRates.ExchangeRate;
            return rates;
        }

        private string ParseDate(string date)
        {
            DateTime dateValue;
            try
            {
                dateValue = DateTime.Parse(date);
                return dateValue.ToString("dd/MM/yyyy");
            }
            catch (FormatException)
            {
                throw new FormatException("Неверная дата");
            }
        }

        private bool DateIsCorrect(string date)
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

        private bool InputDateNotLaterToday(string date)
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
            bool isInputDateNotLaterThanNow = inputDate <= today;
            return isInputDateNotLaterThanNow;
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
                        changeRate = rate;
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

        private bool IsHasRatesWithInputCurrency(List<ExchangeRate> rates, string currency)
        {
            var jsonHasCurrencyRate = rates.Any(x => x.Currency == currency);
            return jsonHasCurrencyRate;
        }
    }
}