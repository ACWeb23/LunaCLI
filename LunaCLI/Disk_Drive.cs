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
        public int DiskID => diskID;

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

        // ------------------------------------------------------------------
        // SET PASSIVES
        // ------------------------------------------------------------------
        // Each drive set defines its passive as two Buff arrays: the 2-piece
        // effect and the 4-piece effect. A character wearing two disks of one
        // set receives that set's 2-piece array. Four disks apply both arrays.
        //
        // Percent values are fractions (0.16 = 16%), matching Character.
        // Flat stats such as Anomaly Proficiency are absolute numbers.
        // When buff_Condition is adv_Stat_Threshold, resourceAmmount is the
        // Anomaly Mastery the wearer must reach for that buff to count.

        private static readonly Dictionary<driveSet, Buff[]> twoPiece = new Dictionary<driveSet, Buff[]>();
        private static readonly Dictionary<driveSet, Buff[]> fourPiece = new Dictionary<driveSet, Buff[]>();

        static Disk_Drive()
        {
            Buff Stat(advancedStats stat, double value, applyConditions condition, double duration)
            {
                return new Buff(
                    specialResource.nill, 0,
                    stat, value,
                    damageBonus.nill, 0,
                    condition, duration,
                    target.self);
            }

            Buff Damage(damageBonus bonus, double amount, applyConditions condition, double duration, target who)
            {
                return new Buff(
                    specialResource.nill, 0,
                    advancedStats.nill, 0,
                    bonus, amount,
                    condition, duration,
                    who);
            }

            // 2pc: Anomaly Proficiency +30.
            // 4pc: On entering battle, Anomaly Proficiency +50 for 15s.
            //      Lumiflux agents also gain Attribute Anomaly DMG +15% for 15s.
            Define(driveSet.Feathered_Fate,
                new[]
                {
                    Stat(advancedStats.Anomaly_Proficiency, 30, applyConditions.Always, 0)
                },
                new[]
                {
                    Stat(advancedStats.Anomaly_Proficiency, 50, applyConditions.EnterBattle, 15),
                    Damage(damageBonus.Anomaly, 0.15, applyConditions.EnterBattle, 15, target.self)
                });

            // 2pc: Anomaly Proficiency +30.
            // 4pc: An EX Special hit lowers the target's Anomaly Buildup RES by 20% for 8s.
            //      bonus_ammount is that resistance cut; the buff targets the enemy.
            Define(driveSet.Freedom_Blues,
                new[]
                {
                    Stat(advancedStats.Anomaly_Proficiency, 30, applyConditions.Always, 0)
                },
                new[]
                {
                    Damage(damageBonus.Anomaly, 0.20, applyConditions.EX_SP_ATK, 8, target.enemy)
                });

            // 2pc: Aftershock and Dash Attack DMG +15%.
            // 4pc: A matching-attribute Aftershock or Dash hit grants one stack for 15s.
            //      Each stack is ATK +4% and Crit Rate +4%, up to 3 stacks.
            //      These buffs are one stack; the cap is not a field on Buff.
            Define(driveSet.Shadow_Harmony,
                new[]
                {
                    Damage(damageBonus.All, 0.15, applyConditions.Aftershock, 0, target.self),
                    Damage(damageBonus.All, 0.15, applyConditions.dashATK, 0, target.self)
                },
                new[]
                {
                    Stat(advancedStats.ATK_Percent, 0.04, applyConditions.Aftershock, 15),
                    Stat(advancedStats.Crit_Rate, 0.04, applyConditions.Aftershock, 15),
                    Stat(advancedStats.ATK_Percent, 0.04, applyConditions.dashATK, 15),
                    Stat(advancedStats.Crit_Rate, 0.04, applyConditions.dashATK, 15)
                });

            // 2pc: Crit DMG +16%.
            // 4pc: Crit DMG +30% while Anomaly Mastery is at least 115.
            //      Freeze or Shatter from any squad member grants Crit Rate +12% for 15s.
            //      The freeze and shatter buffs are the same bonus; they do not stack.
            Define(driveSet.Branch_And_Blade_Song,
                new[]
                {
                    Stat(advancedStats.Crit_Damage, 0.16, applyConditions.Always, 0)
                },
                new[]
                {
                    new Buff(
                        specialResource.nill, 115,
                        advancedStats.Crit_Damage, 0.30,
                        damageBonus.nill, 0,
                        applyConditions.adv_Stat_Threshold, 0,
                        target.self),
                    Stat(advancedStats.Crit_Rate, 0.12, applyConditions.freeze, 15),
                    Stat(advancedStats.Crit_Rate, 0.12, applyConditions.shatter, 15)
                });
        }

        private static void Define(driveSet set, Buff[] two, Buff[] four)
        {
            twoPiece[set] = two;
            fourPiece[set] = four;
        }

        /// <summary>
        /// 2-piece buffs for <paramref name="set"/>. Empty when that set has none.
        /// </summary>
        public static Buff[] TwoPiece(driveSet set)
        {
            return Copy(set, twoPiece);
        }

        /// <summary>
        /// 4-piece buffs for <paramref name="set"/>. Empty when that set has none.
        /// </summary>
        public static Buff[] FourPiece(driveSet set)
        {
            return Copy(set, fourPiece);
        }

        /// <summary>2-piece buffs for this disk's set.</summary>
        public Buff[] TwoPieceEffect => TwoPiece(DriveSet);

        /// <summary>4-piece buffs for this disk's set.</summary>
        public Buff[] FourPieceEffect => FourPiece(DriveSet);

        /// <summary>
        /// Buffs granted by wearing <paramref name="pieceCount"/> disks of <paramref name="set"/>.
        /// Two or more pieces include the 2-piece array. Four or more include both arrays.
        /// The result is one copy of each effect, not one copy per disk.
        /// </summary>
        public static Buff[] ActiveSetBuffs(driveSet set, int pieceCount, atkElement wearer)
        {
            var active = new List<Buff>();
            if (pieceCount >= 2)
                active.AddRange(TwoPiece(set));
            if (pieceCount >= 4)
            {
                foreach (Buff buff in FourPiece(set))
                {
                    if (AppliesToWearer(set, buff, wearer))
                        active.Add(buff);
                }
            }
            return active.ToArray();
        }

        /// <summary>
        /// Feathered Fate's anomaly-damage clause applies only to Lumiflux agents.
        /// Every other set buff applies to any wearer.
        /// </summary>
        public static bool AppliesToWearer(driveSet set, Buff buff, atkElement wearer)
        {
            if (set == driveSet.Feathered_Fate
                && buff.damage_buff == damageBonus.Anomaly
                && wearer != atkElement.Lumiflux)
                return false;
            return true;
        }

        private static Buff[] Copy(driveSet set, Dictionary<driveSet, Buff[]> effects)
        {
            if (!effects.TryGetValue(set, out Buff[]? buffs) || buffs.Length == 0)
                return Array.Empty<Buff>();
            return (Buff[])buffs.Clone();
        }
    }
}