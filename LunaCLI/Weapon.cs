using System.Collections.Generic;

namespace Zenless
{
    // ------------------------------------------------------------------
    // WEAPONS
    // ------------------------------------------------------------------
    class Weapon
    {
        private int weaponID { get; set; }
        private string name { get; set; }
        private double baseATK { get; set; }

        private advancedStats advStat { get; set; }
        private double stat_Value { get; set; }

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
