using System;
using System.Linq;
using System.Windows;

namespace WpfApp13
{
    /// <summary>
    /// Логика взаимодействия для EditWin.xaml
    /// </summary>
    public partial class EditWin : Window
    {
        private readonly OfficeEntities db = new OfficeEntities();
        private Equipment currentEquip;
        public EditWin(Equipment equip)
        {
            InitializeComponent();
            EquioTypeComboBox.ItemsSource = db.Type.ToList();
            StatusTypeComboBox.ItemsSource = db.Status.ToList();
            if (equip != null)
            {
                currentEquip = db.Equipment.FirstOrDefault(m => m.Id == equip.Id);
                if (currentEquip != null)
                {
                    NameTextBox.Text = currentEquip.Name;
                    EquioTypeComboBox.SelectedItem = currentEquip.Type;     
                    StatusTypeComboBox.SelectedItem = currentEquip.Status;
                }
            }
        }
        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            new MainWindow().Show();
            MessageBox.Show("Вы вернулись на страницу.");
            Close();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Вы вышли из программы.");
            Close();
        }

        /// <summary>
        /// Валидация данных и сохранение материала в базе данных.
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Ошибка! Единица измерения обязательна к заполнению.");
                return;
            }

            var selectedType = EquioTypeComboBox.SelectedItem as Type;
            if (selectedType == null)
            {
                MessageBox.Show("Ошибка! Выберите тип материала из списка.");
                return;
            }

            var selectedStatus = StatusTypeComboBox.SelectedItem as Status;
            if (selectedType == null)
            {
                MessageBox.Show("Ошибка! Выберите тип статуса из списка.");
                return;
            }

            currentEquip.Name = NameTextBox.Text;
            currentEquip.Type = selectedType;
            currentEquip.TypeId = selectedType.Id;

            currentEquip.Status = selectedStatus;
            currentEquip.StatusId = selectedStatus.Id;
            try
            {
                db.SaveChanges();
                MessageBox.Show("Отлично! Данные успешно сохранены");
                new MainWindow().Show();
                this.Close();
            }
            catch (Exception ex)
            {

                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }
    }
}
