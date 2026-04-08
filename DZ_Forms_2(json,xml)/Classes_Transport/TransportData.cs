using System.Collections.Generic;

namespace DZ_Forms_2_json_xml_.Classes_Transport
{
    /// <summary>
    /// Контейнер для всех сущностей.
    /// Используется для сериализации в XML/JSON.
    /// </summary>
    [System.Serializable]
    public class TransportData
    {
        // Три списка для трёх типов транспорта 

        /// <summary>
        /// Список автобусов
        /// </summary>
        public List<Bus> Buses { get; set; }
        /// <summary>
        /// Список трамваев
        /// </summary>
        public List<Tram> Trams { get; set; }
        /// <summary>
        /// Список троллейбусов
        /// </summary>
        public List<Trolleybus> Trolleybuses { get; set; }
        public TransportData()
        {
            Buses = new List<Bus>();
            Trams = new List<Tram>();
            Trolleybuses = new List<Trolleybus>();
        }
    }
}