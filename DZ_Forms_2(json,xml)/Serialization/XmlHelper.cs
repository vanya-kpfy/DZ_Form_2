using System;
using System.IO;
using System.Xml.Serialization;

namespace DZ_Forms_2_json_xml_.Serialization
{
    /// <summary>
    /// Вспомогательный класс для работы с XML
    /// </summary>
    public static class XmlHelper
    {
        /// <summary>
        /// Сохранение в XML
        /// </summary>
        public static void SaveToXml<T>(string path, T data)
        {
            try
            {
                // Создаём сериализатор
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                // Создаём файл и сохраняем
                using (FileStream fs = new FileStream(path, FileMode.Create))
                {
                    serializer.Serialize(fs, data);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Ошибка сохранения XML: " + ex.Message,
                    "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
                throw;
            }
        }

        /// <summary>
        /// Загрузка из XML
        /// </summary>
        public static T LoadFromXml<T>(string path)
        {
            if (!File.Exists(path))
            {
                return default(T);
            }

            try
            {
                XmlSerializer serializer = new XmlSerializer(typeof(T));

                using (FileStream fs = new FileStream(path, FileMode.Open))
                {
                    return (T)serializer.Deserialize(fs);
                }
            }
            catch (Exception ex)
            {
                System.Windows.Forms.MessageBox.Show(
                    "Ошибка загрузки XML: " + ex.Message,
                    "Ошибка",
                    System.Windows.Forms.MessageBoxButtons.OK,
                    System.Windows.Forms.MessageBoxIcon.Error);
                return default(T);
            }
        }
    }
}