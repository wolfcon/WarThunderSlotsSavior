using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows;

namespace WarThunderSlotsSavior {
    /// <summary>
    /// Interaction logic for App.xaml
    /// </summary>
    public partial class App : Application {
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
                var argsString = "";
                foreach (var item in args)
                {
                    argsString += "+ " + item;
                }
                MessageBox.Show(argsString);
            }*/
            var app = new WarThunderSlotsSavior.App();
            app.InitializeComponent();

            var mainWindow = new MainWindow();
            app.MainWindow = mainWindow;
            app.Run();
            App.Main();
        }
    }
}
