using System;
using System.IO;
using System.Windows.Forms;
using System.Xml.Serialization;

namespace DZ_Forms_2_json_xml_.Serialization
{
    public static class XmlHelper
    {
        public static void SaveToXml<T>(string path, T data)
        {
            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (StreamWriter writer = new StreamWriter(path))
                {
                    serializer.Serialize(writer, data);
                }
                MessageBox.Show($"XML сохранён: {path}", "Отладка");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка XML: {ex.Message}", "Ошибка");
            }
        }

        public static T LoadFromXml<T>(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    MessageBox.Show($"XML файл не найден: {path}", "Отладка");
                    return default(T);
                }

                XmlSerializer serializer = new XmlSerializer(typeof(T));
                using (StreamReader reader = new StreamReader(path))
                {
                    T result = (T)serializer.Deserialize(reader);
                    return result;
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки XML: {ex.Message}", "Ошибка");
                return default(T);
            }
        }
    }
}