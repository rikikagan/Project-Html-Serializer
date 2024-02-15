using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Text.Json;
using System.IO;

namespace HtmlSerializer
{
    public class HtmlHelper
    {
        private readonly static HtmlHelper _instance = new HtmlHelper();
        public static HtmlHelper Instance => _instance;
        public string[] AllHtmlTags { get; set; }
        public string[] SelfClosingTags { get; set; }

        private HtmlHelper()
        {
            // קריאת הנתונים מקובץ JSON והשמתם במערכים המתאימים
            AllHtmlTags = LoadTagsFromFile("HtmlTags.json");
            SelfClosingTags = LoadTagsFromFile("HtmlVoidTags.json");
        }
        private string[] LoadTagsFromFile(string filePath)
        {
            string json = File.ReadAllText(filePath);
            return JsonSerializer.Deserialize<string[]>(json);
        }

    }
}
