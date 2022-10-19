using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WarThunderSlotsSavior {
    class FileUtility {
        /// <summary>
        /// Copy files and subfolders
        /// </summary>
        /// <param name="sourceFolder">Source folder path</param>
        /// <param name="destFolder">Destination folder path</param>
        /// <returns></returns>
        public static bool CopyFolder(string sourcePath, string destPath) {
            try {
                // If destination folder dose not exist, create folder.
                if (!Directory.Exists(destPath)) {
                    Directory.CreateDirectory(destPath);
                } else {
                    DirectoryInfo dirInfo = new DirectoryInfo(destPath);
                    try {
                        dirInfo.Delete(true);
                        Directory.CreateDirectory(destPath);
                    } catch (Exception ex) {
                        MessageBox.Show(ex.Message);
                        return false;
                    }
                }
                // Get all files in root folder.
                string[] files = Directory.GetFiles(sourcePath);
                foreach (string file in files) {
                    string name = Path.GetFileName(file);
                    string dest = Path.Combine(destPath, name);
                    // copy file to destination
                    File.Copy(file, dest);
                }
                // Get sub folders in root folder.
                string[] folders = Directory.GetDirectories(sourcePath);
                foreach (string folder in folders) {
                    string dirName = folder.Split('\\')[folder.Split('\\').Length - 1];
                    string dest = Path.Combine(destPath, dirName);
                    // Call recursively
                    CopyFolder(folder, dest);
                }
            } catch (Exception ex) {
                MessageBox.Show(ex.Message);
                return false;
            }
            return true;
        }
    }
}
