using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Zenless
{
    // ------------------------------------------------------------------
    // CHARACTER
    // ------------------------------------------------------------------
    /// <summary>
    /// Base type for every agent. Concrete agents (for example <c>Remielle</c>)
    /// supply identity, rank, and level-60 base stats through the constructor and
    /// their own field initializers. This class owns equipment, mindscape unlocks,
    /// and the formulas that turn base stats plus gear into the numbers printed
    /// by <see cref="PrintSummary"/>.
    /// </summary>
    /// <remarks>
    /// Stats are stored as the raw values the combat formulas expect:
    /// ratios such as crit rate and PEN are fractions (0.05 = 5%), while HP, ATK,
    /// DEF, Impact, Anomaly Proficiency, and flat bonuses are absolute numbers.
    /// Subclasses may read every <c>protected</c> member; callers outside the
    /// hierarchy only see identity and the equip / summary API.
    /// </remarks>
    abstract class Character
    {
        // ---------------- Identity ----------------
        // Fixed for the lifetime of the agent. Set once by the constructor and
        // exposed publicly so UI and squad code can read them without a subclass.

        /// <summary>In-game agent id. Used to tell two agents of the same name apart.</summary>
        public int characterID { get; protected set; }

        /// <summary>Display name shown in summaries and squad listings.</summary>
        public string name { get; protected set; }

        /// <summary>Rarity letter, typically <c>'A'</c> or <c>'S'</c>.</summary>
        public char rank { get; protected set; }

        /// <summary>Combat role: Attack, Stun, Anomaly, Support, Defense, Rupture, or Armorer.</summary>
        public Specialty specialty { get; protected set; }

        /// <summary>Basic-attack motion type: Slash, Strike, or Pierce.</summary>
        public attackType attackType { get; protected set; }

        /// <summary>Damage element (Physical, Fire, Ice, Electric, Ether, and so on).</summary>
        public atkElement atkElement { get; protected set; }

        // ---------------- Base combat stats ----------------
        // These are the naked agent values before a W-Engine or disk drives are
        // applied. Subclasses assign them after calling the base constructor.
        // Percent-style stats are stored as fractions, not as 0-100 numbers.

        /// <summary>Level base HP, before HP% and flat HP from gear.</summary>
        protected double baseHP { get; set; }

        /// <summary>Level base ATK. W-Engine base ATK is added on top of this, not baked in.</summary>
        protected double baseATK { get; set; }

        /// <summary>Level base DEF, before DEF% and flat DEF from gear.</summary>
        protected double baseDEF { get; set; }

        /// <summary>Level base Impact. Daze dealt scales with this.</summary>
        protected double baseImpact { get; set; }

        /// <summary>Base crit rate as a fraction (0.05 = 5%). Gear crit rate is added later.</summary>
        protected double critRate { get; set; }

        /// <summary>Base crit damage as a fraction (0.50 = 50%). Gear crit damage is added later.</summary>
        protected double critDMG { get; set; }

        /// <summary>Base Anomaly Mastery. Buildup rate scales with this, then with AM% from gear.</summary>
        protected double AnomalyMastery { get; set; }

        /// <summary>Base Anomaly Proficiency. Disorder and anomaly damage scale with the flat total.</summary>
        protected double AnomalyProficiency { get; set; }

        /// <summary>Base PEN ratio as a fraction. Ignores that portion of enemy DEF.</summary>
        protected double penRatio { get; set; }

        /// <summary>Base energy regeneration multiplier (1.2 = 120% of the default regen rate).</summary>
        protected double baseEnergyRegen { get; set; }

        /// <summary>Generic damage bonus as a fraction, before elemental or gear bonuses.</summary>
        protected double damageBonus { get; set; }

        /// <summary>
        /// Extra damage bonus that only applies to a specific damage type
        /// (element, anomaly, and similar). Kept separate from <see cref="damageBonus"/>
        /// so a subclass can raise one without raising the other.
        /// </summary>
        protected double specialDamageBonus { get; set; }

        /// <summary>Sheer Force. Used by Rupture agents; other specialties leave this at 0.</summary>
        protected double sheerForce { get; set; }

        /// <summary>
        /// Current Decibels, the resource spent on an Ultimate.
        /// Starts at 0 and is filled by combat; it is not a gear stat.
        /// </summary>
        protected double decibles = 0;

        // ---------------- Equipment and mindscapes ----------------

        /// <summary>
        /// Disk drives currently worn. At most one disk per slot (1-6).
        /// The list is kept sorted by <see cref="Disk_Drive.Drive_Num"/>.
        /// </summary>
        protected List<Disk_Drive> diskDrives = new List<Disk_Drive>();

        /// <summary>Equipped W-Engine, or <c>null</c> when the agent is unarmed.</summary>
        protected Weapon? wEngine;

        /// <summary>
        /// Placeholder for the W-Engine passive. Concrete passive data lives on
        /// <see cref="Weapon"/> today; this field is reserved for agent-side overrides.
        /// </summary>
        protected object weapon_Passive;

        /// <summary>
        /// How many mindscapes are unlocked, from 0 (none) through 6 (all).
        /// Mindscape N unlocks every mindscape from 1 through N.
        /// </summary>
        protected int mindscapeNum { get; set; }

        /// <summary>
        /// Per-mindscape unlock flags. Index 0 is Mindscape 1, index 5 is Mindscape 6.
        /// Filled by <see cref="activatMindscapes"/>; left all-false until that runs.
        /// </summary>
        protected List<bool> mindscapes = new List<bool>([false, false, false, false, false, false]);

        /// <summary>
        /// Records identity only. Base stats are assigned by the subclass after
        /// this constructor returns, because each agent has its own level curve.
        /// </summary>
        /// <param name="characterID">In-game agent id.</param>
        /// <param name="name">Display name.</param>
        /// <param name="rank">Rarity letter, typically <c>'A'</c> or <c>'S'</c>.</param>
        /// <param name="specialty">Combat role.</param>
        /// <param name="attackType">Basic-attack motion type.</param>
        /// <param name="atkElement">Damage element.</param>
        /// <param name="Mnum">Mindscapes unlocked, 0 through 6. Not validated here; see <see cref="activatMindscapes"/>.</param>
        protected Character(int characterID, string name, char rank,
                             Specialty specialty, attackType attackType, atkElement atkElement, int Mnum)
        {
            this.characterID = characterID;
            this.name = name;
            this.rank = rank;
            this.specialty = specialty;
            this.attackType = attackType;
            this.atkElement = atkElement;
            this.mindscapeNum = Mnum;
        }

        /// <summary>
        /// Returns the display name.
        /// </summary>
        /// <exception cref="Exception">Thrown when <see cref="name"/> was never assigned.</exception>
        public string getName()
        {
            if (name != null)
                return name;
            throw new Exception("Character not Initalized");
        }

        /// <summary>
        /// Turns <see cref="mindscapeNum"/> into the <see cref="mindscapes"/> flag list.
        /// A value of 3 sets indices 0, 1, and 2 (Mindscapes 1-3) and leaves the rest false.
        /// Call this once after construction, before any mindscape effect is read.
        /// </summary>
        /// <exception cref="Exception">Thrown when <see cref="mindscapeNum"/> is outside 0-6.</exception>
        protected void activatMindscapes()
        {
            if (mindscapeNum > 6 || mindscapeNum < 0)
            {
                throw new Exception("Mindscapes must be between 0 and 6");
            }
            for (int i = 0; i < mindscapeNum; i++)
            {
                mindscapes[i] = true;
            }
        }

        // ---------------- Mindscapes ----------------
        // One method per cinema level. Subclasses replace the empty Buff with the
        // real effect. Mindscape1 is the template; higher cinemas are not wired up yet.

        /// <summary>
        /// Mindscape 1 effect. Returns an empty <see cref="Buff"/> until a subclass
        /// supplies the real bonus (stat, damage type, condition, and target).
        /// </summary>
        protected  Buff Mindscape1()
        {
            Buff M1 = new Buff();
            return M1;
        }


        // ---------------- Equipping ----------------
        // A W-Engine is a single slot. Disk drives are six independent slots;
        // equipping a disk into an occupied slot replaces the previous disk.

        /// <summary>The equipped W-Engine, or <c>null</c> if none is equipped.</summary>
        public Weapon? GetEquippedWeapon()
        {
            return wEngine;
        }

        /// <summary>
        /// Disk drives currently equipped, in slot order. The returned list is
        /// read-only; mutate gear through <see cref="EquipDisk"/> and <see cref="UnequipDisk"/>.
        /// </summary>
        public IReadOnlyList<Disk_Drive> GetEquippedDisks()
        {
            return diskDrives.AsReadOnly();
        }

        /// <summary>
        /// The disk in <paramref name="slot"/> (1-6), or <c>null</c> if that slot is empty.
        /// </summary>
        public Disk_Drive? GetDiskInSlot(int slot)
        {
            return diskDrives.FirstOrDefault(d => d.Drive_Num == slot);
        }

        /// <summary>
        /// Equips <paramref name="weapon"/>, replacing any W-Engine already worn.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="weapon"/> is null.</exception>
        public void EquipWEngine(Weapon weapon)
        {
            wEngine = weapon ?? throw new ArgumentNullException(nameof(weapon));
        }

        /// <summary>
        /// Removes the equipped W-Engine and returns it so the caller can put it back in inventory.
        /// Returns <c>null</c> when nothing was equipped.
        /// </summary>
        public Weapon? UnequipWEngine()
        {
            Weapon? previous = wEngine;
            wEngine = null;
            return previous;
        }

        /// <summary>
        /// Equips a disk drive. Replaces any disk already in the same slot (1-6).
        /// After the swap the list is re-sorted so slot 1 comes first.
        /// </summary>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="disk"/> is null.</exception>
        /// <exception cref="ArgumentException">Thrown when <see cref="Disk_Drive.Drive_Num"/> is outside 1-6.</exception>
        public void EquipDisk(Disk_Drive disk)
        {
            if (disk == null) throw new ArgumentNullException(nameof(disk));
            if (disk.Drive_Num < 1 || disk.Drive_Num > 6)
                throw new ArgumentException("Disk slot must be between 1 and 6.");

            diskDrives.RemoveAll(d => d.Drive_Num == disk.Drive_Num);
            diskDrives.Add(disk);
            diskDrives.Sort((a, b) => a.Drive_Num.CompareTo(b.Drive_Num));
        }

        /// <summary>
        /// Removes the disk in <paramref name="slot"/> and returns it.
        /// Returns <c>null</c> and leaves the list unchanged when the slot is empty.
        /// </summary>
        public Disk_Drive? UnequipDisk(int slot)
        {
            Disk_Drive? disk = GetDiskInSlot(slot);
            if (disk != null)
                diskDrives.Remove(disk);
            return disk;
        }

        // ---------------- Stat calculation ----------------
        // Gear contributes two kinds of bonus:
        //   percent stats (ATK%, HP%, ...) multiply the matching base stat
        //   flat stats (ATK, HP, DEF, AP, crit, PEN ratio, ...) are added afterward
        // W-Engine base ATK is NOT in this dictionary; PrintSummary adds it onto base ATK
        // before the ATK% multiplier. See GetEquipmentBonuses for what is included.

        /// <summary>
        /// Sums every bonus coming from the W-Engine advanced stat and all equipped disk drives.
        /// W-Engine base ATK is excluded; read that from <see cref="Weapon.get_Base_Stats"/> separately.
        /// Disk contribution is main stat plus each substat roll
        /// (<see cref="Disk_Drive.SubstatRollValue"/> times the number of rolls).
        /// Missing stats are simply absent from the dictionary, not stored as zero.
        /// </summary>
        public Dictionary<advancedStats, double> GetEquipmentBonuses()
        {
            var totals = new Dictionary<advancedStats, double>();

            if (wEngine != null)
            {
                var (_, advanced) = wEngine.get_Base_Stats();
                foreach (var kv in advanced)
                    AddStat(totals, kv.Key, kv.Value);
            }

            foreach (Disk_Drive disk in diskDrives)
            {
                AddStat(totals, disk.MainStat, disk.MainStatValue);
                foreach (var sub in disk.GetSubstats())
                    AddStat(totals, sub.Stat, Disk_Drive.SubstatRollValue(sub.Stat) * sub.NumRoll);
            }

            return totals;
        }

        /// <summary>
        /// Adds <paramref name="value"/> into <paramref name="totals"/> for <paramref name="stat"/>,
        /// starting from 0 when the stat has not been seen yet. Same-stat bonuses from
        /// the W-Engine, main stats, and substats all collapse into one total.
        /// </summary>
        private static void AddStat(Dictionary<advancedStats, double> totals, advancedStats stat, double value)
        {
            totals[stat] = (totals.TryGetValue(stat, out double current) ? current : 0) + value;
        }

        /// <summary>
        /// Reads <paramref name="stat"/> from an equipment-bonus dictionary, or 0 when that stat has no gear.
        /// </summary>
        private static double Get(Dictionary<advancedStats, double> totals, advancedStats stat)
        {
            return totals.TryGetValue(stat, out double v) ? v : 0;
        }

        /// <summary>
        /// Returns how many pieces of each drive set are equipped.
        /// Two pieces activate the 2-piece bonus; four pieces activate both the 2-piece and the 4-piece.
        /// Slot count is the number of disks, not a separate set-bonus calculation.
        /// </summary>
        public Dictionary<driveSet, int> GetSetCounts()
        {
            return diskDrives
                .GroupBy(d => d.DriveSet)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        /// <summary>
        /// Reserved for subclass setup that must run after gear is equipped
        /// (mindscape activation, passive registration, and similar). No work is done yet.
        /// </summary>
        public void setupCharacter()
        {
            //
        }

        /// <summary>
        /// Prints identity, equipped gear, set-piece counts, and final combat stats.
        /// </summary>
        /// <remarks>
        /// Final stats follow the standard split between percent and flat gear:
        /// <list type="bullet">
        /// <item>HP  = baseHP * (1 + HP%) + flat HP</item>
        /// <item>ATK = (baseATK + W-Engine base ATK) * (1 + ATK%) + flat ATK</item>
        /// <item>DEF = baseDEF * (1 + DEF%) + flat DEF</item>
        /// <item>Impact = baseImpact * (1 + Impact%). Anomaly Mastery and Energy Regen use the same shape.</item>
        /// <item>Crit rate, crit damage, Anomaly Proficiency, PEN ratio, and damage bonus are base + flat gear.</item>
        /// </list>
        /// Set bonuses are reported as piece counts only. Their stat effects are not folded into these totals yet.
        /// Ratios are printed with the percent format (0.05 displays as 5.0%).
        /// </remarks>
        public void PrintSummary()
        {
            var bonus = GetEquipmentBonuses();
            double weaponATK = wEngine != null ? wEngine.get_Base_Stats().Item1 : 0;

            // Percent multiplier applies to the base (and, for ATK, the W-Engine base).
            // Flat bonuses from disks are added after the multiply so they are not scaled by %.
            double finalHP = baseHP * (1 + Get(bonus, advancedStats.HP_Percent)) + Get(bonus, advancedStats.HP);
            double finalATK = (baseATK + weaponATK) * (1 + Get(bonus, advancedStats.ATK_Percent)) + Get(bonus, advancedStats.ATK);
            double finalDEF = baseDEF * (1 + Get(bonus, advancedStats.DEF_Percent)) + Get(bonus, advancedStats.DEF);
            double finalImpact = baseImpact * (1 + Get(bonus, advancedStats.Impact));
            double finalCR = critRate + Get(bonus, advancedStats.Crit_Rate);
            double finalCD = critDMG + Get(bonus, advancedStats.Crit_Damage);
            double finalAP = AnomalyProficiency + Get(bonus, advancedStats.Anomaly_Proficiency);
            double finalAM = AnomalyMastery * (1 + Get(bonus, advancedStats.Anomaly_Mastery));
            double finalPen = penRatio + Get(bonus, advancedStats.PEN_Ratio);
            double finalER = baseEnergyRegen * (1 + Get(bonus, advancedStats.EnergyRegen));
            double finalDmgBonus = damageBonus + Get(bonus, advancedStats.Damage_bonus);

            Console.WriteLine("==============================================");
            Console.WriteLine($" {name}  [{rank}]  {specialty} / {attackType} / {atkElement}  (ID {characterID})");
            Console.WriteLine("==============================================");
            Console.WriteLine($" W-Engine : {(wEngine != null ? wEngine.get_Name() : "None")}");
            Console.WriteLine($" Disks    : {diskDrives.Count}/6 equipped");
            foreach (var d in diskDrives)
                Console.WriteLine($"   Slot {d.Drive_Num}: {d.DriveSet,-22} Main: {d.MainStat} ({d.MainStatValue})");

            Console.WriteLine(" Set bonuses:");
            foreach (var kv in GetSetCounts())
                Console.WriteLine($"   {kv.Key}: {kv.Value} pc {(kv.Value >= 4 ? "(4pc active)" : kv.Value >= 2 ? "(2pc active)" : "")}");

            Console.WriteLine("----------------------------------------------");
            Console.WriteLine($" HP                  : {finalHP:F0}");
            Console.WriteLine($" ATK                 : {finalATK:F0}");
            Console.WriteLine($" DEF                 : {finalDEF:F0}");
            Console.WriteLine($" Impact              : {finalImpact:F1}");
            Console.WriteLine($" Crit Rate           : {finalCR:P1}");
            Console.WriteLine($" Crit DMG            : {finalCD:P1}");
            Console.WriteLine($" Anomaly Proficiency : {finalAP:F0}");
            Console.WriteLine($" Anomaly Mastery     : {finalAM:F1}");
            Console.WriteLine($" PEN Ratio           : {finalPen:P1}");
            Console.WriteLine($" Energy Regen        : {finalER:F2}");
            Console.WriteLine($" DMG Bonus           : {finalDmgBonus:P1}");
            Console.WriteLine("==============================================");
        }
    }
}
