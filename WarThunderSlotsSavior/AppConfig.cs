using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
    }
}
