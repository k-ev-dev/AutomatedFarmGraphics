using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3oopServer {
    public class Farmer {
        public Farmer(Cow cow) {
            Automatic = true;
            Cow = cow;
        }

        public bool Automatic { get; set; }
        public Cow Cow { get; set; }

        public void AddWater() {
            var feeders = Cow.Feeders
                .Where(f => f.Kind == Feeder.FeederKind.Water)
                .Select(f => f);
            foreach(var item in feeders) {
                do {
                    item.ChangeLevel(true);
                } while (item.Fullness != Feeder.FeederFullness.Level3);
            }
        }

        public void AddGrain() {
            var feeders = Cow.Feeders
                .Where(f => f.Kind == Feeder.FeederKind.Grain)
                .Select(f => f);
            foreach (var item in feeders) {
                do {
                    item.ChangeLevel(true);
                } while (item.Fullness != Feeder.FeederFullness.Level3);
            }
        }

        public void CowCare() {
            if (!CheckCowMood()) {
                AddWater();
                AddGrain();
            }
        }

        public bool CheckCowMood() {
            return Cow.Mood == Cow.CowMood.Happy;
        }
    }
}
