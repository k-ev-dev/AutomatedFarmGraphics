using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Lab3oopServer {
    public class Feeder {
        public Feeder(int kind) {
            Fullness = FeederFullness.Level3;
            Kind = (FeederKind)kind;
        }

        public FeederFullness Fullness { get; set; }
        public enum FeederFullness {
            Empty = 0,
            Level1 = 1,
            Level2 = 2,
            Level3 = 3
        }

        public FeederKind Kind { get; set; }
        public enum FeederKind {
            Water = 0,
            Grain = 1
        }

        public void ChangeLevel(bool direction) { 
            if (!direction) {
                if (Fullness == FeederFullness.Empty) { return; }
                Fullness--;
                return;
            }
            if (Fullness == FeederFullness.Level3) { return; }
            Fullness++;
        }
    }
}
