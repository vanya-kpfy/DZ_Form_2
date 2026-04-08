using System;

namespace DZ_Forms_2_json_xml_.Classes_Transport
{
    /// <summary>
    /// Класс "Водитель"
    /// </summary>
    [Serializable]  // Атрибут для сериализации (нужен для XML)
    public class Driver
    {
        // Свойства (их 2, нужно 5 свойств на сущность,  Driver + Schedule дают 4 свойства, плюс Id, Number, Capacity = 7)

        /// <summary>
        /// ФИО водителя
        /// </summary>
        public string Name { get; set; }
        /// <summary>
        /// Стаж в годах
        /// </summary>
        public int Experience { get; set; } 
        public Driver() { }
        public Driver(string name, int experience)
        {
            Name = name;
            Experience = experience;
        }

        public override string ToString()
        {
            return Name + " (стаж " + Experience + " лет)";
        }
    }
}