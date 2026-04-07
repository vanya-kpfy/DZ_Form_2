using System.Xml;
using Newtonsoft.Json;
using System.IO;

namespace DZ_Forms_2_json_xml_.Serialization
{
    /// <summary>
    /// Вспомогательный класс для работы с JSON
    /// </summary>
    public static class JsonHelper
    {
        /// <summary>
        /// Сериализация: сохраняет объект в JSON файл
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <param name="data"></param>
        public static void SaveToJson<T>(string path, T data)
        {
            // Превращаем объект в JSON с отступами (красиво)
            string json = JsonConvert.SerializeObject(data, Newtonsoft.Json.Formatting.Indented);
            // Записываем в файл
            File.WriteAllText(path, json);
        }

        /// <summary>
        /// Десериализация: загружает объект из JSON файла
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="path"></param>
        /// <returns>ОБЪЕКТ</returns>
        public static T LoadFromJson<T>(string path)
        {
            if (!File.Exists(path))
            {
                return default(T);
            }

            // Читаем JSON из файла
            string json = File.ReadAllText(path);
            // Превращаем JSON обратно в объект
            return JsonConvert.DeserializeObject<T>(json);
        }
    }
}