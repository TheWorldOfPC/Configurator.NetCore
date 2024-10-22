using Configurator.Classes;
using System.Diagnostics;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Threading;

namespace Configurator.Pages
{
    public partial class DriversPage : Page
    {
        private int dotCount = 0;
        private string status = string.Empty;
        DispatcherTimer timer = new()
        {
            Interval = TimeSpan.FromSeconds(0.5)
        };

        public DriversPage()
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

        private async Task Download(string appName, string url, string filename)
        {
            DownloadStarted(appName);
            bool downloadSuccess = await Utils.DownloadFile(url, filename);
            DownloadFinished();
            if (downloadSuccess) Process.Start("explorer.exe", $"/select, \"{filename}\"");
            else Utils.ShowDialog("Configurator", $"{appName} Failed to download.");
        }

        private async void btnDriverBooster_Click(object sender, RoutedEventArgs e)
        {
            string url = "https://files.catbox.moe/akz02w.bin";
            string filename = $"{Utils.DownloadsFolder}\\IObit Driver Booster Pro.zip";
            await Download("Driver Booster", url, filename);
        }

        private async void btnSDI_Click(object sender, RoutedEventArgs e)
        {
            string url = "https://nexus-toolkit.epubg691.workers.dev/?file=/Update%20Packages/SDI.zip";
            string filename = $"{Utils.DownloadsFolder}\\SnappyDriverInstaller.zip";
            await Download("Snappy Driver Installer", url, filename);
        }

        private async void btnNVCleanstall_Click(object sender, RoutedEventArgs e)
        {
            string url = "https://nexus-toolkit.epubg691.workers.dev/?file=/Update%20Packages/NVCleanstall_1.16.0.exe";
            string filename = $"{Utils.DownloadsFolder}\\NVCleanstall_1.16.0.exe";
            await Download("NVCleanstall", url, filename);
        }

        private async void btnNVSlimmer_Click(object sender, RoutedEventArgs e)
        {
            string url = "https://nexus-toolkit.epubg691.workers.dev/?file=/Update%20Packages/NVSlimmer_v0.13.zip";
            string filename = $"{Utils.DownloadsFolder}\\NVSlimmer_v0.13.zip";
            await Download("NVSlimmer", url, filename);
        }

        private async void btnRadeonSlimmer_Click(object sender, RoutedEventArgs e)
        {
            string url = "https://nexus-toolkit.epubg691.workers.dev/?file=/Update%20Packages/RadeonSoftwareSlimmer_1.11.0.zip";
            string filename = $"{Utils.DownloadsFolder}\\RadeonSoftwareSlimmer_1.11.0.zip";
            await Download("Radeon Software Slimmer", url, filename);
        }
    }
}