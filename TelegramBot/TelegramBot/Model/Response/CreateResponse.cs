using System;
using System.Globalization;
using System.Net;

namespace TelegramBot.Model.Response
{
    public class CreateResponse
    {
        public IResponseForBot Response(string date, string currency, string url)
        {
            var responseByJson = new ParseJsonData();
            currency = currency.ToUpper();
            if (!IsDateCorrect(date))
            {
                return new NegativeResponse("Введённая дата не корректна!");
            }

            if (!InputDateNotLaterToday(date))
            {
                return new NegativeResponse("Введённая дата не может быть позднее текущей!");
            }

            var stringJson = GetStringJson(date, url);
            var response = responseByJson.ResponseByJsonData(stringJson, currency);

            return response;
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

        private string GetStringJson(string date, string inputUrl)
        {
            date = ParseDate(date);
            var requestUrl = inputUrl + date;
            using var webClient = new WebClient();
            var jsonString = webClient.DownloadString(requestUrl);
            return jsonString;
        }
    }
}