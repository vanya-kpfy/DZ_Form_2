using System;

namespace DZ_Forms_2_json_xml_.Classes_Transport
{
    /// <summary>
    /// Класс "Расписание"
    /// </summary>
    [Serializable]
    public class Schedule
    {
        // Время начала и конца работы в будни и выходные
        /// <summary>
        /// Начало в будни
        /// </summary>
        public string WeekdaysStart { get; set; }
        /// <summary>
        /// Конец в будни
        /// </summary>
        public string WeekdaysEnd { get; set; }
        /// <summary>
        /// Начало в выходные
        /// </summary>
        public string WeekendStart { get; set; }
        /// <summary>
        /// Конец в выходные
        /// </summary>
        public string WeekendEnd { get; set; }
        public Schedule() { }
        public Schedule(string wdStart, string wdEnd, string weStart, string weEnd)
        {
            WeekdaysStart = wdStart;
            WeekdaysEnd = wdEnd;
            WeekendStart = weStart;
            WeekendEnd = weEnd;
        }

        public override string ToString()
        {
            return "Будни: " + WeekdaysStart + "-" + WeekdaysEnd + ", Выходные: " + WeekendStart + "-" + WeekendEnd;
        }
    }
}