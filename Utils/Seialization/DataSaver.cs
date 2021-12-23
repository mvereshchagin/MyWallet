using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Utils.Serialization
{
    public abstract class DataSaver
    {
        public string DataDir { get; }

        //protected string Extension
        //{
        //    get
        //    {
        //        throw new NotImplementedException("You have to fix the extansion");
        //    }
        //}

        protected abstract string Extension { get; }

        public DataSaver(string dataDir)
        {
            this.DataDir = dataDir;
        }

        protected string CreateFilePath(string fileName)
        {
            var fullFileName =
                String.IsNullOrEmpty(this.Extension) ? fileName : 
                $"{fileName}.{this.Extension}";

            return Path.Combine(this.DataDir, fullFileName);
        }

        public abstract T? LoadItem<T>(string fileName) where T : class;

        public abstract void SaveItem<T>(string fileName, T? data) where T : class;
    }
}
