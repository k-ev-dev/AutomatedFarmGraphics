using System;
using System.Collections.Generic;
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
using static Lab3oopClient.App;

namespace Lab3oopClient {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        public bool Auto = false;
        public JsonRPCClient client;

        override protected void OnActivated(EventArgs e) {
            string url = "http://localhost:8888/connection/";
            client = new JsonRPCClient(url);
        }

        private void AddWater_Click(object sender, RoutedEventArgs e) {
            client.AddWater();
        }

        private void AddGrain_Click(object sender, RoutedEventArgs e) {
            client.AddGrain();
        }

        private void AutoOn_Click(object sender, RoutedEventArgs e) {
            if (!Auto) {
                Auto = true;
                client.AutoOn();
                AutoOff.Background = Brushes.Gray;
                AutoOn.Background = Brushes.GreenYellow;
            }
        }

        private void AutoOff_Click(object sender, RoutedEventArgs e) {
            if (Auto) {
                Auto = false;
                client.AutoOff();
                AutoOff.Background = Brushes.GreenYellow;
                AutoOn.Background = Brushes.Gray;
            }
        }

        private void OnClosed(object sender, EventArgs e) {
            Application.Current.Shutdown();
        }
    }
}
