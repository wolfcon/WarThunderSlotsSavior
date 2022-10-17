using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Data;
using System.Windows.Documents;
using System.Windows.Input;
using System.Windows.Media;
using System.Windows.Media.Imaging;
using System.Windows.Navigation;
using System.Windows.Shapes;

namespace WarThunderSlotsSavior {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
            refreshBackupStatus();
        }
        
        protected override void OnClosed(EventArgs e) {
            App.Current.Shutdown();
        }

        public void refreshBackupStatus() {
            var dateString = lastBackupDateString();
            var hasBackup = dateString != null;
            restoreButton.IsEnabled = hasBackup;
            lastBackupDateLabel.Content = hasBackup ? "Last: " + lastBackupDateString() : "No backup";
        }
        static string lastBackupDateString() {
            string dateString = null;
            var dirInfo = new DirectoryInfo(AppConfig.backupPath);
            if (dirInfo.Exists) {
                dateString = dirInfo.CreationTime.ToString();
            }
            
            return dateString;
        }
        static void backupSettings() {
            var result = FileUtility.CopyFolder(AppConfig.savingPath, AppConfig.backupPath);
            if (result) {
                MessageBox.Show("🎉Your slot preset has been backed up!🎉");
            }
        }

        static void restoreSettings() {
            if (!Directory.Exists(AppConfig.backupPath)) {
                MessageBox.Show("🎃You have no backup at all!!!🎃");
                return;
            }
            var result = FileUtility.CopyFolder(AppConfig.backupPath, AppConfig.savingPath);
            if (result) {
                MessageBox.Show("🎉Your slot preset has been RESTORED!🎉");
            }
        }

        protected override void OnMouseLeftButtonDown(MouseButtonEventArgs e) {
            base.OnMouseLeftButtonDown(e);

            // Begin dragging the window
            DragMove();
        }

        private void backupButton_Click(object sender, RoutedEventArgs e) {
            backupSettings();
            refreshBackupStatus();
        }

        private void restoreButton_Click(object sender, RoutedEventArgs e) {
            restoreSettings();
        }

        private void infoButton_Click(object sender, RoutedEventArgs e) {
            MessageBox.Show("Github: https://github.com/wolfcon \nQQ: 472730949 \nWarThunder ID: conMan", "I am Frank!");
        }
    }
}
