using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WarThunderSlotsSavior {
    class AppConfig {
        public static string SavingPath {
            get {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) 
                    + "\\My Games\\WarThunder\\Saves";
                return path;
            }
        }

        public static string BackupPath {
            get {
                string path = SavingPath + ".backup";
                return path;
            }
        }

        public static string PathIn(string rootPath, string accountID = "", WTSetting.Type? setting = null) {
            string path = rootPath;
            path += accountID == "" ? "" : ("\\" + accountID);
            path += setting is null ? "" : ("\\production\\" + setting.ToString() + ".blk");
            return path;
        }

        public static string GlobalPathIn(string rootPath, string accountID = "") {
            return PathIn(rootPath, accountID, WTSetting.Type.global);
        }

        public static void CheckSystemLanguage(string defaultLang = "en") {
            string systemLang = System.Threading.Thread.CurrentThread.CurrentCulture.Name;
            string lang = systemLang.Contains("zh") ? "zh" : defaultLang;

            List<ResourceDictionary> dictList = new List<ResourceDictionary>();
            foreach (ResourceDictionary dict in App.Current.Resources.MergedDictionaries) {
                dictList.Add(dict);
            }
            string langCulture = "Resources\\Lang\\" + lang + ".xaml";
            ResourceDictionary resourceDict = dictList.FirstOrDefault(d => d.Source.OriginalString == langCulture);
            Application.Current.Resources.MergedDictionaries.Remove(resourceDict);
            Application.Current.Resources.MergedDictionaries.Add(resourceDict);
        }
    }
}
