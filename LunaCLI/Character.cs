using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography.X509Certificates;

namespace Zenless
{
    // ------------------------------------------------------------------
    // CHARACTER
    // ------------------------------------------------------------------
    abstract class Character
    {
        public int characterID { get; protected set; }
        public string name { get; protected set; }
        public char rank { get; protected set; }
        public Specialty specialty { get; protected set; }
        public attackType attackType { get; protected set; }
        public atkElement atkElement { get; protected set; }

        // Character Stats
        protected double baseHP { get; set; }
        protected double baseATK { get; set; }
        protected double baseDEF { get; set; }
        protected double baseImpact { get; set; }
        protected double critRate { get; set; }
        protected double critDMG { get; set; }
        protected double AnomalyMastery { get; set; }
        protected double AnomalyProficiency { get; set; }
        protected double penRatio { get; set; }
        protected double baseEnergyRegen { get; set; }
        protected double damageBonus { get; set; }
        protected double specialDamageBonus { get; set; }
        protected double sheerForce { get; set; }

        protected double decibles = 0;

        // Equipable items
        protected List<Disk_Drive> diskDrives = new List<Disk_Drive>();
        protected Weapon wEngine;
        protected object weapon_Passive;
        protected int mindscapeNum { get; set; }
        protected List<bool> mindscapes = new List<bool>([false, false, false, false, false, false]);

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

        public string getName()
        {
            if (name != null)
                return name;
            throw new Exception("Character not Initalized");
        }

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

        protected  Buff Mindscape1()
        {
            Buff M1 = new Buff();
            return M1;
        }
        

        // ---------------- Equipping ----------------

        public void EquipWEngine(Weapon weapon)
        {
            wEngine = weapon ?? throw new ArgumentNullException(nameof(weapon));
        }

        /// <summary>
        /// Equips a disk drive. Replaces any disk already in the same slot (1-6).
        /// </summary>
        public void EquipDisk(Disk_Drive disk)
        {
            if (disk == null) throw new ArgumentNullException(nameof(disk));
            if (disk.Drive_Num < 1 || disk.Drive_Num > 6)
                throw new ArgumentException("Disk slot must be between 1 and 6.");

            diskDrives.RemoveAll(d => d.Drive_Num == disk.Drive_Num);
            diskDrives.Add(disk);
            diskDrives.Sort((a, b) => a.Drive_Num.CompareTo(b.Drive_Num));
        }

        // ---------------- Stat calculation ----------------

        /// <summary>
        /// Sums every bonus coming from the W-Engine and all equipped disk drives.
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

        private static void AddStat(Dictionary<advancedStats, double> totals, advancedStats stat, double value)
        {
            totals[stat] = (totals.TryGetValue(stat, out double current) ? current : 0) + value;
        }

        private static double Get(Dictionary<advancedStats, double> totals, advancedStats stat)
        {
            return totals.TryGetValue(stat, out double v) ? v : 0;
        }

        /// <summary>
        /// Returns how many pieces of each drive set are equipped.
        /// </summary>
        public Dictionary<driveSet, int> GetSetCounts()
        {
            return diskDrives
                .GroupBy(d => d.DriveSet)
                .ToDictionary(g => g.Key, g => g.Count());
        }

        public void setupCharacter()
        {
            //
        }

        public void PrintSummary()
        {
            var bonus = GetEquipmentBonuses();
            double weaponATK = wEngine != null ? wEngine.get_Base_Stats().Item1 : 0;

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
