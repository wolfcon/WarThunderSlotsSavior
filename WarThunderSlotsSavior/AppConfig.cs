using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace WarThunderSlotsSavior {
    class AppConfig {
        public static string savingPath {
            get {
                string path = Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments) 
                    + "\\My Games\\WarThunder\\Saves";
                return path;
            }
        }

        public static string backupPath {
            get {
                string path = savingPath + ".backup";
                return path;
            }
        }
    }
}
