using System;
using System.Collections.Generic;
using System.Linq;

namespace Zenless
{
    // ------------------------------------------------------------------
    // DISK DRIVES
    // ------------------------------------------------------------------
    abstract partial class Disk_Drive
    {
        protected int diskID;
        public int DiskID => diskID;

        /// <summary>Optional disk art shown in the windowed interface. Empty until an image is chosen.</summary>
        public string? ImagePath { get; set; }

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
            if (MainStat != advancedStats.nill && DriveSubStats.ContainsKey(MainStat))
                throw new ArgumentException($"{StatName(MainStat)} is the main stat, so it cannot be a substat.");

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
            DefineSetBonuses();
        }

        private static Buff Stat(advancedStats stat, double value, applyConditions condition, double duration, target who = target.self)
        {
            return new Buff(
                specialResource.nill, 0,
                stat, value,
                damageBonus.nill, 0,
                condition, duration,
                who);
        }

        private static Buff Damage(damageBonus bonus, double amount, applyConditions condition, double duration, target who)
        {
            return new Buff(
                specialResource.nill, 0,
                advancedStats.nill, 0,
                bonus, amount,
                condition, duration,
                who);
        }

        private static Buff Noted(string note)
        {
            Buff buff = new Buff();
            buff.Note = note;
            return buff;
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
        public static Buff[] ActiveSetBuffs(driveSet set, int pieceCount, atkElement wearer, Specialty specialty)
        {
            var active = new List<Buff>();
            if (pieceCount >= 2)
                active.AddRange(TwoPiece(set));
            if (pieceCount >= 4)
            {
                foreach (Buff buff in FourPiece(set))
                {
                    if (AppliesToWearer(set, buff, wearer, specialty))
                        active.Add(buff);
                }
            }
            return active.ToArray();
        }

        /// <summary>
        /// Drops 4-piece clauses that require an element or a specialty the wearer does not have.
        /// </summary>
        public static bool AppliesToWearer(driveSet set, Buff buff, atkElement wearer, Specialty specialty)
        {
            if (set == driveSet.Feathered_Fate
                && buff.damage_buff == damageBonus.Anomaly
                && wearer != atkElement.Lumiflux)
                return false;
            if (set == driveSet.The_Sky_Ablaze
                && buff.statUp == advancedStats.Crit_Damage
                && wearer != atkElement.Ether)
                return false;
            if (set == driveSet.Moonlight_Lullaby && specialty != Specialty.Support)
                return false;
            if (set == driveSet.King_of_the_Summit && specialty != Specialty.Stun)
                return false;
            if (set == driveSet.Bunny_in_Wonderland && specialty != Specialty.Defense)
                return false;
            if (set == driveSet.Dawns_Bloom && buff.buff_Duration >= 25 && specialty != Specialty.Attack)
                return false;
            if (set == driveSet.White_Water_Ballad && buff.buff_Duration >= 30 && specialty != Specialty.Attack)
                return false;
            return true;
        }

        /// <summary>Portrait file for a drive set, under <c>Images/DiskDrives</c>.</summary>
        public static string IconFileName(driveSet set)
        {
            return set switch
            {
                driveSet.Feathered_Fate => "Drive_Disc_Feathered_Fate_Icon.webp",
                driveSet.Freedom_Blues => "Drive_Disc_Freedom_Blues_Icon.webp",
                driveSet.Shadow_Harmony => "Drive_Disc_Shadow_Harmony_Icon.webp",
                driveSet.Branch_And_Blade_Song => "Drive_Disc_Branch_%26_Blade_Song_Icon.webp",
                driveSet.Woodpecker_Electro => "Drive_Disc_Woodpecker_Electro_Icon.webp",
                driveSet.Puffer_Electro => "Drive_Disc_Puffer_Electro_Icon.webp",
                driveSet.Shockstar_Disco => "Drive_Disc_Shockstar_Disco_Icon.webp",
                driveSet.Hormone_Punk => "Drive_Disc_Hormone_Punk_Icon.webp",
                driveSet.Swing_Jazz => "Drive_Disc_Swing_Jazz_Icon.webp",
                driveSet.Soul_Rock => "Drive_Disc_Soul_Rock_Icon.webp",
                driveSet.Inferno_Metal => "Drive_Disc_Inferno_Metal_Icon.webp",
                driveSet.Chaotic_Metal => "Drive_Disc_Chaotic_Metal_Icon.webp",
                driveSet.Thunder_Metal => "Drive_Disc_Thunder_Metal_Icon.webp",
                driveSet.Polar_Metal => "Drive_Disc_Polar_Metal_Icon.webp",
                driveSet.Fanged_Metal => "Drive_Disc_Fanged_Metal_Icon.webp",
                driveSet.Chaos_Jazz => "Drive_Disc_Chaos_Jazz_Icon.webp",
                driveSet.Proto_Punk => "Drive_Disc_Proto_Punk_Icon.webp",
                driveSet.Astral_Voice => "Drive_Disc_Astral_Voice_Icon.webp",
                driveSet.Phaethons_Melody => "Drive_Disc_Phaethon%27s_Melody_Icon.webp",
                driveSet.Yunkui_Tales => "Drive_Disc_Yunkui_Tales_Icon.webp",
                driveSet.King_of_the_Summit => "Drive_Disc_King_of_the_Summit_Icon.webp",
                driveSet.Moonlight_Lullaby => "Drive_Disc_Moonlight_Lullaby_Icon.webp",
                driveSet.Dawns_Bloom => "Drive_Disc_Dawn's_Bloom_Icon.webp",
                driveSet.The_Sky_Ablaze => "Drive_Disc_The_Sky_Ablaze_Icon.webp",
                driveSet.White_Water_Ballad => "Drive_Disc_White_Water_Ballad_Icon.webp",
                driveSet.Thorned_Rose => "Drive_Disc_Thorned_Rose_Icon.webp",
                driveSet.Bunny_in_Wonderland => "Drive_Disc_Bunny_in_Wonderland_Icon.webp",
                driveSet.Shining_Aria => "Drive_Disc_Shining_Aria_Icon.webp",
                driveSet.Wuthering_Salon => "Drive_Disc_Wuthering_Salon_Icon.webp",
                driveSet.Notes_From_the_Chained => "Drive_Disc_Notes_From_the_Chained_Icon.webp",
                _ => throw new ArgumentOutOfRangeException(nameof(set))
            };
        }

        public static string IconPath(driveSet set)
        {
            return Path.Combine(AppContext.BaseDirectory, "Images", "DiskDrives", IconFileName(set));
        }

        public static string SetName(driveSet set)
        {
            return set switch
            {
                driveSet.Branch_And_Blade_Song => "Branch & Blade Song",
                driveSet.Dawns_Bloom => "Dawn's Bloom",
                driveSet.Phaethons_Melody => "Phaethon's Melody",
                _ => set.ToString().Replace('_', ' ')
            };
        }

        /// <summary>Substats a disk is allowed to roll.</summary>
        public static IReadOnlyList<advancedStats> SubstatOptions { get; } = new[]
        {
            advancedStats.HP_Percent,
            advancedStats.ATK_Percent,
            advancedStats.DEF_Percent,
            advancedStats.HP,
            advancedStats.ATK,
            advancedStats.DEF,
            advancedStats.Crit_Rate,
            advancedStats.Crit_Damage,
            advancedStats.Anomaly_Proficiency,
            advancedStats.PEN_Ratio,
            advancedStats.PEN
        };

        /// <summary>
        /// Substats a disk can roll when <paramref name="mainStat"/> is its main stat.
        /// The main stat itself is left out.
        /// </summary>
        public static IReadOnlyList<advancedStats> SubstatOptionsFor(advancedStats mainStat)
        {
            if (mainStat == advancedStats.nill)
                return SubstatOptions;
            return SubstatOptions.Where(stat => stat != mainStat).ToArray();
        }

        /// <summary>Fixed main stat for slots 1-3. Slots 4-6 return nill because the player chooses.</summary>
        public static advancedStats SlotMainStat(int slot)
        {
            switch (slot)
            {
                case 1: return advancedStats.HP;
                case 2: return advancedStats.ATK;
                case 3: return advancedStats.DEF;
                default: return advancedStats.nill;
            }
        }

        public static string StatName(advancedStats stat)
        {
            return stat.ToString().Replace('_', ' ');
        }

        /// <summary>Main stats legal for slots 4-6. Slots 1-3 are fixed by the disk class.</summary>
        public static IReadOnlyList<advancedStats> MainStatOptions(int slot)
        {
            switch (slot)
            {
                case 4:
                    return new[]
                    {
                        advancedStats.Crit_Rate, advancedStats.Crit_Damage, advancedStats.Anomaly_Proficiency,
                        advancedStats.HP_Percent, advancedStats.ATK_Percent, advancedStats.DEF_Percent,
                        advancedStats.HP, advancedStats.ATK, advancedStats.DEF
                    };
                case 5:
                    return new[]
                    {
                        advancedStats.HP_Percent, advancedStats.ATK_Percent, advancedStats.DEF_Percent,
                        advancedStats.HP, advancedStats.ATK, advancedStats.DEF,
                        advancedStats.PEN_Ratio, advancedStats.Damage_bonus
                    };
                case 6:
                    return new[]
                    {
                        advancedStats.HP_Percent, advancedStats.ATK_Percent, advancedStats.DEF_Percent,
                        advancedStats.HP, advancedStats.ATK, advancedStats.DEF,
                        advancedStats.Impact, advancedStats.Anomaly_Mastery, advancedStats.EnergyRegen
                    };
                default:
                    return Array.Empty<advancedStats>();
            }
        }

        /// <summary>
        /// S-rank level 15 main-stat value. Slots 4-6 keep this number once the stat is chosen.
        /// </summary>
        public static double FixedMainValue(advancedStats stat)
        {
            switch (stat)
            {
                case advancedStats.HP: return 2200;
                case advancedStats.ATK: return 316;
                case advancedStats.DEF: return 184;
                case advancedStats.Anomaly_Proficiency: return 92;
                case advancedStats.Crit_Rate:
                case advancedStats.PEN_Ratio:
                    return 0.24;
                case advancedStats.Crit_Damage:
                case advancedStats.DEF_Percent:
                    return 0.48;
                case advancedStats.Impact:
                    return 0.18;
                case advancedStats.EnergyRegen:
                    return 0.60;
                case advancedStats.HP_Percent:
                case advancedStats.ATK_Percent:
                case advancedStats.Damage_bonus:
                case advancedStats.Anomaly_Mastery:
                    return 0.30;
                default:
                    return 0;
            }
        }

        protected void UseSetIcon()
        {
            ImagePath = IconPath(DriveSet);
        }

        private static Buff[] Copy(driveSet set, Dictionary<driveSet, Buff[]> effects)
        {
            if (!effects.TryGetValue(set, out Buff[]? buffs) || buffs.Length == 0)
                return Array.Empty<Buff>();
            return (Buff[])buffs.Clone();
        }
    }
}