using System;
using System.Windows;
using WpfApp13.Interfaces;

namespace WpfApp13
{
    /// <summary>
    /// Логика взаимодействия для EditWin.xaml
    /// </summary>
    public partial class EditWin : Window
    {
        private readonly IEquipmentRepository _repository;
        private Equipment _currentEquip;

        public EditWin(Equipment equip, IEquipmentRepository repository)
        {
            InitializeComponent();
            _repository = repository;

            LoadDataAndInitialize(equip);
        }

        /// <summary>
        /// подгрузка данных для редактирования
        /// </summary>
        /// <param name="equip"></param>
        private async void LoadDataAndInitialize(Equipment equip)
        {
            try
            {
                var types = await _repository.GetTypesAsync();
                var statuses = await _repository.GetStatusesAsync();

                EquioTypeComboBox.ItemsSource = types;
                StatusTypeComboBox.ItemsSource = statuses;

                if (equip != null)
                {
                    _currentEquip = await _repository.GetByIdAsync(equip.Id);
                    if (_currentEquip != null)
                    {
                        NameTextBox.Text = _currentEquip.Name;
                        EquioTypeComboBox.SelectedItem = _currentEquip.Type;
                        StatusTypeComboBox.SelectedItem = _currentEquip.Status;
                    }
                }
                else
                {
                    _currentEquip = new Equipment();
                }
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        /// <summary>
        /// сохранение изменений/добавление нового пользователя
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void SaveBtn_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrWhiteSpace(NameTextBox.Text))
            {
                MessageBox.Show("Ошибка! Имя обязательно к заполнению.");
                return;
            }

            var selectedType = EquioTypeComboBox.SelectedItem as Type;
            var selectedStatus = StatusTypeComboBox.SelectedItem as Status;

            if (selectedType == null || selectedStatus == null)
            {
                MessageBox.Show("Ошибка! Выберите тип и статус из списка.");
                return;
            }

            _currentEquip.Name = NameTextBox.Text;
            _currentEquip.Type = selectedType;
            _currentEquip.TypeId = selectedType.Id;

            _currentEquip.Status = selectedStatus;
            _currentEquip.StatusId = selectedStatus.Id;

            try
            {
                if (_currentEquip.Id == 0)
                    await _repository.AddAsync(_currentEquip);
                else
                    await _repository.UpdateAsync(_currentEquip);

                MessageBox.Show("Отлично! Данные успешно сохранены");
                DialogResult = true;
                Close();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка сохранения: {ex.Message}");
            }
        }

        private void BackBtn_Click(object sender, RoutedEventArgs e)
        {
            DialogResult = false;
            Close();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}