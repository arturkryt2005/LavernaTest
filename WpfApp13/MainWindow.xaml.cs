using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using System.Windows;
using WpfApp13.Interfaces;

namespace WpfApp13
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        private readonly IEquipmentRepository _repository;
        private List<Equipment> _equipmentList;

        public MainWindow(IEquipmentRepository repository)
        {
            InitializeComponent();
            _repository = repository;
            LoadDataAsync();
        }

        private async Task LoadDataAsync()
        {
            try
            {
                _equipmentList = await _repository.GetAllAsync();

                vivod.ItemsSource = _equipmentList.Select(p => new
                {
                    p.Id,
                    p.Name,
                    Status = p.Status?.Name,
                    Type = p.Type?.Name
                }).ToList();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка загрузки данных: {ex.Message}");
            }
        }

        private async void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            var editWindow = new EditWin(null, _repository);
            editWindow.ShowDialog(); 
            await RefreshDataAsync();
        }

        /// <summary>
        /// метод удаления обьекта
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void DelBtn_Click(object sender, RoutedEventArgs e)
        {
            dynamic selected = vivod.SelectedItem;
            if (selected == null)
            {
                MessageBox.Show("Ошибка! Выберите оборудование для удаления!");
                return;
            }

            int equipId = selected.Id;

            try
            {
                await _repository.DeleteAsync(equipId);
                await RefreshDataAsync();
                MessageBox.Show("Оборудование успешно удалено.");
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Ошибка удаления: {ex.Message}");
            }
        }
        /// <summary>
        /// переход к старниццу редактирования при двойном нажатии на обьект
        /// </summary>
        /// <param name="sender"></param>
        /// <param name="e"></param>
        private async void Vivod_PreviewMouseDoubleClick(object sender, System.Windows.Input.MouseButtonEventArgs e)
        {
            dynamic selected = vivod.SelectedItem;
            if (selected != null)
            {
                int id = selected.Id;
                var equipment = await _repository.GetByIdAsync(id);

                if (equipment != null)
                {
                    var editWindow = new EditWin(equipment, _repository);
                    editWindow.ShowDialog();
                    await RefreshDataAsync();
                }
            }
        }
        
        /// <summary>
        /// метод для обновления списка обьектов
        /// </summary>
        /// <returns></returns>
        private async Task RefreshDataAsync()
        {
            await LoadDataAsync();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            Close();
        }
    }
}