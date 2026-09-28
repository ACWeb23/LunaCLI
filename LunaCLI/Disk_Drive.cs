using System;
using System.Collections.Generic;
using System.Linq;

namespace Zenless
{
    // ------------------------------------------------------------------
    // DISK DRIVES
    // ------------------------------------------------------------------
    abstract class Disk_Drive
    {
        protected int diskID;

        protected int setID;
        public driveSet DriveSet { get; protected set; }
        public int Drive_Num { get; protected set; }
        public advancedStats MainStat { get; protected set; }
        public double MainStatValue { get; protected set; }
        protected (advancedStats Stat, int NumRoll) substat1;
        protected (advancedStats Stat, int NumRoll) substat2;
        protected (advancedStats Stat, int NumRoll) substat3;
        protected (advancedStats Stat, int NumRoll) substat4;

        protected Disk_Drive(int ID, int specID)
        {
            this.diskID = ID;
            this.setID = specID;
        }

        protected void Load_Substats(Dictionary<advancedStats, int> DriveSubStats)
        {
            if (DriveSubStats.Count > 4)
                throw new ArgumentException("A disk drive can have at most 4 substats.");

            int i = 0;
            foreach (KeyValuePair<advancedStats, int> sub in DriveSubStats)
            {
                var entry = (sub.Key, sub.Value);
                switch (i)
                {
                    case 0: substat1 = entry; break;
                    case 1: substat2 = entry; break;
                    case 2: substat3 = entry; break;
                    case 3: substat4 = entry; break;
                }
                i++;
            }
        }

        /// <summary>
        /// Returns only the substats that are actually populated (NumRoll > 0).
        /// </summary>
        public IEnumerable<(advancedStats Stat, int NumRoll)> GetSubstats()
        {
            var all = new[] { substat1, substat2, substat3, substat4 };
            return all.Where(s => s.NumRoll > 0);
        }

        /// <summary>
        /// Approximate value of a single substat roll. Tweak these to match the real game values.
        /// </summary>
        public static double SubstatRollValue(advancedStats stat)
        {
            switch (stat)
            {
                case advancedStats.ATK_Percent: return 0.03;
                case advancedStats.HP_Percent: return 0.03;
                case advancedStats.DEF_Percent: return 0.048;
                case advancedStats.Crit_Rate: return 0.024;
                case advancedStats.Crit_Damage: return 0.048;
                case advancedStats.PEN_Ratio: return 0.024;
                case advancedStats.Anomaly_Proficiency: return 9;
                case advancedStats.ATK: return 19;
                case advancedStats.HP: return 112;
                case advancedStats.DEF: return 15;
                case advancedStats.PEN: return 9;
                default: return 0;
            }
        }
    }
}