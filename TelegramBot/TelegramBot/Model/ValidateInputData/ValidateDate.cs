using System;
using System.Globalization;
using TelegramBot.Model.Response;

namespace TelegramBot.Model.ValidateInputData
{
    public class ValidateDate
    {
        private string _date;

        public ValidateDate(string date)
        {
            _date = date;
        }

        public bool IsDateCorrect()
        {
            if (string.IsNullOrEmpty(_date)) return false;
            if (!TryParseDate()) return false;
            try
            {
                var isDateCorrect = IsDate() && !IsDateLaterToday(_date) && IsDateInLastFourYears(_date);
                return isDateCorrect;
            }
            catch
            {
                return false;
            }
        }

        public IResponseForBot CreateResponseIfDateIncorrect()
        {
            if (string.IsNullOrEmpty(_date))
            {
                return new NegativeResponse("Дата не введена!");
                ;
            }

            if (!IsDate())
            {
                return new NegativeResponse("Введённая дата не корректна!");
            }

            if (IsDateLaterToday(_date))
            {
                return new NegativeResponse("Введённая дата не может быть позже текущей!");
            }

            if (!IsDateInLastFourYears(_date))
            {
                return new NegativeResponse("Можно получить курс только за последние 4 года!");
            }

            throw new InvalidOperationException();
        }

        private bool IsDate()
        {
            if (!TryParseDate()) return false;

            var dateFormat = "dd.MM.yyyy";
            DateTime outputDate;

            bool isDateCorrect = DateTime.TryParseExact(
                _date,
                dateFormat,
                DateTimeFormatInfo.InvariantInfo,
                DateTimeStyles.None,
                out outputDate);

            return isDateCorrect;
        }

        private bool TryParseDate()
        {
            try
            {
                var dateValue = DateTime.Parse(_date);
                _date = dateValue.ToString("dd/MM/yyyy");
            }
            catch
            {
                return false;
            }

            return true;
        }

        private bool IsDateLaterToday(string date)
        {
            if (!TryParseDate()) return false;

            var today = DateTime.Now;
            DateTime inputDate = Convert.ToDateTime(date);
            bool isInputDateNotLaterThanNow = inputDate > today;
            return isInputDateNotLaterThanNow;
        }

        private bool IsDateInLastFourYears(string date)
        {
            if (!TryParseDate()) return false;

            var inputDate = Convert.ToDateTime(date);
            var startDate = DateTime.Now.AddYears(-4);
            return inputDate > startDate;
        }
    }
}