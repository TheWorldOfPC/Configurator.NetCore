using Microsoft.Win32;

namespace Configurator.Classes
{
    public class RegistryTools
    {
        public static bool CheckTweakState(RegistryKey key, string dwordName, int value)
        {
            try
            {
                object dwordValue = key.GetValue(dwordName);

                if (dwordValue != null && dwordValue is int && (int)dwordValue == value)
                {
                    return true;
                }
            }
            catch
            {
                return false;
            }

            return false;
        }

        public static void TryDeletRegistryKey(RegistryKey key, string dword)
        {
            try
            {
                key.DeleteValue(dword);
            }
            catch { }
        }

        public static void TryDeletRegistryKey(RegistryKey key, params string[] dwords)
        {
            foreach (string dword in dwords)
            {
                try
                {
                    key.DeleteValue(dword);
                }
                catch { }
            }
        }
    }
}