using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SimpleMessagesSignalR
{
    public class JsonConfigurationFileReader
    {
        public string FileName { get; set; }

        public JsonConfigurationFileReader(string fileName)
        {
            this.FileName = fileName;
        }

        public string GetConfigValue(string key)
        {
            var text = System.IO.File.ReadAllText(FileName);
            var obj = JObject.Parse(text);
            return obj.GetValue(key).ToString();
        }
    }
}
