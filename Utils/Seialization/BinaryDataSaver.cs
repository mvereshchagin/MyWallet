using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;

namespace Utils.Serialization
{
    public class BinaryDataSaver : DataSaver
    {
        public BinaryDataSaver(string dataDir) : base(dataDir: dataDir)
        {
        }

        protected override string Extension => "dat";

        public override T LoadItem<T>(string fileName) where T : class
        {
            string filePath = this.CreateFilePath(fileName);
            BinaryFormatter formatter = new BinaryFormatter();

            T? data = null;
            if (File.Exists(filePath))
            {
                try
                {
                    using (var stream = File.OpenRead(filePath))
                        data = (T)formatter.Deserialize(stream);
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
            BinaryFormatter formatter = new BinaryFormatter();
            try
            {
                using (var stream = File.Create(filePath))
                    formatter.Serialize(stream, data);
            }
            catch (Exception)
            {
                Console.WriteLine($"Cannot save data to file {filePath}");
            }
        }
    }
}
