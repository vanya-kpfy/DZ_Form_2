using DZ_Forms_2_json_xml_.Classes_Transport;
using DZ_Forms_2_json_xml_.Serialization;
using System;
using System.Collections.Generic;
using System.Drawing;
using System.IO;
using System.Windows.Forms;

namespace DZ_Forms_2_json_xml_.Forms
{
    public partial class MainForm : Form
    {
        private readonly string dataFolder = "Data";
        private string xmlPath, jsonPath;
        private TransportData data;

        public MainForm()
        {
            InitializeComponent();
            InitializePaths();
            LoadPicture();
            LoadData();
            FillTree();
            SetupDataGridView();
            ShowAllEntities();

            // Подписка событий
            treeView.AfterSelect += TreeView_AfterSelect;
            treeView.NodeMouseDoubleClick += TreeView_NodeDoubleClick;
            dataGridView.CellDoubleClick += DataGridView_CellDoubleClick;
            btnImport.Click += BtnImport_Click;
            btnExport.Click += BtnExport_Click;
            btnLoadSample.Click += BtnLoadSample_Click;
        }

        // ---------- Инициализация путей ----------
        private void InitializePaths()
        {
            var projectPath = Directory.GetParent(Application.StartupPath).Parent.Parent.FullName;
            var dataPath = Path.Combine(projectPath, dataFolder);
            if (!Directory.Exists(dataPath)) Directory.CreateDirectory(dataPath);

            xmlPath = Path.Combine(dataPath, "transport_data.xml");
            jsonPath = Path.Combine(dataPath, "transport_data.json");
        }

        private string GetDataFolderPath() => Path.Combine(Directory.GetParent(Application.StartupPath).Parent.Parent.FullName, dataFolder);

        // ---------- Загрузка / сохранение ----------
        private void LoadData()
        {
            if (File.Exists(jsonPath))
            {
                data = JsonHelper.LoadFromJson<TransportData>(jsonPath);
                if (data?.Buses.Count > 0 || data?.Trams.Count > 0 || data?.Trolleybuses.Count > 0) return;
            }
            CreateSampleData();
            SaveDataToJsonOnly();
        }

        private void SaveDataToJsonOnly() => JsonHelper.SaveToJson(jsonPath, data);

        private void CreateSampleData()
        {
            data = new TransportData
            {
                Buses = {
                    new Bus(1, "101", new Driver("Иванов И.И.", 12), new Schedule("05:30", "23:00", "06:00", "22:00"), 60),
                    new Bus(2, "205", new Driver("Петров П.П.", 8), new Schedule("06:00", "22:30", "07:00", "21:30"), 50)
                },
                Trams = {
                    new Tram(1, "3", new Driver("Сидоров С.С.", 15), new Schedule("05:00", "00:00", "05:30", "23:30"), 120),
                    new Tram(2, "7", new Driver("Кузнецова А.А.", 6), new Schedule("06:00", "23:00", "06:30", "22:30"), 100)
                },
                Trolleybuses = {
                    new Trolleybus(1, "12", new Driver("Михайлов М.М.", 10), new Schedule("05:45", "22:45", "06:15", "22:15"), 80),
                    new Trolleybus(2, "9", new Driver("Егорова Е.Е.", 5), new Schedule("06:00", "21:00", "07:00", "20:00"), 70)
                }
            };
        }

        // ---------- Картинка ----------
        private void LoadPicture()
        {
            var paths = new[] { "Resources/bus.png", "bus.png", "../../Resources/bus.png", Path.Combine(GetDataFolderPath(), "bus.png") };
            foreach (var path in paths)
            {
                if (File.Exists(path))
                {
                    try { pictureBox.Image = Image.FromFile(path); pictureBox.SizeMode = PictureBoxSizeMode.Zoom; return; }
                    catch { }
                }
            }
            pictureBox.Image = SystemIcons.Information.ToBitmap();
        }

        // ---------- Обновление интерфейса ----------
        private void FillTree()
        {
            if (treeView == null || data == null) return;
            treeView.Nodes.Clear();

            var root = new TreeNode("Общественный транспорт");
            root.Nodes.Add(new TreeNode("Все сущности") { Tag = "all" });
            root.Nodes.Add(new TreeNode($"Автобусы ({data.Buses.Count})") { Tag = "buses" });
            root.Nodes.Add(new TreeNode($"Трамваи ({data.Trams.Count})") { Tag = "trams" });
            root.Nodes.Add(new TreeNode($"Троллейбусы ({data.Trolleybuses.Count})") { Tag = "trolleys" });

            foreach (var bus in data.Buses)
                root.Nodes["buses"]?.Nodes.Add($"{bus.Number} (ID {bus.Id})");
            foreach (var tram in data.Trams)
                root.Nodes["trams"]?.Nodes.Add($"{tram.Number} (ID {tram.Id})");
            foreach (var trolley in data.Trolleybuses)
                root.Nodes["trolleys"]?.Nodes.Add($"{trolley.Number} (ID {trolley.Id})");

            treeView.Nodes.Add(root);
            treeView.ExpandAll();
        }

        private void SetupDataGridView()
        {
            dataGridView.AutoGenerateColumns = false;
            dataGridView.Columns.Clear();
            dataGridView.AutoSizeColumnsMode = DataGridViewAutoSizeColumnsMode.Fill;

            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Type", HeaderText = "Тип транспорта", DataPropertyName = "Type" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Number", HeaderText = "Номер маршрута", DataPropertyName = "Number" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Driver", HeaderText = "Водитель", DataPropertyName = "Driver" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Schedule", HeaderText = "График работы", DataPropertyName = "Schedule" });
            dataGridView.Columns.Add(new DataGridViewTextBoxColumn { Name = "Capacity", HeaderText = "Вместимость (чел)", DataPropertyName = "Capacity" });
        }

        private void ShowAllEntities()
        {
            if (data == null) return;
            var list = new List<object>();

            foreach (var b in data.Buses)
                list.Add(new { Type = "Автобус", b.Number, Driver = b.Driver.ToString(), Schedule = b.Schedule.ToString(), b.Capacity });
            foreach (var t in data.Trams)
                list.Add(new { Type = "Трамвай", t.Number, Driver = t.Driver.ToString(), Schedule = t.Schedule.ToString(), t.Capacity });
            foreach (var t in data.Trolleybuses)
                list.Add(new { Type = "Троллейбус", t.Number, Driver = t.Driver.ToString(), Schedule = t.Schedule.ToString(), t.Capacity });

            // сбрасываем источник и обновляем
            dataGridView.DataSource = null;
            dataGridView.DataSource = list;
            dataGridView.Refresh();
        }

        private void ShowBuses() => ShowFiltered("Автобус", data.Buses);
        private void ShowTrams() => ShowFiltered("Трамвай", data.Trams);
        private void ShowTrolleybuses() => ShowFiltered("Троллейбус", data.Trolleybuses);

        private void ShowFiltered<T>(string type, IEnumerable<T> items) where T : class
        {
            var list = new List<object>();
            foreach (var item in items)
            {
                dynamic d = item;
                list.Add(new { Type = type, Number = d.Number, Driver = d.Driver.ToString(), Schedule = d.Schedule.ToString(), Capacity = d.Capacity });
            }
            dataGridView.DataSource = null;
            dataGridView.DataSource = list;
            dataGridView.Refresh();
        }

        // ---------- Обработчики ----------
        private void TreeView_AfterSelect(object sender, TreeViewEventArgs e)
        {
            if (e.Node.Tag == null) return;
            switch (e.Node.Tag.ToString())
            {
                case "all": ShowAllEntities(); break;
                case "buses": ShowBuses(); break;
                case "trams": ShowTrams(); break;
                case "trolleys": ShowTrolleybuses(); break;
            }
        }

        private void TreeView_NodeDoubleClick(object sender, TreeNodeMouseClickEventArgs e)
        {
            var node = e.Node;
            if (node.Parent?.Tag == null) return;
            var number = node.Text.Split(' ')[0];
            object selected = null;

            switch (node.Parent.Tag.ToString())
            {
                case "buses": selected = data.Buses.Find(b => b.Number == number); break;
                case "trams": selected = data.Trams.Find(t => t.Number == number); break;
                case "trolleys": selected = data.Trolleybuses.Find(t => t.Number == number); break;
            }
            if (selected != null) new DetailsForm(selected).ShowDialog();
        }

        private void DataGridView_CellDoubleClick(object sender, DataGridViewCellEventArgs e)
        {
            if (e.RowIndex < 0) return;
            var type = dataGridView.Rows[e.RowIndex].Cells["Type"].Value.ToString();
            var number = dataGridView.Rows[e.RowIndex].Cells["Number"].Value.ToString();
            object selected = null;

            if (type == "Автобус") selected = data.Buses.Find(b => b.Number == number);
            else if (type == "Трамвай") selected = data.Trams.Find(t => t.Number == number);
            else if (type == "Троллейбус") selected = data.Trolleybuses.Find(t => t.Number == number);

            if (selected != null) new DetailsForm(selected).ShowDialog();
        }

        private void BtnImport_Click(object sender, EventArgs e)
        {
            using (OpenFileDialog openFileDialog = new OpenFileDialog())
            {
                openFileDialog.Title = "Выберите файл с данными";
                openFileDialog.Filter = "JSON файлы (*.json)|*.json|XML файлы (*.xml)|*.xml|Все файлы (*.*)|*.*";
                openFileDialog.InitialDirectory = GetDataFolderPath();

                if (openFileDialog.ShowDialog() == DialogResult.OK)
                {
                    string filePath = openFileDialog.FileName;
                    TransportData importedData = null;

                    try
                    {
                        if (filePath.EndsWith(".json"))
                        {
                            importedData = JsonHelper.LoadFromJson<TransportData>(filePath);
                        }
                        else if (filePath.EndsWith(".xml"))
                        {
                            importedData = XmlHelper.LoadFromXml<TransportData>(filePath);
                        }
                        else
                        {
                            MessageBox.Show("Неподдерживаемый формат файла!", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        if (importedData == null)
                        {
                            MessageBox.Show("Не удалось загрузить данные из файла!", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // Проверяем, что данные есть
                        if (importedData.Buses.Count == 0 && importedData.Trams.Count == 0 && importedData.Trolleybuses.Count == 0)
                        {
                            MessageBox.Show("Файл не содержит данных!", "Ошибка",
                                MessageBoxButtons.OK, MessageBoxIcon.Error);
                            return;
                        }

                        // ПЕРЕЗАПИСЫВАЕМ данные
                        data = importedData;

                        // Сохраняем в JSON для автозагрузки
                        SaveDataToJsonOnly();

                        // ОБНОВЛЯЕМ интерфейс
                        FillTree();
                        ShowAllEntities();

                        MessageBox.Show($"Импорт выполнен!\n\n" +
                            $"Автобусов: {data.Buses.Count}\n" +
                            $"Трамваев: {data.Trams.Count}\n" +
                            $"Троллейбусов: {data.Trolleybuses.Count}",
                            "Успех", MessageBoxButtons.OK, MessageBoxIcon.Information);
                    }
                    catch (Exception ex)
                    {
                        MessageBox.Show($"Ошибка при импорте: {ex.Message}",
                            "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    }
                }
            }
        }

        private void BtnExport_Click(object sender, EventArgs e)
        {
            try
            {
                var folder = GetDataFolderPath();
                var stamp = DateTime.Now.ToString("yyyyMMdd_HHmmss");
                var jsonPath = Path.Combine(folder, $"transport_export_{stamp}.json");
                var xmlPath = Path.Combine(folder, $"transport_export_{stamp}.xml");

                JsonHelper.SaveToJson(jsonPath, data);
                XmlHelper.SaveToXml(xmlPath, data);

                MessageBox.Show($"Экспорт выполнен!\nJSON: {jsonPath}\nXML: {xmlPath}", "Экспорт",
                    MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка экспорта: {ex.Message}", "Ошибка", MessageBoxButtons.OK, MessageBoxIcon.Error);
            }
        }

        private void BtnLoadSample_Click(object sender, EventArgs e)
        {
            if (MessageBox.Show("Загрузить тестовые данные? Текущие будут потеряны!", "Подтверждение",
                MessageBoxButtons.YesNo, MessageBoxIcon.Question) == DialogResult.Yes)
            {
                CreateSampleData();
                FillTree();
                ShowAllEntities();
                SaveDataToJsonOnly();
                MessageBox.Show("Тестовые данные загружены!", "Пример", MessageBoxButtons.OK, MessageBoxIcon.Information);
            }
        }
    }
}