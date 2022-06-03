using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3oopServer {
    public class Farmer {
        public Farmer(Cow cow) {
            Automatic = false;
            Cow = cow;
        }

        public bool Automatic { get; set; }
        public Cow Cow { get; set; }

        public void AddWaterFull() {
            var feeders = Cow.Feeders
                .Where(f => f.Kind == Feeder.FeederKind.Water)
                .Where(f => f.Fullness != Feeder.FeederFullness.Level3)
                .Select(f => f);
            foreach(var item in feeders) {
                do {
                    item.ChangeLevel(true);
                } while (item.Fullness != Feeder.FeederFullness.Level3);
            }
        }

        public void AddGrainFull() {
            var feeders = Cow.Feeders
                .Where(f => f.Kind == Feeder.FeederKind.Grain)
                .Where(f => f.Fullness != Feeder.FeederFullness.Level3)
                .Select(f => f);
            foreach (var item in feeders) {
                do {
                    item.ChangeLevel(true);
                } while (item.Fullness != Feeder.FeederFullness.Level3);
            }
        }

        public void AddWater() {
            var feeder = Cow.Feeders
                .Where(f => f.Kind == Feeder.FeederKind.Water)
                .Where(f => f.Fullness != Feeder.FeederFullness.Level3)
                .FirstOrDefault();
            if (feeder != null) {
                feeder.ChangeLevel(true);
            }
        }

        public void AddGrain() {
            var feeder = Cow.Feeders
                .Where(f => f.Kind == Feeder.FeederKind.Grain)
                .Where(f => f.Fullness != Feeder.FeederFullness.Level3)
                .FirstOrDefault();
            if (feeder != null) {
                feeder.ChangeLevel(true);
            }
        }

        public void FermerAutoOn() {
            Automatic = true;
        }

        public void FermerAutoOff() {
            Automatic = false;
        }

        public void CowCare() {
            if (!CheckCowMood()) {
                AddWaterFull();
                AddGrainFull();
            }
        }

        public bool CheckCowMood() {
            return Cow.Mood == Cow.CowMood.Happy;
        }
    }
}
