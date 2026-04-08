using DZ_Forms_2_json_xml_.Classes_Transport;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DZ_Forms_2_json_xml_.Forms
{
    /// <summary>
    /// ВТОРАЯ ФОРМА
    /// Показывает подробную информацию о выбранной сущности
    /// </summary>
    public partial class DetailsForm : Form
    {
        private object entity;  // Выбранный транспорт (автобус/трамвай/троллейбус)
        private Label labelInfo; // Метка для текста
        private PictureBox pictureBox; // Картинка
        private Panel panelButtons; // Панель для кнопок
        private Button btnClose; // Кнопка закрытия
        private Button btnCopy; // Кнопка копирования

        public DetailsForm(object ent)
        {
            entity = ent;

            // Настройки формы
            this.Text = "ℹ️ Подробная информация о транспорте";
            this.Size = new Size(550, 500);
            this.StartPosition = FormStartPosition.CenterParent;  // Центрируем относительно главной
            this.MinimumSize = new Size(500, 400);
            this.MaximumSize = new Size(600, 700);
            this.BackColor = Color.White;
            this.FormBorderStyle = FormBorderStyle.FixedDialog; // Фиксированный размер
            this.MaximizeBox = false; // Запрещаем разворачивание

            // Создаём элементы
            CreateControls();
        }

        private void CreateControls()
        {
            // Картинка 
            pictureBox = new PictureBox();
            pictureBox.Dock = DockStyle.Top;
            pictureBox.Height = 120;
            pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
            pictureBox.BackColor = Color.FromArgb(240, 240, 240);

            // Выбираем иконку в зависимости от типа транспорта
            SetPictureByType();

            // Текстовая метка
            labelInfo = new Label();
            labelInfo.Dock = DockStyle.Fill;
            labelInfo.Font = new Font("Consolas", 11); // Моноширинный шрифт для красивого форматирования
            labelInfo.Padding = new Padding(20, 20, 20, 20);
            labelInfo.Text = GetDetailsText();
            labelInfo.BackColor = Color.White;

            // Панель с кнопками
            panelButtons = new Panel();
            panelButtons.Dock = DockStyle.Bottom;
            panelButtons.Height = 60;
            panelButtons.BackColor = Color.FromArgb(240, 240, 240);

            // Кнопка "Закрыть"
            btnClose = new Button();
            btnClose.Text = "✖ Закрыть";
            btnClose.Size = new Size(120, 40);
            btnClose.Location = new Point(panelButtons.Width - 140, 10);
            btnClose.Anchor = AnchorStyles.Top | AnchorStyles.Right;
            btnClose.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnClose.BackColor = Color.LightCoral;
            btnClose.FlatStyle = FlatStyle.Flat;
            btnClose.Click += (s, e) => this.Close();

            // Кнопка "Копировать"
            btnCopy = new Button();
            btnCopy.Text = "📋 Копировать";
            btnCopy.Size = new Size(120, 40);
            btnCopy.Location = new Point(20, 10);
            btnCopy.Anchor = AnchorStyles.Top | AnchorStyles.Left;
            btnCopy.Font = new Font("Segoe UI", 10, FontStyle.Bold);
            btnCopy.BackColor = Color.LightBlue;
            btnCopy.FlatStyle = FlatStyle.Flat;
            btnCopy.Click += BtnCopy_Click;

            // Добавляем кнопки на панель
            panelButtons.Controls.Add(btnClose);
            panelButtons.Controls.Add(btnCopy);

            // Собираем форму, не решил делать форму так, хочу попробовать вручную через код
            this.Controls.Add(labelInfo);
            this.Controls.Add(pictureBox);
            this.Controls.Add(panelButtons);
        }

        /// <summary>
        /// Устанавливает картинку в зависимости от типа транспорта
        /// </summary>
        private void SetPictureByType()
        {
            try
            {
                // Путь к папке Resources (иконки в формате .ico)
                string resourcesPath = Application.StartupPath + "\\Resources\\";

                if (entity is Bus)
                {
                    // Загружаем иконку автобуса из .ico файла
                    if (System.IO.File.Exists(resourcesPath + "bus.ico"))
                    {
                        pictureBox.Image = Icon.ExtractAssociatedIcon(resourcesPath + "bus.ico").ToBitmap();
                        // ИЛИ так:
                        // using (var icon = new Icon(resourcesPath + "bus.ico"))
                        // {
                        //     pictureBox.Image = icon.ToBitmap();
                        // }
                    }
                    else if (System.IO.File.Exists("bus.ico"))
                    {
                        using (var icon = new Icon("bus.ico"))
                        {
                            pictureBox.Image = icon.ToBitmap();
                        }
                    }
                    else
                    {
                        pictureBox.Image = SystemIcons.Information.ToBitmap();
                    }
                    pictureBox.BackColor = Color.FromArgb(255, 240, 240);
                }
                else if (entity is Tram)
                {
                    if (System.IO.File.Exists(resourcesPath + "tram.ico"))
                    {
                        using (var icon = new Icon(resourcesPath + "tram.ico"))
                        {
                            pictureBox.Image = icon.ToBitmap();
                        }
                    }
                    else
                    {
                        pictureBox.Image = SystemIcons.Information.ToBitmap();
                    }
                    pictureBox.BackColor = Color.FromArgb(240, 255, 240);
                }
                else if (entity is Trolleybus)
                {
                    if (System.IO.File.Exists(resourcesPath + "trolley.ico"))
                    {
                        using (var icon = new Icon(resourcesPath + "trolley.ico"))
                        {
                            pictureBox.Image = icon.ToBitmap();
                        }
                    }
                    else
                    {
                        pictureBox.Image = SystemIcons.Information.ToBitmap();
                    }
                    pictureBox.BackColor = Color.FromArgb(240, 240, 255);
                }
                else
                {
                    pictureBox.Image = SystemIcons.Information.ToBitmap();
                }
            }
            catch (Exception ex)
            {
                // Если ошибка - показываем стандартную иконку
                pictureBox.Image = SystemIcons.Information.ToBitmap();
                MessageBox.Show("Ошибка загрузки иконки: " + ex.Message);
            }
        }

        /// <summary>
        /// Формирует текст с детальной информацией
        /// </summary>
        private string GetDetailsText()
        {
            if (entity is Bus bus)
            {
                return
                    "═══════════════════════════════════════════════════════════\n" +
                    "                         АВТОБУС                           \n" +
                    "═══════════════════════════════════════════════════════════\n\n" +
                    "  ОСНОВНАЯ ИНФОРМАЦИЯ:\n" +
                    "  ┌─────────────────────────────────────────────────────┐\n" +
                    "  │ ID транспортного средства: " + bus.Id.ToString().PadRight(33) + "│\n" +
                    "  │ Номер маршрута:           " + bus.Number.PadRight(33) + "│\n" +
                    "  │ Вместимость:              " + bus.Capacity + " мест".PadRight(33) + "│\n" +
                    "  └─────────────────────────────────────────────────────┘\n\n" +
                    "  ВОДИТЕЛЬ:\n" +
                    "  ┌─────────────────────────────────────────────────────┐\n" +
                    "  │ ФИО:                    " + bus.Driver.Name.PadRight(33) + "│\n" +
                    "  │ Стаж работы:            " + bus.Driver.Experience + " лет".PadRight(33) + "│\n" +
                    "  └─────────────────────────────────────────────────────┘\n\n" +
                    "  ГРАФИК РАБОТЫ:\n" +
                    "  ┌─────────────────────────────────────────────────────┐\n" +
                    "  │ Будни:                  " + bus.Schedule.WeekdaysStart + " - " + bus.Schedule.WeekdaysEnd + "".PadRight(25) + "│\n" +
                    "  │ Выходные:               " + bus.Schedule.WeekendStart + " - " + bus.Schedule.WeekendEnd + "".PadRight(25) + "│\n" +
                    "  └─────────────────────────────────────────────────────┘\n\n" +
                    "═══════════════════════════════════════════════════════════\n" +
                    "   Двойной клик по строке в таблице закрывает это окно   \n" +
                    "═══════════════════════════════════════════════════════════";
            }
            else if (entity is Tram tram)
            {
                return
                    "═══════════════════════════════════════════════════════════\n" +
                    "                         ТРАМВАЙ                       \n" +
                    "═══════════════════════════════════════════════════════════\n\n" +
                    "  ОСНОВНАЯ ИНФОРМАЦИЯ:\n" +
                    "  ┌─────────────────────────────────────────────────────┐\n" +
                    "  │ ID транспортного средства: " + tram.Id.ToString().PadRight(33) + "│\n" +
                    "  │ Номер маршрута:           " + tram.Number.PadRight(33) + "│\n" +
                    "  │ Вместимость:              " + tram.Capacity + " мест".PadRight(33) + "│\n" +
                    "  └─────────────────────────────────────────────────────┘\n\n" +
                    "  ВОДИТЕЛЬ:\n" +
                    "  ┌─────────────────────────────────────────────────────┐\n" +
                    "  │ ФИО:                    " + tram.Driver.Name.PadRight(33) + "│\n" +
                    "  │ Стаж работы:            " + tram.Driver.Experience + " лет".PadRight(33) + "│\n" +
                    "  └─────────────────────────────────────────────────────┘\n\n" +
                    "  ГРАФИК РАБОТЫ:\n" +
                    "  ┌─────────────────────────────────────────────────────┐\n" +
                    "  │ Будни:                  " + tram.Schedule.WeekdaysStart + " - " + tram.Schedule.WeekdaysEnd + "".PadRight(25) + "│\n" +
                    "  │ Выходные:               " + tram.Schedule.WeekendStart + " - " + tram.Schedule.WeekendEnd + "".PadRight(25) + "│\n" +
                    "  └─────────────────────────────────────────────────────┘\n\n" +
                    "  ОСОБЕННОСТИ:\n" +
                    "  ┌─────────────────────────────────────────────────────┐\n" +
                    "  │ • Передвигается по рельсам                           │\n" +
                    "  │ • Питание от контактной сети                        │\n" +
                    "  │ • Высокая пассажировместимость                      │\n" +
                    "  └─────────────────────────────────────────────────────┘\n\n" +
                    "═══════════════════════════════════════════════════════════";
            }
            else if (entity is Trolleybus trolley)
            {
                return
                    "═══════════════════════════════════════════════════════════\n" +
                    "                        ТРОЛЛЕЙБУС                         \n" +
                    "═══════════════════════════════════════════════════════════\n\n" +
                    "  ОСНОВНАЯ ИНФОРМАЦИЯ:\n" +
                    "  ┌─────────────────────────────────────────────────────┐\n" +
                    "  │ ID транспортного средства: " + trolley.Id.ToString().PadRight(33) + "│\n" +
                    "  │ Номер маршрута:           " + trolley.Number.PadRight(33) + "│\n" +
                    "  │ Вместимость:              " + trolley.Capacity + " мест".PadRight(33) + "│\n" +
                    "  └─────────────────────────────────────────────────────┘\n\n" +
                    "  ВОДИТЕЛЬ:\n" +
                    "  ┌─────────────────────────────────────────────────────┐\n" +
                    "  │ ФИО:                    " + trolley.Driver.Name.PadRight(33) + "│\n" +
                    "  │ Стаж работы:            " + trolley.Driver.Experience + " лет".PadRight(33) + "│\n" +
                    "  └─────────────────────────────────────────────────────┘\n\n" +
                    "  ГРАФИК РАБОТЫ:\n" +
                    "  ┌─────────────────────────────────────────────────────┐\n" +
                    "  │ Будни:                  " + trolley.Schedule.WeekdaysStart + " - " + trolley.Schedule.WeekdaysEnd + "".PadRight(25) + "│\n" +
                    "  │ Выходные:               " + trolley.Schedule.WeekendStart + " - " + trolley.Schedule.WeekendEnd + "".PadRight(25) + "│\n" +
                    "  └─────────────────────────────────────────────────────┘\n\n" +
                    "  ОСОБЕННОСТИ:\n" +
                    "  ┌─────────────────────────────────────────────────────┐\n" +
                    "  │ • Экологически чистый транспорт                     │\n" +
                    "  │ • Питание от контактной сети                        │\n" +
                    "  │ • Плавный ход и низкий уровень шума                │\n" +
                    "  └─────────────────────────────────────────────────────┘\n\n" +
                    "═══════════════════════════════════════════════════════════";
            }
            else
            {
                return "Информация недоступна";
            }
        }

        /// <summary>
        /// Копирует текст в буфер обмена
        /// </summary>
        private void BtnCopy_Click(object sender, EventArgs e)
        {
            try
            {
                // Копируем текст из labelInfo в буфер обмена
                Clipboard.SetText(labelInfo.Text);

                // Показываем уведомление, что скопировано
                MessageBox.Show("Информация скопирована в буфер обмена!","Копирование",MessageBoxButtons.OK,MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show("Ошибка при копировании: " + ex.Message,"Ошибка",MessageBoxButtons.OK,MessageBoxIcon.Error);
            }
        }

        /// <summary>
        /// Обработчик изменения размера формы (для правильного позиционирования кнопок)
        /// </summary>
        protected override void OnResize(EventArgs e)
        {
            base.OnResize(e);

            // Перемещаем кнопку "Закрыть" при изменении размера
            if (btnClose != null && panelButtons != null)
            {
                btnClose.Location = new Point(panelButtons.Width - 140, 10);
            }
        }
    }
}
