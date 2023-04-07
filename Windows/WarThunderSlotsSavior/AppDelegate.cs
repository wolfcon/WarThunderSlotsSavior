using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Controls;
using System.Windows.Forms;

namespace WarThunderSlotsSavior {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : System.Windows.Application
    {

        NotifyIcon notifyIcon;

        public void InitializeSystemTray() {
            string path = System.IO.Path.GetFullPath(@"Logo\icon.ico");
            if (File.Exists(path)) {
                notifyIcon = new NotifyIcon();
                notifyIcon.BalloonTipText = "Hello, 文件监视器"; //设置程序启动时显示的文本
                notifyIcon.Text = "文件监视器";//最小化到托盘时，鼠标点击时显示的文本
                System.Drawing.Icon icon = new System.Drawing.Icon(path);//程序图标
                notifyIcon.Icon = icon;
                notifyIcon.Visible = true;
                notifyIcon.ShowBalloonTip(1);

                ToolStripMenuItem showMenuItem = new ToolStripMenuItem("显示");
                showMenuItem.Click += showMenuItem_Click;
                MenuItem hideMenuItem = new MenuItem("隐藏");
                hideMenuItem.Click += new EventHandler(hideMenuItem_Click);
                MenuItem quitMenuItem = new MenuItem("退出");
                quitMenuItem.Click += new EventHandler(quitMenuItem_Click);
                //将上面的3个自选项加入到parentMenuitem中。
                MenuItem[] parentMenuItem = new MenuItem[] { showMenuItem, hideMenuItem, quitMenuItem };
                //为notifyIconContextMenu。
                notifyIcon.ContextMenuStrip = new ContextMenuStrip(parentMenuItem);
                
                notifyIcon.MouseClick += new MouseEventHandler(notifyIcon_MouseClick);
                notifyIcon.MouseDoubleClick += new MouseEventHandler(notifyIcon_MouseDoubleClick);
            }
        }

        private void showMenuItem_Click(object sender, EventArgs e) { 
            
        }
        private void hideMenuItem_Click(object sender, EventArgs e) {

        }
        private void quitMenuItem_Click(object sender, EventArgs e) {

        }
        private void notifyIcon_MouseClick(object sender, MouseEventArgs e) {

        }
        private void notifyIcon_MouseDoubleClick(object sender, MouseEventArgs e) {

        }
    }

    /// <summary>
    /// App
    /// Rewrite Main function to receive the arguments.
    class AppDelegate {
        public static string[] globalArgs;
        [STAThread]
        public static void Main(string[] args) {
            /*if (args.Length > 0)
            {
                globalArgs = args;
                string argsString = "";
                foreach (var item in args)
                {
                    argsString += "+ " + item;
                }
                MessageBox.Show(argsString);
            }*/
            App app = new WarThunderSlotsSavior.App();
            app.InitializeComponent();
            app.InitializeSystemTray();

            AppConfig.CheckSystemLanguage();
            MainWindow mainWindow = new MainWindow();
            app.MainWindow = mainWindow;
            app.Run();
        }
    }
}
