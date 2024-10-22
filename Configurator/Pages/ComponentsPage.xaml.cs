using Configurator.Classes;
using Microsoft.Win32;
using System.Diagnostics;
using System.Windows;

namespace Configurator.Pages
{
    public partial class ComponentsPage
    {
        public ComponentsPage()
        {
            InitializeComponent();
            CheckTweakState();
            if (!App.IsWindows11) containerGrid.Rows = 17;
        }

        private void CheckTweakState()
        {
            tsAnimations.IsChecked = !RegistryTools.CheckTweakState(regDWMRegistry, "DisallowAnimations", 1);
            tsBackgroundApps.IsChecked = RegistryTools.CheckTweakState(regBackgroundApps, "LetAppsRunInBackground", 1);
            tsBluetooth.IsChecked = RegistryTools.CheckTweakState(regBluetooth, "Start", 3);
            tsClipboard.IsChecked = RegistryTools.CheckTweakState(regClipboard, "EnableClipboardHistory", 1);
            tsFSOGameBar.IsChecked = RegistryTools.CheckTweakState(regFSO1, "UseNexusForGameBarEnabled", 1);
            tsPrefetch.IsChecked = RegistryTools.CheckTweakState(regPrefetch, "Start", 2);
            tsHyperV.IsChecked = RegistryTools.CheckTweakState(regHyperV2, "RequireMicrosoftSignedBootChain", 2);
            tsLanmanWorkstation.IsChecked = RegistryTools.CheckTweakState(regWorkstationService1, "Start", 2);
            tsNetworkDiscovery.IsChecked = RegistryTools.CheckTweakState(regNetworkDiscoveryService1, "Start", 2);
            tsNotifications.IsChecked = RegistryTools.CheckTweakState(regNotification1, "ToastEnabled", 1);
            tsPrintSpooler.IsChecked = RegistryTools.CheckTweakState(regSpooler, "Start", 3);
            tsUAC.IsChecked = RegistryTools.CheckTweakState(regUAC, "FilterAdministratorToken", 1);
            tsVPN.IsChecked = RegistryTools.CheckTweakState(regVPNService1, "Start", 3);
            tsWiFi.IsChecked = RegistryTools.CheckTweakState(regWiFiService1, "Start", 2);
            tsHAGS.IsChecked = RegistryTools.CheckTweakState(regHAGS, "HwSchMode", 2);
            tsOldAltTab.IsChecked = RegistryTools.CheckTweakState(regAltTab, "AltTabSettings", 1);
            tsOldContextMenu.IsChecked = RegistryTools.CheckTweakState(Configurator, "OldContextMenu", 1);
        }

        private void tsAnimations_Click(object sender, RoutedEventArgs e)
        {
            if (tsAnimations.IsChecked == true)
            {
                RegistryTools.TryDeletRegistryKey(regDWMRegistry, "DisallowAnimations");
                RegistryTools.TryDeletRegistryKey(regWindowMetrics, "MinAnimate");
                regExplorer.SetValue("TaskbarAnimations", 1);
                regVisualEffects.SetValue("VisualFXSetting", 1);
                byte[] userPreferencesMaskData = { 0x9e, 0x3e, 0x07, 0x80, 0x12, 0x00, 0x00, 0x00 };
                regDesktop.SetValue("UserPreferencesMask", userPreferencesMaskData, RegistryValueKind.Binary);
            }
            else
            {
                regDWMRegistry.SetValue("DisallowAnimations", 1);
                regWindowMetrics.SetValue("MinAnimate", 0);
                regExplorer.SetValue("TaskbarAnimations", 0);
                regVisualEffects.SetValue("VisualFXSetting", 3);
                byte[] userPreferencesMaskData = { 0x90, 0x12, 0x03, 0x80, 0x10, 0x00, 0x00, 0x00 };
                regDesktop.SetValue("UserPreferencesMask", userPreferencesMaskData, RegistryValueKind.Binary);
            }
        }

        private void tsBackgroundApps_Click(object sender, RoutedEventArgs e)
        {
            regBackgroundApps.SetValue("LetAppsRunInBackground", tsBackgroundApps.IsChecked == true ? 1 : 2);
            regBackgroundApps1.SetValue("GlobalUserDisabled", tsBackgroundApps.IsChecked == true ? 0 : 1);
            regBackgroundApps2.SetValue("BackgroundAppGlobalToggle", tsBackgroundApps.IsChecked == true ? 1 : 0);
        }

        private void tsBluetooth_Click(object sender, RoutedEventArgs e)
        {
            regBluetooth.SetValue("Start", tsBackgroundApps.IsChecked == true ? 3 : 4);
        }

        private void tsClipboard_Click(object sender, RoutedEventArgs e)
        {
            using (RegistryKey servicesKey = Registry.LocalMachine.OpenSubKey("SYSTEM\\CurrentControlSet\\Services", true))
            {
                if (servicesKey != null)
                {
                    foreach (string subkeyName in servicesKey.GetSubKeyNames())
                    {
                        if (subkeyName.Contains("cbdhsvc"))
                        {
                            using (RegistryKey subkey = servicesKey.OpenSubKey(subkeyName, true))
                            {
                                subkey?.SetValue("Start", tsClipboard.IsChecked == true ? 2 : 4, RegistryValueKind.DWord);
                            }
                        }
                    }
                }
            }

            regClipboard.SetValue("EnableClipboardHistory", tsClipboard.IsChecked == true ? 1 : 0);
            regClipboard1.SetValue("AllowClipboardHistory", tsClipboard.IsChecked == true ? 1 : 0);
        }

        private void tsFSOGameBar_Click(object sender, RoutedEventArgs e)
        {
            if (tsAnimations.IsChecked == true)
            {
                regFSO1.SetValue("UseNexusForGameBarEnabled", 1);
                regFSO1.SetValue("ShowStartupPanel", 1);
                regFSO2.SetValue("GameDVR_Enabled", 1);
                regFSO2.SetValue("GameDVR_FSEBehavior", 0);
                regFSO2.SetValue("GameDVR_FSEBehaviorMode", 2);
                regFSO2.SetValue("GameDVR_HonorUserFSEBehaviorMode", 0);
                regFSO2.SetValue("GameDVR_DXGIHonorFSEWindowsCompatible", 0);
                regFSO2.SetValue("GameDVR_EFSEFeatureFlags", 1);
                regFSO4.SetValue("AppCaptureEnabled", 1);
                regFSO5.SetValue("Start", 3);
                RegistryTools.TryDeletRegistryKey(regFSO1, "GamePanelStartupTipIndex", "AllowAutoGameMode", "AutoGameModeEnabled");
                RegistryTools.TryDeletRegistryKey(regFSO2, "GameDVR_DSEBehavior");
                RegistryTools.TryDeletRegistryKey(regFSO3, "AllowGameDVR");
                RegistryTools.TryDeletRegistryKey(regFSO6, "__COMPAT_LAYER");
            }
            else
            {
                regFSO1.SetValue("ShowStartupPanel", 0);
                regFSO1.SetValue("GamePanelStartupTipIndex", 3);
                regFSO1.SetValue("AllowAutoGameMode", 0);
                regFSO1.SetValue("AutoGameModeEnabled", 0);
                regFSO1.SetValue("UseNexusForGameBarEnabled", 0);
                regFSO2.SetValue("GameDVR_Enabled", 0);
                regFSO2.SetValue("GameDVR_FSEBehaviorMode", 2);
                regFSO2.SetValue("GameDVR_FSEBehavior", 2);
                regFSO2.SetValue("GameDVR_HonorUserFSEBehaviorMode", 1);
                regFSO2.SetValue("GameDVR_DXGIHonorFSEWindowsCompatible", 1);
                regFSO2.SetValue("GameDVR_EFSEFeatureFlags", 0);
                regFSO2.SetValue("GameDVR_DSEBehavior", 2);
                regFSO3.SetValue("AllowGameDVR", 0);
                regFSO4.SetValue("AppCaptureEnabled", 0);
                regFSO5.SetValue("Start", 4);
                regFSO6.SetValue("__COMPAT_LAYER", "~ DISABLEDXMAXIMIZEDWINDOWEDMODE");
            }
        }

        private void tsPrefetch_Click(object sender, RoutedEventArgs e)
        {
            regPrefetch.SetValue("Start", tsPrefetch.IsChecked == true ? 2 : 4);
            regPrefetch1.SetValue("Start", tsPrefetch.IsChecked == true ? 2 : 4);
        }

        private void tsHyperV_Click(object sender, RoutedEventArgs e)
        {
            if (tsHyperV.IsChecked == true)
            {
                Utils.RunCommand("bcdedit", "/set hypervisorlaunchtype auto");
                Utils.RunCommand("bcdedit", "/deletevalue vm");
                Utils.RunCommand("bcdedit", "/deletevalue loadoptions");
                Utils.RunCommand("DISM", "/Online /Enable-Feature:Microsoft-Hyper-V-All /Quiet /NoRestart");
                RegistryTools.TryDeletRegistryKey(regHyperV1, "EnableVirtualizationBasedSecurity", "RequirePlatformSecurityFeatures", "HypervisorEnforcedCodeIntegrity", "HVCIMATRequired", "LsaCfgFlags", "ConfigureSystemGuardLaunch");
                regHyperV2.SetValue("RequireMicrosoftSignedBootChain", 1);
                regHyperV2.SetValue("EnableVirtualizationBasedSecurity", 1);
                regHyperV2.SetValue("RequirePlatformSecurityFeatures", 1);
                regHyperV2.SetValue("Locked", 0);
                regHyperV3.SetValue("Enabled", 1);
                regHyperV3.SetValue("Locked", 0);
                regHyperV3.SetValue("WasEnabledBy", 1);
            }
            else
            {
                Utils.RunCommand("bcdedit", "/set hypervisorlaunchtype off");
                Utils.RunCommand("bcdedit", "/set vm no");
                Utils.RunCommand("bcdedit", "/set vsmlaunchtype Off");
                Utils.RunCommand("bcdedit", "/set loadoptions DISABLE-LSA-ISO,DISABLE-VBS");
                Utils.RunCommand("DISM", "/Online /Disable-Feature:Microsoft-Hyper-V-All /Quiet /NoRestart");
                regHyperV1.SetValue("EnableVirtualizationBasedSecurity", 0);
                regHyperV1.SetValue("RequirePlatformSecurityFeatures", 1);
                regHyperV1.SetValue("HypervisorEnforcedCodeIntegrity", 0);
                regHyperV1.SetValue("HVCIMATRequired", 0);
                regHyperV1.SetValue("LsaCfgFlags", 0);
                regHyperV1.SetValue("ConfigureSystemGuardLaunch", 0);
                regHyperV2.SetValue("RequireMicrosoftSignedBootChain", 0);
                regHyperV3.SetValue("WasEnabledBy", 0);
                regHyperV3.SetValue("Enabled", 0);
            }
        }

        private void tsLanmanWorkstation_Click(object sender, RoutedEventArgs e)
        {
            ConfigureWorkstation(tsLanmanWorkstation.IsChecked == true);
        }

        private void tsNetworkDiscovery_Click(object sender, RoutedEventArgs e)
        {
            ConfigureWorkstation(tsNetworkDiscovery.IsChecked == true);
            regNetworkDiscoveryService1.SetValue("Start", tsNetworkDiscovery.IsChecked == true ? 2 : 4);
            regNetworkDiscoveryService2.SetValue("Start", tsNetworkDiscovery.IsChecked == true ? 2 : 4);
            regNetworkDiscoveryService3.SetValue("Start", tsNetworkDiscovery.IsChecked == true ? 2 : 4);
        }

        private void tsNotifications_Click(object sender, RoutedEventArgs e)
        {
            regNotificationService.SetValue("Start", tsNotifications.IsChecked == true ? 2 : 4);
            regNotification1.SetValue("ToastEnabled", tsNotifications.IsChecked == true ? 1 : 0);
            regNotification2.SetValue("DisableNotificationCenter", tsNotifications.IsChecked == true ? 0 : 1);
            regNotification3.SetValue("Value", tsNotifications.IsChecked == true ? "Allow" : "Deny");
        }

        private void tsPrintSpooler_Click(object sender, RoutedEventArgs e)
        {
            regSpooler.SetValue("Start", tsPrintSpooler.IsChecked == true ? 3 : 4);
        }

        private void tsVPN_Click(object sender, RoutedEventArgs e)
        {
            regVPNService1.SetValue("Start", tsVPN.IsChecked == true ? 3 : 4);
            regVPNService2.SetValue("Start", tsVPN.IsChecked == true ? 2 : 4);
            regVPNService3.SetValue("Start", tsVPN.IsChecked == true ? 3 : 4);
            regVPNService4.SetValue("Start", tsVPN.IsChecked == true ? 3 : 4);
            regVPNService5.SetValue("Start", tsVPN.IsChecked == true ? 3 : 4);
            regVPNService6.SetValue("Start", tsVPN.IsChecked == true ? 3 : 4);
            regVPNService7.SetValue("Start", tsVPN.IsChecked == true ? 3 : 4);
            regVPNService8.SetValue("Start", tsVPN.IsChecked == true ? 3 : 4);
        }

        private void tsWiFi_Click(object sender, RoutedEventArgs e)
        {
            regWiFiService1.SetValue("Start", tsWiFi.IsChecked == true ? 2 : 4);
            regWiFiService2.SetValue("Start", tsWiFi.IsChecked == true ? 1 : 4);
        }

        private void tsHAGS_Click(object sender, RoutedEventArgs e)
        {
            regHAGS.SetValue("HwSchMode", tsHAGS.IsChecked == true ? 2 : 1);
        }

        private void tsOldAltTab_Click(object sender, RoutedEventArgs e)
        {
            regAltTab.SetValue("AltTabSettings", tsOldAltTab.IsChecked == true ? 1 : 0);
        }

        private void tsOldContextMenu_Click(object sender, RoutedEventArgs e)
        {
            if (tsOldContextMenu.IsChecked == true)
            {
                Process.Start("regedit.exe", "/s C:\\Windows\\Modules\\OldContextMenu.reg"); //Change According to your preference
                Configurator.SetValue("OldContextMenu", 1);
            }
            else
            {
                Process.Start("regedit.exe", "/s C:\\Windows\\Modules\\NewContextMenu.reg"); //Change According to your preference
                RegistryTools.TryDeletRegistryKey(Configurator, "OldContextMenu");
            }
        }

        private void ConfigureWorkstation(bool enable)
        {
            string command;

            regWorkstationService1.SetValue("Start", enable ? 2 : 4);
            regWorkstationService2.SetValue("Start", enable ? 2 : 4);
            regWorkstationService3.SetValue("Start", enable ? 2 : 4);
            regWorkstationService4.SetValue("Start", enable ? 2 : 4);
            regWorkstationService5.SetValue("Start", enable ? 2 : 4);
            regWorkstationService6.SetValue("Start", enable ? 2 : 4);

            if (enable) command = "/Online /Enable-Feature /FeatureName:SmbDirect /NoRestart";
            else command = "/Online /Disable-Feature /FeatureName:SmbDirect /NoRestart";

            Utils.RunCommand("DISM", command);
        }

        private void tsUAC_Click(object sender, RoutedEventArgs e)
        {
            if (tsUAC.IsChecked == true)
            {
                regUAC.SetValue("FilterAdministratorToken", 1);
                regUAC.SetValue("EnableLUA", 1);
            }
            else
            {
                regUAC.SetValue("EnableLUA", 0);
                RegistryTools.TryDeletRegistryKey(regUAC, "FilterAdministratorToken");
            }
        }

        private void btnEdit_Click(object sender, RoutedEventArgs e)
        {
            txtBoxUsername.Text = Environment.UserName;
            spEditUsername.Visibility = Visibility.Visible;
            btnEdit.Visibility = Visibility.Collapsed;
        }

        private void btnGoBack_Click(object sender, RoutedEventArgs e)
        {
            spEditUsername.Visibility = Visibility.Collapsed;
            btnEdit.Visibility = Visibility.Visible;
        }

        private void btnApplyUsername_Click(object sender, RoutedEventArgs e)
        {
            if (string.IsNullOrEmpty(txtBoxUsername.Text))
            {
                Utils.ShowDialog("Configurator", "Username cannot be empty!");
            }
            else if (Environment.UserName == txtBoxUsername.Text)
            {
                Utils.ShowDialog("Configurator", "New username cannot be same as the old one!");
            }
            else
            {
                Utils.RunCommand("wmic.exe", $"useraccount where name='{Environment.UserName}' call rename name='{txtBoxUsername.Text}'");
                Utils.ShowDialog("Configurator", "Username changed successfully! Restart your PC to let the changes take place.");
                spEditUsername.Visibility = Visibility.Collapsed;
                btnEdit.Visibility = Visibility.Visible;
            }
        }

        #region Registry Keys

        private RegistryKey Configurator = Registry.CurrentUser.CreateSubKey(@"Software\Configurator", RegistryKeyPermissionCheck.ReadWriteSubTree);
        private RegistryKey regDWMRegistry = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\DWM");
        private RegistryKey regWindowMetrics = Registry.CurrentUser.CreateSubKey(@"Control Panel\Desktop\WindowMetrics");
        private RegistryKey regExplorer = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\Advanced");
        private RegistryKey regVisualEffects = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer\VisualEffects");
        private RegistryKey regDesktop = Registry.CurrentUser.CreateSubKey(@"Control Panel\Desktop");
        private RegistryKey regBackgroundApps = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\AppPrivacy");
        private RegistryKey regBackgroundApps1 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\BackgroundAccessApplications");
        private RegistryKey regBackgroundApps2 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Search");
        private RegistryKey regBluetooth = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\BthAvctpSvc");
        private RegistryKey regClipboard = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Clipboard");
        private RegistryKey regClipboard1 = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\System");
        private RegistryKey regPrefetch = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\SysMain");
        private RegistryKey regPrefetch1 = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\FontCache");
        private RegistryKey regSpooler = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\Spooler");
        private RegistryKey regHAGS = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\GraphicsDrivers");
        private RegistryKey regAltTab = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Explorer");
        private RegistryKey regWorkstationService1 = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\rdbss");
        private RegistryKey regWorkstationService2 = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\KSecPkg");
        private RegistryKey regWorkstationService3 = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\mrxsmb20");
        private RegistryKey regWorkstationService4 = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\mrxsmb");
        private RegistryKey regWorkstationService5 = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\srv2");
        private RegistryKey regWorkstationService6 = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\LanmanWorkstation");
        private RegistryKey regNetworkDiscoveryService1 = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\NlaSvc");
        private RegistryKey regNetworkDiscoveryService2 = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\lmhosts");
        private RegistryKey regNetworkDiscoveryService3 = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\netman");
        private RegistryKey regVPNService1 = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\IKEEXT");
        private RegistryKey regVPNService2 = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\BFE");
        private RegistryKey regVPNService3 = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\WinHttpAutoProxySvc");
        private RegistryKey regVPNService4 = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\RasMan");
        private RegistryKey regVPNService5 = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\SstpSvc");
        private RegistryKey regVPNService6 = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\iphlpsvc");
        private RegistryKey regVPNService7 = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\NdisVirtualBus");
        private RegistryKey regVPNService8 = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\Eaphost");
        private RegistryKey regWiFiService1 = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\WlanSvc");
        private RegistryKey regWiFiService2 = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\vwififlt");
        private RegistryKey regNotificationService = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\WpnService");
        private RegistryKey regNotification1 = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\PushNotifications");
        private RegistryKey regNotification2 = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\Explorer");
        private RegistryKey regNotification3 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\CapabilityAccessManager\ConsentStore\userNotificationListener");
        private RegistryKey regHyperV1 = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\DeviceGuard");
        private RegistryKey regHyperV2 = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\DeviceGuard");
        private RegistryKey regHyperV3 = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\DeviceGuard\Scenarios\HypervisorEnforcedCodeIntegrity");
        private RegistryKey regFSO1 = Registry.CurrentUser.CreateSubKey(@"Software\Microsoft\GameBar");
        private RegistryKey regFSO2 = Registry.CurrentUser.CreateSubKey(@"System\GameConfigStore");
        private RegistryKey regFSO3 = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Policies\Microsoft\Windows\GameDVR");
        private RegistryKey regFSO4 = Registry.CurrentUser.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\GameDVR");
        private RegistryKey regFSO5 = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Services\BcastDVRUserService");
        private RegistryKey regFSO6 = Registry.LocalMachine.CreateSubKey(@"SYSTEM\CurrentControlSet\Control\Session Manager\Environment");
        private RegistryKey regUAC = Registry.LocalMachine.CreateSubKey(@"SOFTWARE\Microsoft\Windows\CurrentVersion\Policies\System");

        #endregion
    }
}