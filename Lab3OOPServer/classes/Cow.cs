using Lab3OOP;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;
using System.Windows;

namespace Lab3oopServer {
    public class Cow {

        public Cow(List<Feeder> feeders) {
            Feeders = feeders;
            Mood = CowMood.Happy;
        }

        public List<Feeder> Feeders { get; set; }

        public CowMood Mood { get; set; }
        public enum CowMood {
            Sad = 0,
            Passive = 1,
            Happy = 2
        }

        public void Drinking() {
            var feeder = Feeders
                .Where(f => f.Kind == Feeder.FeederKind.Water)
                .Where(f => f.Fullness != Feeder.FeederFullness.Empty)
                .FirstOrDefault();
            if (feeder == null) {
                ChangeMood(0);
                return;
            }
            feeder.ChangeLevel(false);
            ChangeMood(1);
        }

        public void Eating() {
            var feederGrain = Feeders
                .Where(f => f.Kind == Feeder.FeederKind.Grain)
                .Where(f => f.Fullness != Feeder.FeederFullness.Empty)
                .FirstOrDefault();
            if (feederGrain == null) {
                ChangeMood(0);
                return;
            }
            var feederWater = Feeders
               .Where(f => f.Kind == Feeder.FeederKind.Water)
               .FirstOrDefault();
            feederGrain.ChangeLevel(false);
            if (feederWater.Fullness != Feeder.FeederFullness.Empty) {
                ChangeMood(1);
            }
        }

        public void ChangeMood(int direction) {
            if (direction == 0) {
                if (Mood == CowMood.Sad) { return; }
                Mood--;
                return;
            }
            if (Mood == CowMood.Happy) { return; }
            Mood++;
        }

        /*public void threadCowProcess(MainWindow window) {
            while (true) {
                Thread.Sleep(3000);
                Drinking();
                window.Dispatcher.BeginInvoke(
                    new Action(window.SetFeederFullness)
                );
                window.Dispatcher.BeginInvoke(
                new Action(window.SetCowMood)
                );
                Thread.Sleep(3000);
                Drinking();
                Eating();
                window.Dispatcher.BeginInvoke(
                   new Action(window.SetFeederFullness)
                );
                window.Dispatcher.BeginInvoke(
                new Action(window.SetCowMood)
                );
            }
        }*/
    }
}
