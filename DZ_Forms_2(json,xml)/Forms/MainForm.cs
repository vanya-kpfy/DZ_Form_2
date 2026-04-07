using DZ_Forms_2_json_xml_.Classes_Transport;
using DZ_Forms_2_json_xml_.Serialization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.Windows.Forms;

namespace DZ_Forms_2_json_xml_.Forms
{
    /// <summary>
    /// ГЛАВНАЯ ФОРМА
    /// </summary>
    public partial class MainForm : Form
    {
        private TransportData data;
        //private string xmlPath = System.IO.Path.Combine(Application.StartupPath, "transport.xml");
        private string jsonPath = System.IO.Path.Combine(Application.StartupPath, "transport.json");

        public MainForm()
        {
            InitializeComponent();

            LoadPicture();

            // Загружаем данные
            LoadData();

            // Заполняем дерево
            FillTree();

            // Настраиваем таблицу
            SetupDataGridView();

            // Показываем все сущности
            ShowAllEntities();

            // Подписываем события
            treeView.AfterSelect += TreeView_AfterSelect;
            treeView.NodeMouseDoubleClick += TreeView_NodeDoubleClick;
            dataGridView.CellDoubleClick += DataGridView_CellDoubleClick;
            btnSave.Click += BtnSave_Click;
            btnLoad.Click += BtnLoad_Click;
        }

        // Загрузка данных
        private void LoadData()
        {
            data = JsonHelper.LoadFromJson<TransportData>(jsonPath);

            //if (data == null || (data.Buses.Count == 0 && data.Trams.Count == 0 && data.Trolleybuses.Count == 0))
            //{
            //     data = XmlHelper.LoadFromXml<TransportData>(xmlPath);
            //}

            if (data == null || (data.Buses.Count == 0 && data.Trams.Count == 0 && data.Trolleybuses.Count == 0))
            {
                CreateSampleData();
                SaveData();
            }
        }

        // Создание тестовых 
        private void CreateSampleData()
        {
            data = new TransportData();

            data.Buses.Add(new Bus(1, "101", new Driver("Иванов И.И.", 12),
                new Schedule("05:30", "23:00", "06:00", "22:00"), 60));
            data.Buses.Add(new Bus(2, "205", new Driver("Петров П.П.", 8),
                new Schedule("06:00", "22:30", "07:00", "21:30"), 50));

            data.Trams.Add(new Tram(1, "3", new Driver("Сидоров С.С.", 15),
                new Schedule("05:00", "00:00", "05:30", "23:30"), 120));
            data.Trams.Add(new Tram(2, "7", new Driver("Кузнецова А.А.", 6),
                new Schedule("06:00", "23:00", "06:30", "22:30"), 100));

            data.Trolleybuses.Add(new Trolleybus(1, "12", new Driver("Михайлов М.М.", 10),
                new Schedule("05:45", "22:45", "06:15", "22:15"), 80));
            data.Trolleybuses.Add(new Trolleybus(2, "9", new Driver("Егорова Е.Е.", 5),
                new Schedule("06:00", "21:00", "07:00", "20:00"), 70));
        }

        // Сохранение данных
        private void SaveData()
        {
            JsonHelper.SaveToJson(jsonPath, data);
            //XmlHelper.SaveToXml(xmlPath, data);

            MessageBox.Show("Данные сохранены!\nJSON: " + System.IO.Path.GetFullPath(jsonPath) + " Сохранение");
        }
        
        private void LoadPicture()
        {
            // Массив возможных относительных путей
            string[] paths = {
        "Resources/bus.png",        // Вариант 1
        "bus.png",                   // Вариант 2
        "Images/bus.png",            // Вариант 4
        "../../Resources/bus.png",   // Вариант 3
        "../Resources/bus.png"       // Ещё вариант
    };

            bool found = false;

            foreach (string path in paths)
            {
                if (System.IO.File.Exists(path))
                {
                    try
                    {
                        pictureBox.Image = Image.FromFile(path);
                        pictureBox.SizeMode = PictureBoxSizeMode.Zoom;
                        found = true;
                        break;
                    }
                    catch
                    {
                        continue;
                    }
                }
            }

            if (!found)
            {
                pictureBox.Image = SystemIcons.Information.ToBitmap();
                MessageBox.Show("Картинка не найдена! Искал по путям:\n" + string.Join("\n", paths));
            }
        }

        // Заполнение дерева
        private void FillTree()
        {
            if (treeView == null) 
            { 
                return; 
            }

            treeView.Nodes.Clear();

            TreeNode root = new TreeNode("Общественный транспорт");

            TreeNode allNode = new TreeNode("Все сущности");
            allNode.Tag = "all";
            root.Nodes.Add(allNode);

            TreeNode busesNode = new TreeNode("Автобусы (" + data.Buses.Count + ")");
            busesNode.Tag = "buses";
            foreach (var bus in data.Buses)
            {
                busesNode.Nodes.Add(bus.Number + " (ID " + bus.Id + ")");
            }
            root.Nodes.Add(busesNode);

            TreeNode tramsNode = new TreeNode("Трамваи (" + data.Trams.Count + ")");
            tramsNode.Tag = "trams";
            foreach (var tram in data.Trams)
            {
                tramsNode.Nodes.Add(tram.Number + " (ID " + tram.Id + ")");
            }
            root.Nodes.Add(tramsNode);

            TreeNode trolleysNode = new TreeNode("Троллейбусы (" + data.Trolleybuses.Count + ")");
            trolleysNode.Tag = "trolleys";
            foreach (var trolley in data.Trolleybuses)
            {
                trolleysNode.Nodes.Add(trolley.Number + " (ID " + trolley.Id + ")");
            }
            root.Nodes.Add(trolleysNode);

            treeView.Nodes.Add(root);
            treeView.ExpandAll();
        }

        // Настройка таблицы
        private void SetupDataGridView()
        {
            dataGridView.AutoGenerateColumns = false;
            dataGridView.Columns.Clear();
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            { Name = "Type", HeaderText = "Тип транспорта", DataPropertyName = "Type" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            { Name = "Number", HeaderText = "Номер маршрута", DataPropertyName = "Number" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            { Name = "Driver", HeaderText = "Водитель", DataPropertyName = "Driver" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            { Name = "Schedule", HeaderText = "График работы", DataPropertyName = "Schedule" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn()
            { Name = "Capacity", HeaderText = "Вместимость (чел)", DataPropertyName = "Capacity" });
        }

        // Отображение данных
        private void ShowAllEntities()
        {
            List<object> list = new List<object>();
            foreach (var b in data.Buses)
                list.Add(new
                {
                    Type = "Автобус",
                    Number = b.Number,
                    Driver = b.Driver.ToString(),
                    Schedule = b.Schedule.ToString(),
                    Capacity = b.Capacity
                });
            foreach (var t in data.Trams)
                list.Add(new
                {
                    Type = "Трамвай",
                    Number = t.Number,
                    Driver = t.Driver.ToString(),
                    Schedule = t.Schedule.ToString(),
                    Capacity = t.Capacity
                });
            foreach (var t in data.Trolleybuses)
                list.Add(new
                {
                    Type = "Троллейбус",
                    Number = t.Number,
                    Driver = t.Driver.ToString(),
                    Schedule = t.Schedule.ToString(),
                    Capacity = t.Capacity
                });
            dataGridView.DataSource = list;
        }
        // Отдельно Автобусы
        private void ShowBuses()
        {
            List<object> list = new List<object>();
            foreach (var b in data.Buses)
                list.Add(new
                {
                    Type = "Автобус",
                    Number = b.Number,
                    Driver = b.Driver.ToString(),
                    Schedule = b.Schedule.ToString(),
                    Capacity = b.Capacity
                });
            dataGridView.DataSource = list;
        }
        // Отдельно Трамваи
        private void ShowTrams()
        {
            List<object> list = new List<object>();
            foreach (var t in data.Trams)
                list.Add(new
                {
                    Type = "Трамвай",
                    Number = t.Number,
                    Driver = t.Driver.ToString(),
                    Schedule = t.Schedule.ToString(),
                    Capacity = t.Capacity
                });
            dataGridView.DataSource = list;
        }
        // Отдельно Троллейбусы
        private void ShowTrolleybuses()
        {
            List<object> list = new List<object>();
            foreach (var t in data.Trolleybuses)
                list.Add(new
                {
                    Type = "Троллейбус",
                    Number = t.Number,
                    Driver = t.Driver.ToString(),
                    Schedule = t.Schedule.ToString(),
                    Capacity = t.Capacity
                });
            dataGridView.DataSource = list;
        }

        // Обработчики все
        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag == null) return;
            switch (e.Node.Tag.ToString())
            {
                case "all": 
                    ShowAllEntities(); 
                    break;
                case "buses": 
                    ShowBuses(); 
                    break;
                case "trams": 
                    ShowTrams(); 
                    break;
                case "trolleys": 
                    ShowTrolleybuses(); 
                    break;
                default: 
                    ShowAllEntities(); 
                    break;
            }
        }

        private void TreeView_NodeDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            TreeNode node = e.Node;
            if (node.Parent != null && node.Parent.Tag != null)
            {
                var number = node.Text.Split(' ')[1];
                object selected = null;

                if (node.Parent.Tag.ToString() == "buses")
                {
                    selected = data.Buses.Find(b => b.Number == number);
                }
                else if (node.Parent.Tag.ToString() == "trams")
                {
                    selected = data.Trams.Find(t => t.Number == number);
                }
                else if (node.Parent.Tag.ToString() == "trolleys")
                {
                    selected = data.Trolleybuses.Find(t => t.Number == number);
                }

                if (selected != null)
                {
                    DetailsForm details = new DetailsForm(selected);
                    details.ShowDialog();
                }
            }
        }

        private void DataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0)
            {
                return;
            }

            var type = dataGridView.Rows[e.RowIndex].Cells["Type"].Value.ToString();
            var number = dataGridView.Rows[e.RowIndex].Cells["Number"].Value.ToString();
            object selected = null;

            if (type == "Автобус")
            {
                selected = data.Buses.Find(b => b.Number == number);
            }
            else if (type == "Трамвай")
            {
                selected = data.Trams.Find(t => t.Number == number);
            }
            else if (type == "Троллейбус")
            {
                selected = data.Trolleybuses.Find(t => t.Number == number);
            }

            if (selected != null)
            {
                DetailsForm details = new DetailsForm(selected);
                details.ShowDialog();
            }
        }

        private void BtnSave_Click(object sender, EventArgs e)
        {
            SaveData();
        }

        private void BtnLoad_Click(object sender, EventArgs e)
        {
            LoadData();
            FillTree();
            ShowAllEntities();
            MessageBox.Show("Данные перезагружены из файлов!", "Загрузка");
        }
    }
}