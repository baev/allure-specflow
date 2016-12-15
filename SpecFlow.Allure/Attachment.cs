using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace SpecFlow.Allure
{
    internal class Attachment
    {
        private static readonly Dictionary<string, string> MimeTypes = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase)
        {
            {"txt", "text/plain"},
            {"xml", "application/xml"},
            {"html", "text/html"},
            {"png", "image/png"},
            {"jpg", "image/jpeg"},
            {"json", "application/json"},
        };

        public string Name { get; private set; }
        public string Path { get; private set; }

        public string MimeType
        {
            get
            {
                string defaultValue = MimeTypes["txt"];
                string result;
                if (string.IsNullOrWhiteSpace(this.Path))
                    return defaultValue;

                var ext = System.IO.Path.GetExtension(Regex.Escape(this.Path)).Replace(".", string.Empty);
                if (MimeTypes.TryGetValue(ext, out result))
                    return result;
                else return defaultValue;
            }
        }
        public byte[] Read()
        {
            try
            {
                return File.ReadAllBytes(this.Path);
            }
            catch (Exception)
            {
                return new byte[0];
            }

        }

        internal Attachment(string path, string name)
        {
            this.Path = path;
            this.Name = name;
        }
    }
}
