using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

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
            feeder.ChangeLevel(0);
            ChangeMood(1);
        }

        public void Eating() {
            var feeder = Feeders
                .Where(f => f.Kind == Feeder.FeederKind.Grain)
                .Where(f => f.Fullness != Feeder.FeederFullness.Empty)
                .FirstOrDefault();
            if (feeder == null) {
                ChangeMood(0);
                return;
            }
            feeder.ChangeLevel(0);
            ChangeMood(1);
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
    }
}
