using System.Globalization;
using System.Threading;
using System.Windows;

namespace LiveCaptionsTranslator.utils
{
    public static class LocalizationManager
    {
        private static ResourceDictionary? currentDictionary;

        public static string NormalizeCulture(string? cultureName)
        {
            if (string.IsNullOrWhiteSpace(cultureName))
                return "zh-CN";

            cultureName = cultureName.Trim();
            if (string.Equals(cultureName, "zh", StringComparison.OrdinalIgnoreCase))
                return "zh-CN";
            if (string.Equals(cultureName, "en", StringComparison.OrdinalIgnoreCase))
                return "en-US";

            return cultureName;
        }

        public static void ApplyCulture(string? cultureName)
        {
            string normalized = NormalizeCulture(cultureName);

            var culture = new CultureInfo(normalized);
            Thread.CurrentThread.CurrentCulture = culture;
            Thread.CurrentThread.CurrentUICulture = culture;
            CultureInfo.DefaultThreadCurrentCulture = culture;
            CultureInfo.DefaultThreadCurrentUICulture = culture;

            if (Application.Current == null)
                return;

            var uri = new Uri($"pack://application:,,,/;component/src/Strings/StringResources.{normalized}.xaml", UriKind.Absolute);
            var dictionary = new ResourceDictionary { Source = uri };

            if (currentDictionary != null)
                Application.Current.Resources.MergedDictionaries.Remove(currentDictionary);

            Application.Current.Resources.MergedDictionaries.Add(dictionary);
            currentDictionary = dictionary;
        }
    }
}
