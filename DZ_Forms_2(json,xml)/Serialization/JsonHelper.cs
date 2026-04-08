using System;
using System.IO;
using System.Windows.Forms;
using Newtonsoft.Json;

namespace DZ_Forms_2_json_xml_.Serialization
{
    public static class JsonHelper
    {
        public static void SaveToJson<T>(string path, T data)
        {
            try
            {
                string json = JsonConvert.SerializeObject(data, Formatting.Indented);
                File.WriteAllText(path, json);
                MessageBox.Show($"JSON сохранён: {path}", "Отладка");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка JSON: {ex.Message}", "Ошибка");
            }
        }

        public static T LoadFromJson<T>(string path)
        {
            try
            {
                if (!File.Exists(path))
                {
                    MessageBox.Show($"Файл не найден: {path}", "Отладка");
                    return default(T);
                }

                string json = File.ReadAllText(path);
                MessageBox.Show($"Читаем JSON:\n{json}", "Отладка - содержимое");

                T result = JsonConvert.DeserializeObject<T>(json);

                if (result == null)
                {
                    MessageBox.Show("Десериализация вернула null!", "Отладка");
                }

                return result;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки JSON: {ex.Message}\n{ex.StackTrace}", "Ошибка");
                return default(T);
            }
        }
    }
}