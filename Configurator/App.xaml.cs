using Configurator.Classes;
using System.Windows;

namespace Configurator
{
    public partial class App : Application
    {
        public static bool IsWindows11;

        public App() 
        {
            IsWindows11 = Utils.GetOS().Contains("Windows 11");
        }
    }
}