using System.Collections.Generic;

namespace Zenless
{
    // ------------------------------------------------------------------
    // WEAPONS
    // ------------------------------------------------------------------
    abstract class Weapon
    {
        private int weaponID { get; set; } // Unique ID for the weapon
        private string name { get; set; } 
        private double baseATK { get; set; }

        private advancedStats advStat { get; set; } // Energy, crit, ATK% ect
        private double stat_Value { get; set; }

        private Buff[] wPassive { get; set; } = [];

        protected Weapon(int weaponID, string name, double baseATK, advancedStats advStat, double stat_Value)
        {
            this.weaponID = weaponID;
            this.name = name;
            this.baseATK = baseATK;
            this.advStat = advStat;
            this.stat_Value = stat_Value;
        }

        /// <summary>
        /// Returns the name of the weapon
        /// </summary>
        public string get_Name()
        {
            return name;
        }
        /// <summary>
        /// Takes an array of the struct buff and assings them to the wPassive array.
        /// </summary>
        /// <param name="passive"></param>
        protected void setPassive(Buff[] passive)
        {
            wPassive = passive;
        }

        /// <summary>
        /// this method returns the weapons passive.
        /// </summary>
        /// <returns>Wpassive</returns>
        public virtual Buff[] weaponPassive()
        {
            return wPassive;
        }

        /// <summary>
        /// Returns the stats of the weapon as a tuple
        /// </summary>
        /// <returns>A tuple with the weapon's base ATK and advanced stat</returns>
        public (double, Dictionary<advancedStats, double>) get_Base_Stats()
        {
            Dictionary<advancedStats, double> stats = new Dictionary<advancedStats, double>();
            stats.Add(advStat, stat_Value);
            return (baseATK, stats);
        }
    }
}
