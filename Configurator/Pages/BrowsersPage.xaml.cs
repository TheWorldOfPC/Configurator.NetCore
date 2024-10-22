using Configurator.Classes;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Configurator.Pages
{
    public partial class BrowsersPage : Page
    {
        private int dotCount = 0;
        private string status = string.Empty;
        DispatcherTimer timer = new()
        {
            Interval = TimeSpan.FromSeconds(0.5)
        };

        public BrowsersPage()
        {
            InitializeComponent();
            timer.Tick += Timer_Tick;
        }

        private void Timer_Tick(object sender, EventArgs e)
        {
            txtBlockDownload.Text = status + new string('.', dotCount);
            dotCount = (dotCount + 1) % 5;
        }

        private void DownloadStarted(string appName)
        {
            status = $"Downloading {appName}";
            txtBlockDownload.Text = status;
            txtBlockDownload.Visibility = Visibility.Visible;
            progBarDownload.Visibility = Visibility.Visible;
            timer.Start();
        }

        private void DownloadFinished()
        {
            dotCount = 0;
            status = string.Empty;
            txtBlockDownload.Text = status;
            txtBlockDownload.Visibility = Visibility.Collapsed;
            progBarDownload.Visibility = Visibility.Collapsed;
            timer.Stop();
        }

        private async Task DownloadAndInstall(string appName, string url, string filename)
        {
            DownloadStarted(appName);
            bool downloadSuccess = await Utils.DownloadFile(url, filename);
            DownloadFinished();
            if (downloadSuccess) Process.Start(filename);
            else Utils.ShowDialog("Configurator", $"{appName} Failed to download.");
        }

        private async void btnBrave_Click(object sender, RoutedEventArgs e)
        {
            string url = "https://brave-browser-downloads.s3.brave.com/latest/BraveBrowserSetup.exe";
            string filename = $"{Utils.DownloadsFolder}\\BraveBrowserSetup.exe";
            await DownloadAndInstall("Brave", url, filename);
        }

        private async void btnChrome_Click(object sender, RoutedEventArgs e)
        {
            string url = "http://dl.google.com/chrome/install/chrome_installer.exe";
            string filename = $"{Utils.DownloadsFolder}\\ChromeInstaller.exe";
            await DownloadAndInstall("Chrome", url, filename);
        }

        private async void btnFirefox_Click(object sender, RoutedEventArgs e)
        {
            string url = "https://mzl.la/3o6YriV";
            string filename = $"{Utils.DownloadsFolder}\\MozillaFirefoxSetup.exe";
            await DownloadAndInstall("Firefox", url, filename);
        }

        private async void btnLibreWolf_Click(object sender, RoutedEventArgs e)
        {
            string url = "https://nexus-toolkit.epubg691.workers.dev/?file=/Update%20Packages/librewolf.exe";
            string filename = $"{Utils.DownloadsFolder}\\LibreWolfSetup.exe";
            await DownloadAndInstall("LibreWolf", url, filename);
        }

        private async void btnEdge_Click(object sender, RoutedEventArgs e)
        {
            string url = "https://nexus-toolkit.epubg691.workers.dev/?file=/Update%20Packages/MicrosoftEdgeSetup.exe";
            string filename = $"{Utils.DownloadsFolder}\\MicrosoftEdgeSetup.exe";
            await DownloadAndInstall("Microsoft Edge", url, filename);
        }

        private async void btnUGChrome_Click(object sender, RoutedEventArgs e)
        {
            string url = "https://nexus-toolkit.epubg691.workers.dev/?file=/Update%20Packages/ungoogled-chromium.exe";
            string filename = $"{Utils.DownloadsFolder}\\ungoogled-chromium.exe";
            await DownloadAndInstall("Ungoogled Chromium", url, filename);
        }
    }
}