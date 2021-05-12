using System;
using NUnit.Framework;
using TelegramBot.Model.Response;
using TelegramBot.Model.ValidateInputData;

namespace TelegramBot.Tests
{
    public class ValidateDateTests
    {
        private string _date;
        private ValidateDate _validateDate;

        #region IsDateTests

        [Test]
        public void IsDate_StringWithDateLater_False()
        {
            _date = DateTime.Now.AddDays(1).ToString("dd.MM.yyyy");
            _validateDate = new ValidateDate(_date);

            var expectedResponse = false;
            var currentResponse = _validateDate.IsDateCorrect();

            Assert.AreEqual(currentResponse, expectedResponse);
        }

        [Test]
        public void IsDate_ASD_False()
        {
            _date = "ASD";
            _validateDate = new ValidateDate(_date);

            var expectedResponse = false;
            var currentResponse = _validateDate.IsDateCorrect();

            Assert.AreEqual(currentResponse, expectedResponse);
        }

        [Test]
        public void IsDate_DateEarlierThanFourYearsBefore_False()
        {
            _date = DateTime.Now.AddYears(-4).ToString("dd.MM.yyyy");
            _validateDate = new ValidateDate(_date);

            var expectedResponse = false;
            var currentResponse = _validateDate.IsDateCorrect();

            Assert.AreEqual(currentResponse, expectedResponse);
        }

        [Test]
        public void IsDate_DateCorrect_True()
        {
            _date = DateTime.Now.ToString("dd.MM.yyyy");
            _validateDate = new ValidateDate(_date);

            var expectedResponse = true;
            var currentResponse = _validateDate.IsDateCorrect();

            Assert.AreEqual(currentResponse, expectedResponse);
        }

        #endregion


        #region CreateResponseTests

        [Test]
        public void CreateResponseIfDateIncorrect_DateIsEmpty_MessageThatDateEmpty()
        {
            const string expectedResponse = "Дата не введена!";
            _date = "";
            _validateDate = new ValidateDate(_date);

            var currentResponseType = _validateDate.CreateResponseIfDateIncorrect();
            var currentResponseString = _validateDate.CreateResponseIfDateIncorrect().ToString();

            Assert.That(currentResponseType, Is.TypeOf<NegativeResponse>());
            Assert.AreEqual(currentResponseString, expectedResponse);
        }

        [Test]
        public void CreateResponseIfDateIncorrect_DateIsLater_MessageThatDateIsLater()
        {
            const string expectedResponse = "Введённая дата не может быть позже текущей!";
            _date = DateTime.Now.AddDays(1).ToString("dd.MM.yyyy");
            _validateDate = new ValidateDate(_date);

            var currentResponseType = _validateDate.CreateResponseIfDateIncorrect();
            var currentResponseString = _validateDate.CreateResponseIfDateIncorrect().ToString();

            Assert.That(currentResponseType, Is.TypeOf<NegativeResponse>());
            Assert.AreEqual(currentResponseString, expectedResponse);
        }

        [Test]
        public void CreateResponseIfDateIncorrect_DateIsLEarlierThan4YearsBefore_MessageThereIsDataForLast4Years()
        {
            const string expectedResponse = "Можно получить курс только за последние 4 года!";
            _date = DateTime.Now.AddYears(-4).ToString("dd.MM.yyyy");
            _validateDate = new ValidateDate(_date);

            var currentResponseType = _validateDate.CreateResponseIfDateIncorrect();
            var currentResponseString = _validateDate.CreateResponseIfDateIncorrect().ToString();

            Assert.That(currentResponseType, Is.TypeOf<NegativeResponse>());
            Assert.AreEqual(currentResponseString, expectedResponse);
        }

        [Test]
        public void CreateResponseIfDateIncorrect_Asd_MessageThatDateIsIncorrect()
        {
            const string expectedResponse = "Введённая дата не корректна!";
            _date = "Asd";
            _validateDate = new ValidateDate(_date);

            var currentResponseType = _validateDate.CreateResponseIfDateIncorrect();
            var currentResponseString = _validateDate.CreateResponseIfDateIncorrect().ToString();

            Assert.That(currentResponseType, Is.TypeOf<NegativeResponse>());
            Assert.AreEqual(currentResponseString, expectedResponse);
        }

        [Test]
        public void CreateResponseIfDateIncorrect_12_13_21_MessageThatDateIsIncorrect()
        {
            const string expectedResponse = "Введённая дата не корректна!";
            _date = "12.13.21";
            _validateDate = new ValidateDate(_date);

            var currentResponseType = _validateDate.CreateResponseIfDateIncorrect();
            var currentResponseString = _validateDate.CreateResponseIfDateIncorrect().ToString();

            Assert.That(currentResponseType, Is.TypeOf<NegativeResponse>());
            Assert.AreEqual(currentResponseString, expectedResponse);
        }

        [Test]
        public void CreateResponseIfDateIncorrect_30_02_2021_MessageThatDateIsIncorrect()
        {
            const string expectedResponse = "Введённая дата не корректна!";
            _date = "30.02.2021";
            _validateDate = new ValidateDate(_date);

            var currentResponseType = _validateDate.CreateResponseIfDateIncorrect();
            var currentResponseString = _validateDate.CreateResponseIfDateIncorrect().ToString();

            Assert.That(currentResponseType, Is.TypeOf<NegativeResponse>());
            Assert.AreEqual(currentResponseString, expectedResponse);
        }

        [Test]
        public void CreateResponseIfDateIncorrect_30_V_2021_MessageThatDateIsIncorrect()
        {
            const string expectedResponse = "Введённая дата не корректна!";
            _date = "30.v.2021";
            _validateDate = new ValidateDate(_date);

            var currentResponseType = _validateDate.CreateResponseIfDateIncorrect();
            var currentResponseString = _validateDate.CreateResponseIfDateIncorrect().ToString();

            Assert.That(currentResponseType, Is.TypeOf<NegativeResponse>());
            Assert.AreEqual(currentResponseString, expectedResponse);
        }

        #endregion
    }
}