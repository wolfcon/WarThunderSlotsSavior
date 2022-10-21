using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading.Tasks;
using System.Windows;

namespace WarThunderSlotsSavior {
    class WTSetting {

        public enum Type {
            global,
            machine,
            storage
        }
        private static readonly Regex versionRegex = new("^version:i=([0-9]*)");

        public static string LastBackupDateString() {
            string dateString = null;
            DirectoryInfo dirInfo = new DirectoryInfo(AppConfig.BackupPath);
            if (dirInfo.Exists) {
                dateString = dirInfo.CreationTime.ToString();
            }

            return dateString;
        }
        public static void Backup() {
            bool result = FileUtility.CopyFolder(AppConfig.SavingPath, AppConfig.BackupPath);
            if (result) {
                MessageBox.Show("🎉Your slot preset has been backed up!🎉");
            }
        }

        public static void Restore() {
            ResetBackupVersions();

            if (!Directory.Exists(AppConfig.BackupPath)) {
                MessageBox.Show("🎃You have no backup at all!!!🎃");
                return;
            }
            bool result = FileUtility.CopyFolder(AppConfig.BackupPath, AppConfig.SavingPath);
            if (result) {
                MessageBox.Show("🎉Your slot preset has been RESTORED!🎉");
            }
        }

        public static void ResetBackupVersions() {
            string[] accounts = GetAllSavedAccounts();
            foreach (string account in accounts) {
                string globalVersion = WTSetting.Version(AppConfig.GlobalPathIn(AppConfig.SavingPath, account));
                ReplaceVersion(BoostVersion(globalVersion), account, Type.global);

                string machineVersion = WTSetting.Version(AppConfig.PathIn(AppConfig.SavingPath, account, Type.machine));
                ReplaceVersion(BoostVersion(machineVersion), account, Type.machine);

                string storageVersion = WTSetting.Version(AppConfig.PathIn(AppConfig.SavingPath, account, Type.storage));
                ReplaceVersion(storageVersion, account, Type.storage);
            }
        }

        public static string[] GetAllSavedAccounts() {
            ArrayList accounts = new ArrayList();
            string savingRoot = AppConfig.SavingPath;
            // If destination folder dose not exist, return null.
            if (!Directory.Exists(savingRoot)) {
                return null;
            }

            // Search global setting in sub folders.
            string[] folders = Directory.GetDirectories(savingRoot);
            foreach (string folder in folders) {
                string accountID = folder.Split('\\')[folder.Split('\\').Length - 1];
                string globalPath = AppConfig.GlobalPathIn(folder);
                if (File.Exists(globalPath)) {
                    accounts.Add(accountID);
                }
            }

            return (string[])accounts.ToArray(typeof(string));
        }

        private static string Text(string savingRoot, string account = "", Type? setting = null) {
            string path = AppConfig.PathIn(savingRoot, account, setting);
            if (!File.Exists(path)) {
                return null;
            }

            string text = File.ReadAllText(path);
            if (text == null || text.Length == 0) {
                return null;
            }
            return text;
        }

        private static void SaveText(string text, string savingRoot, string account = "", Type? setting = null) {
            string path = AppConfig.PathIn(savingRoot, account, setting);
            try {
                File.WriteAllText(path, text);
            } catch {
                MessageBox.Show("😈Version modificaiton failed!😈");
            }
        }

        public static string Version(string savingRoot, string account = "", Type? setting = null) {
            string text = Text(savingRoot, account, setting);

            string version = null;
            Match match = versionRegex.Match(text);
            if (match.Groups.Count > 0) {
                version = match.Groups[1].Value;
            }

            return version;
        }

        private static string BoostVersion(string version) {
            try {
                return (Convert.ToInt64(version) + 1).ToString();
            } catch {
                return version;
            }
        }
        /// <summary>
        /// Replace file setting version. If saving root is null, backup path will be the default value.
        /// </summary>
        /// <param name="version"></param>
        /// <param name="account"></param>
        /// <param name="setting"></param>
        /// <param name="savingRoot"></param>
        /// <returns></returns>
        public static bool ReplaceVersion(string version, string account = "", Type? setting = null, string savingRoot = null) {
            string path = savingRoot ?? AppConfig.BackupPath;
            string text = Text(path, account, setting);

            Match match = versionRegex.Match(text);
            if (match.Groups.Count > 0) {
                string replacedText = Regex.Replace(text, versionRegex.ToString(), "version:i=" + version);
                SaveText(replacedText, path, account, setting);
            }

            return true;
        }
    }
}
