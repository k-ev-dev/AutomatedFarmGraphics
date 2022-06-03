using Lab3oopServer;
using Lab3oopServer.BaseHandler;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Text;
using System.Threading;
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
using static Lab3OOP.App;

namespace Lab3OOP {
    /// <summary>
    /// Interaction logic for MainWindow.xaml
    /// </summary>
    public partial class MainWindow : Window {
        public MainWindow() {
            InitializeComponent();
        }

        public void SetCowMood() {
            if (MyCow.Mood == Lab3oopServer.Cow.CowMood.Happy) {
                CowMood.Fill = Brushes.Green;
                return;
            }
            if (MyCow.Mood == Lab3oopServer.Cow.CowMood.Passive) {
                CowMood.Fill = Brushes.Yellow;
                return;
            }
            if (MyCow.Mood == Lab3oopServer.Cow.CowMood.Sad) {
                CowMood.Fill = Brushes.Red;
                return;
            }
        }

        public void SetFeederFullness() {
            Brush colorEmpty = Brushes.White;
            Border[] borders = new Border[] { WaterLevel1, WaterLevel2, WaterLevel3 };
            Brush colorMain = Brushes.Blue;
            for(int i = 0; i < 2; i++) {
                if (Feeders[i].Fullness == Feeder.FeederFullness.Level3) {
                    borders[0].Background = colorMain;
                    borders[1].Background = colorMain;
                    borders[2].Background = colorMain;
                    colorMain = Brushes.Yellow;
                    borders = new Border[] { GrainLevel1, GrainLevel2, GrainLevel3 };
                    continue;
                }
                if (Feeders[i].Fullness == Feeder.FeederFullness.Level2) {
                    borders[0].Background = colorMain;
                    borders[1].Background = colorMain;
                    borders[2].Background = colorEmpty;
                    colorMain = Brushes.Yellow;
                    borders = new Border[] { GrainLevel1, GrainLevel2, GrainLevel3 };
                    continue;
                }
                if (Feeders[i].Fullness == Feeder.FeederFullness.Level1) {
                    borders[0].Background = colorMain;
                    borders[1].Background = colorEmpty;
                    borders[2].Background = colorEmpty;
                    colorMain = Brushes.Yellow;
                    borders = new Border[] { GrainLevel1, GrainLevel2, GrainLevel3 };
                    continue;
                }
                if (Feeders[i].Fullness == Feeder.FeederFullness.Empty) {
                    borders[0].Background = colorEmpty;
                    borders[1].Background = colorEmpty;
                    borders[2].Background = colorEmpty;
                    colorMain = Brushes.Yellow;
                    borders = new Border[] { GrainLevel1, GrainLevel2, GrainLevel3 };
                    continue;
                }
            }
        }

        void CowLife() {
            while (true) {
                Thread.Sleep(3000);
                MyCow.Drinking();
                Dispatcher.BeginInvoke(
                    new Action(SetFeederFullness)
                );
                Dispatcher.BeginInvoke(
                new Action(SetCowMood)
                );
                Thread.Sleep(3000);
                MyCow.Drinking();
                MyCow.Eating();
                Dispatcher.BeginInvoke(
                   new Action(SetFeederFullness)
                );
                Dispatcher.BeginInvoke(
                new Action(SetCowMood)
                );
            }
        }

        void FarmerLife() {
            while (BestFarmer.Automatic) {
                Thread.Sleep(5000);
                BestFarmer.CowCare();
                Dispatcher.BeginInvoke(
                   new Action(SetFeederFullness)
                );
            }

            //FarmerLifeTR.Join();
            Dispatcher.BeginInvoke(
                        new Action(FarmerLifeTR.Join)
            );
            FarmerLifeTR = null;
        }

        void ServerRun() {
            server.listener.Start();
            while (true) {
                HttpListenerContext context = server.listener.GetContext();

                if (server.handler.processRequest(context)) {

                    Dispatcher.BeginInvoke(
                        new Action(SetFeederFullness)
                    );

                    if (FarmerLifeTR == null && BestFarmer.Automatic) {
                        FarmerLifeTR = new Thread(FarmerLife);
                        FarmerLifeTR.Start();
                    }

                    Console.WriteLine("RESPONSE IS SENDED!");
                }
                else {
                    // консоль
                    Console.WriteLine("ERROR!!!");
                }
            }
        }

        Thread CowLifeTR;
        Thread FarmerLifeTR;
        Thread ServerLifeTR;
        Server server;
        //MethodHandler handler;
        public Cow MyCow;
        public Farmer BestFarmer;
        public Feeder Water;
        public Feeder Grain;
        public List<Feeder> Feeders;

        override protected void OnActivated(EventArgs e) {
            Grain = new Feeder(1);
            Water = new Feeder(0);
            Feeders = new List<Feeder>();
            Feeders.Add(Water);
            Feeders.Add(Grain);
            MyCow = new Cow(Feeders);
            BestFarmer = new Farmer(MyCow);
            if (CowLifeTR == null) {
                CowLifeTR = new Thread(CowLife);
                CowLifeTR.Start();
            }

            MethodHandler handler = new MethodHandler(BestFarmer);
            server = new("http://localhost:8888/connection/", handler);
            
            if (ServerLifeTR == null) {
                ServerLifeTR = new Thread(ServerRun);
                ServerLifeTR.Start();
            }
        }

        private void Window_Closing(object sender, System.ComponentModel.CancelEventArgs e) {
            Environment.Exit(0);
        }
    }
}
