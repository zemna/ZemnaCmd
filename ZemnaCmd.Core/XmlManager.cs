using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Xml;

namespace ZemnaCmd.Core
{
    public class XmlManager
    {
        private const string XML_FILE_NAME = "ZemnaCmd.xml";

        public IList<String> LoadPaths()
        {
            XmlReaderSettings settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Fragment;
            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;

            List<String> paths = new List<string>();

            try
            {
                string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), XML_FILE_NAME);

                using (XmlReader reader = XmlReader.Create(path, settings))
                {
                    if (reader.ReadToFollowing("Path") == true)
                    {
                        do
                        {
                            paths.Add(reader.ReadString());
                        } while (reader.ReadToNextSibling("Path"));
                    }
                }
            }
            catch (FileNotFoundException)
            {
            }

            return paths;
        }

        public void SavePaths(IList<String> paths)
        {
            XmlWriterSettings settings = new XmlWriterSettings();
            settings.Indent = true;
            settings.IndentChars = ("    ");

            string path = Path.Combine(Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location), XML_FILE_NAME);

            using (XmlWriter writer = XmlWriter.Create(path, settings))
            {
                writer.WriteStartElement("Config");
                writer.WriteStartElement("Paths");
                foreach (object item in paths)
                {
                    writer.WriteElementString("Path", item.ToString());
                }
                writer.WriteEndElement();
                writer.WriteEndElement();
                writer.Flush();
            }
        }
    }
}
