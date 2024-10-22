using Microsoft.Win32;
using System.Diagnostics;
using System.IO;
using System.Net.Http;

namespace Configurator.Classes
{
    public class Utils
    {
        public static readonly string DownloadsFolder = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.UserProfile), "Downloads");
        public static void RunCommand(string command, string arguments)
        {
            ProcessStartInfo startInfo = new()
            {
                FileName = command,
                Arguments = arguments,
                UseShellExecute = false,
                CreateNoWindow = false
            };

            Process process = new() { StartInfo = startInfo };

            process.Start();
            process.WaitForExit();
        }

        public static async Task<bool> DownloadFile(string url, string filename)
        {
            try
            {
                using HttpClient client = new();
                using HttpResponseMessage response = await client.GetAsync(url, HttpCompletionOption.ResponseHeadersRead);

                if (response.IsSuccessStatusCode)
                {
                    using var stream = await response.Content.ReadAsStreamAsync();
                    using var fileStream = new FileStream(filename, FileMode.Create, FileAccess.Write, FileShare.None);
                    await stream.CopyToAsync(fileStream);
                    return true;
                }
                return false;
            }
            catch
            {
                return false;
            }
        }

        public static string GetOS()
        {
            string productName = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "ProductName", "");

            if (productName.Contains("Windows 10"))
            {
                string buildNumber = (string)Registry.GetValue("HKEY_LOCAL_MACHINE\\SOFTWARE\\Microsoft\\Windows NT\\CurrentVersion", "CurrentBuild", "");

                if (Convert.ToInt32(buildNumber) >= 22000)
                {
                    productName = productName.Replace("Windows 10", "Windows 11");
                }
            }

            return productName;
        }

        public static async Task ShowDialog(string title, string content)
        {
            var uiMessageBox = new Wpf.Ui.Controls.MessageBox
            {
                Title = title,
                Content = content,
            };

            _ = await uiMessageBox.ShowDialogAsync();
        }
    }
}