using System.IO;
using NUnit.Framework;
using TelegramBot.Model;

namespace TelegramBot.Tests
{
    public class ParseJsonDataTests
    {
        private readonly ParseJsonData _parseJsonData = new ParseJsonData();
        private static readonly string _json = File.ReadAllText("JsonData.json");

        [Test]
        public void ShouldReturnAnswerThatSaleRateIs19_20PurchaseRateIs20_00()
        {
            var currency = "euR";
            const string expectedResponse =
                "Курс обмена гривны к EUR:\nКурс покупки: 19,20\nКурс продажи: 20,00";
            var currentResponse = _parseJsonData.CreateResponseByJsonData(_json, currency);
            Assert.That(currentResponse.ToString(), Is.EqualTo(expectedResponse));
        }

        [Test]
        public void ShouldReturnAnswerThatDataByCurrencyEUNotFound()
        {
            var currency = "eu";
            const string expectedResponse = "Данные по валюте EU не найдены.\nПроверьте корректность кода валюты.";
            var currentResponse = _parseJsonData.CreateResponseByJsonData(_json, currency);
            Assert.That(currentResponse.ToString(), Is.EqualTo(expectedResponse));
        }
    }
}