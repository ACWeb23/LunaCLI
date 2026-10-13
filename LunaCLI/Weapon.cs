using System.Collections.Generic;

namespace Zenless
{
    // ------------------------------------------------------------------
    // WEAPONS
    // ------------------------------------------------------------------
    abstract class Weapon
    {
        public int WeaponID { get; private set; }

        /// <summary>Optional weapon art shown in the windowed interface. Empty until an image is chosen.</summary>
        public string? ImagePath { get; set; }

        private string name { get; set; } 
        private double baseATK { get; set; }

        private advancedStats advStat { get; set; } // Energy, crit, ATK% ect
        private double stat_Value { get; set; }

        private Buff[] passiveBuffs { get; set; } = [];

        /// <summary>
        /// Armorer W-Engines add <see cref="baseATK"/> to DEF. Every other engine adds it to ATK.
        /// </summary>
        public bool UsesDefenseBase { get; private set; }

        public string BaseStatLabel => UsesDefenseBase ? "Base DEF" : "Base ATK";

        protected Weapon(int weaponID, string name, double baseATK, advancedStats advStat, double stat_Value, bool usesDefenseBase = false)
        {
            this.WeaponID = weaponID;
            this.name = name;
            this.baseATK = baseATK;
            this.advStat = advStat;
            this.stat_Value = stat_Value;
            this.UsesDefenseBase = usesDefenseBase;
        }

        /// <summary>
        /// Resolves a file in <c>Images/Wengine</c> next to the running app.
        /// </summary>
        protected static string Portrait(string fileName)
        {
            return Path.Combine(AppContext.BaseDirectory, "Images", "Wengine", fileName);
        }

        /// <summary>
        /// Returns the name of the weapon
        /// </summary>
        public string get_Name()
        {
            return name;
        }
        /// <summary>
        /// Takes an array of the struct buff and assings them to the passiveBuffs array.
        /// </summary>
        /// <param name="passive"></param>
        protected void setPassive(Buff[] passive)
        {
            passiveBuffs = passive;
        }

        /// <summary>
        /// this method returns the weapons passive.
        /// </summary>
        /// <returns>Wpassive</returns>
        public virtual Buff[] weaponPassive()
        {
            return passiveBuffs;
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
