using System.Threading.Tasks;
using TelegramBot.Model;
using TelegramBot.Model.TelegramBot;

namespace TelegramBot
{
    class Program
    {
        private const string _path = "Config/Config.json";

        static async Task Main(string[] args)
        {
            var dataFromConfigFile = new DataFromFileJson(_path);
            var data = dataFromConfigFile.GetData();

            var bot = new Bot(data.token, data.urlApi);
            bot.Start();
        }
    }
}