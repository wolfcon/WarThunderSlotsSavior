using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WarThunderSlotsSavior {
    class WTSetting {
        public static string lastBackupDateString() {
            string dateString = null;
            DirectoryInfo dirInfo = new DirectoryInfo(AppConfig.backupPath);
            if (dirInfo.Exists) {
                dateString = dirInfo.CreationTime.ToString();
            }

            return dateString;
        }
        public static void backup() {
            bool result = FileUtility.CopyFolder(AppConfig.savingPath, AppConfig.backupPath);
            if (result) {
                MessageBox.Show("🎉Your slot preset has been backed up!🎉");
            }
        }

        public static void restore() {
            if (!Directory.Exists(AppConfig.backupPath)) {
                MessageBox.Show("🎃You have no backup at all!!!🎃");
                return;
            }
            bool result = FileUtility.CopyFolder(AppConfig.backupPath, AppConfig.savingPath);
            if (result) {
                MessageBox.Show("🎉Your slot preset has been RESTORED!🎉");
            }
        }
    }
}
