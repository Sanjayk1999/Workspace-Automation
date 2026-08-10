using System.Configuration;
using System.Data;
using System.Windows;
using WorkspaceAuto.ViewModels;

namespace WorkspaceAuto
{
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application
    {
        protected override async void OnStartup(StartupEventArgs e)
        {
            base.OnStartup(e);
            var vm = new MainWindowViewModel();

            await vm.ShowDashboardAsync();

            var window = new MainWindow
            {
                DataContext = vm
            };

            window.Show();
        }
    }

}
