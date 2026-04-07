using System;

namespace DZ_Forms_2_json_xml_.Classes_Transport
{
    /// <summary>
    /// Сущность "Трамвай"
    /// </summary>
    [Serializable]
    public class Tram
    {
        /// <summary>
        /// Уникальный ID
        /// </summary>
        public int Id { get; set; }
        /// <summary>
        /// Номер маршрута
        /// </summary>
        public string Number { get; set; }
        /// <summary>
        /// Вложенный объект №1
        /// </summary>
        public Driver Driver { get; set; }
        /// <summary>
        /// Вложенный объект №2
        /// </summary>
        public Schedule Schedule { get; set; }
        /// <summary>
        /// Вместимость
        /// </summary>
        public int Capacity { get; set; }
        public Tram() { }
        public Tram(int id, string number, Driver driver, Schedule schedule, int capacity)
        {
            Id = id;
            Number = number;
            Driver = driver;
            Schedule = schedule;
            Capacity = capacity;
        }
    }
}