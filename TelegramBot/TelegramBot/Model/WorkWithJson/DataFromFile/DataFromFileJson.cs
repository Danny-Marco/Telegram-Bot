using System.IO;
using Newtonsoft.Json;

namespace TelegramBot.Model
{
    public class DataFromFileJson
    {
        private readonly string _path;

        public DataFromFileJson(string path)
        {
            _path = path;
        }

        public DataFromFileObject GetData()
        {
            using StreamReader r = new StreamReader(_path);
            var json = r.ReadToEnd();
            var dataFromFile = JsonConvert.DeserializeObject<DataFromFileObject>(json);
            return dataFromFile;
        }
    }
}