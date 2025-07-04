using System.Windows;
using System.Data.Entity;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Media.Media3D;
using System.Windows.Input;

namespace WpfApp13
{
    /// <summary>
    /// Логика взаимодействия для MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window
    {
        OfficeEntities db = new OfficeEntities();
        private List<Equipment> eqipList;

        public MainWindow()
        {
            InitializeComponent();
            eqipList = db.Equipment
                .Include(e => e.Type)
                .Include(e => e.Status)
                .ToList();
            vivod.ItemsSource = eqipList.Select(p => new
            {
                p.TypeId,
                p.Name,
                Status = p.Status?.Name,
                Type = p.Type?.Name
            }).ToList();
        }

        private void AddBtn_Click(object sender, RoutedEventArgs e)
        {
            new EditWin(null).Show();
            MessageBox.Show("Вы перешли на окно добавления. Для добавления нового материала, пожалуйста, заполните все поля в форме редактирования.");
            Close();
        }

        private void ExitBtn_Click(object sender, RoutedEventArgs e)
        {
            MessageBox.Show("Вы вышли из системы.");
            Close();
        }

        private void DelBtn_Click(object sender, RoutedEventArgs e)
        {
            dynamic selected = vivod.SelectedItem;
            if (selected == null)
            {
                MessageBox.Show("Ошибка! Выберите материал для удаления!");
                return;
            }

            int equipId = selected.Id;

            Equipment equipToDelete = db.Equipment.Find(equipId);

            if (equipToDelete != null)
            {
                db.Equipment.Remove(equipToDelete);
                db.SaveChanges();
                MessageBox.Show("Оборудование успешно удален!");
            }
        }

        private void Vivod_PreviewMouseDoubleClick(object sender, MouseButtonEventArgs e)
        {
            if (vivod.SelectedIndex >= 0 && vivod.SelectedIndex < eqipList.Count)
            {
                Equipment selectedEquipment = eqipList[vivod.SelectedIndex];
                new EditWin(selectedEquipment).Show();
                Close();
            }
        }
    }
}
