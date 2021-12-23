using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Xml.Serialization;

namespace Utils.Serialization
{
    public class XmlDataSaver : DataSaver
    {
        public XmlDataSaver(string dataDir) : base(dataDir: dataDir)
        {
        }

        protected override string Extension => "xml";

        public override T? LoadItem<T>(string fileName) where T : class
        {
            string filePath = this.CreateFilePath(fileName);
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            T? data = null;
            if (File.Exists(filePath))
            {
                try
                {
                    using (var stream = File.OpenRead(filePath))
                        data = (T?)serializer.Deserialize(stream);
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
            XmlSerializer serializer = new XmlSerializer(typeof(T));
            try
            {
                using (var stream = File.Create(filePath))
                    serializer.Serialize(stream, data);
            }
            catch (Exception)
            {
                Console.WriteLine($"Cannot save data to file {filePath}");
            }
        }
    }
}
