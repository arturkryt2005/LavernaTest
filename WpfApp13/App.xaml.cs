using System.Windows;

namespace WpfApp13
{
    /// <summary>
    /// Логика взаимодействия для App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);

            var repository = new Services.EfEquipmentRepository();
            var main = new MainWindow(repository);
            main.Show();
        }
    }
}
