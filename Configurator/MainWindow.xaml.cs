using System.Windows;
using Wpf.Ui.Appearance;
using Configurator.Pages;

namespace Configurator
{
    public partial class MainWindow
    {
        public MainWindow()
        {
            DataContext = this;

            InitializeComponent();

            Loaded += (_, _) =>
            {
                RootNavigation.Navigate(typeof(Page1));
                SystemThemeWatcher.Watch(this, Wpf.Ui.Controls.WindowBackdropType.Mica, true);
            };
        }

        private void componentsBtn_Click(object sender, RoutedEventArgs e)
        {
            DashboardPage.Visibility = Visibility.Collapsed;
            RootNavigation.Navigate(typeof(ComponentsPage));
        }

        private void browsersBtn_Click(object sender, RoutedEventArgs e)
        {
            DashboardPage.Visibility = Visibility.Collapsed;
            RootNavigation.Navigate(typeof(BrowsersPage));
        }

        private void updatesBtn_Click(object sender, RoutedEventArgs e)
        {
            // Removed :P
        }

        private void dashboardNavBtn_Click(object sender, RoutedEventArgs e) => DashboardPage.Visibility = Visibility.Visible;

        private void componentsNavBtn_Click(object sender, RoutedEventArgs e) => DashboardPage.Visibility = Visibility.Collapsed;

        private void browsersNavBtn_Click(object sender, RoutedEventArgs e) => DashboardPage.Visibility = Visibility.Collapsed;

        private void driversNavBtn_Click(object sender, RoutedEventArgs e) => DashboardPage.Visibility = Visibility.Collapsed;

        private void updatesNavBtn_Click(object sender, RoutedEventArgs e) => DashboardPage.Visibility = Visibility.Collapsed;

        private void aboutNavBtn_Click(object sender, RoutedEventArgs e) => DashboardPage.Visibility = Visibility.Collapsed;
    }
}