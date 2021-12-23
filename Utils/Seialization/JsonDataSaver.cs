using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace Utils.Serialization
{
    public class JsonDataSaver : DataSaver
    { 
        public JsonDataSaver(string dataDir) : base(dataDir: dataDir)
        {
        }

        protected override string Extension => "json";

        public override T? LoadItem<T>(string fileName) where T : class
        {
            string filePath = this.CreateFilePath(fileName);
            T? data = null;
            if (File.Exists(filePath))
            {
                try
                {
                    using (var stream = File.OpenText(filePath))
                    {
                        var jsonString = stream.ReadToEnd();
                        data = JsonSerializer.Deserialize<T?>(jsonString);
                    }
                }
                catch (Exception)
                {
                    Console.WriteLine($"Cannot read file {filePath}");
                }
            }

            return data;
        }

        public override void SaveItem<T>(string fileName, T? data) where T : class
        {
            if (data is null)
                return;
            string filePath = this.CreateFilePath(fileName);
            try
            {
                using (var stream = File.CreateText(filePath))
                {
                    var jsonString = JsonSerializer.Serialize(data);
                    stream.Write(jsonString);
                }
            }
            catch (Exception)
            {
                Console.WriteLine($"Cannot save data to file {filePath}");
            }
        }
    }
}
