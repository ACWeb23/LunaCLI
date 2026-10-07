using System;
using System.Collections.Generic;
using System.Linq;

namespace Zenless
{
    // ------------------------------------------------------------------
    // PROGRAM
    // ------------------------------------------------------------------
    class Program
    {
        public static void Main(string[] args)
        {
            Player george = LoadGeorge();
            RunMenu(george);
        }

        /// <summary>
        /// Builds player George with Remielle, two Ode of Resurrected Wings, and ten unique disk drives.
        /// Six disks and one W-Engine are equipped on Remielle; the rest stay in inventory.
        /// </summary>
        private static Player LoadGeorge()
        {
            Player george = new Player("George");
            Remielle remielle = new Remielle(0);
            george.AddCharacter(remielle);

            Weapon equippedWings = new Ode_Of_Resurrected_Wings();
            Weapon spareWings = new Ode_Of_Resurrected_Wings();
            george.AddWeapon(equippedWings);
            george.AddWeapon(spareWings);
            george.EquipWeapon(remielle, equippedWings);

            Disk_Drive slot1 = george.CreateDiskDrive(driveSet.Feathered_Fate, Subs(
                advancedStats.DEF_Percent, 2,
                advancedStats.Crit_Rate, 1,
                advancedStats.Anomaly_Proficiency, 2,
                advancedStats.ATK_Percent, 4), 1);
            Disk_Drive slot2 = george.CreateDiskDrive(driveSet.Freedom_Blues, Subs(
                advancedStats.ATK_Percent, 3,
                advancedStats.Crit_Damage, 1,
                advancedStats.Anomaly_Proficiency, 3,
                advancedStats.DEF_Percent, 1), 2);
            Disk_Drive slot3 = george.CreateDiskDrive(driveSet.Feathered_Fate, Subs(
                advancedStats.ATK, 2,
                advancedStats.Anomaly_Proficiency, 2,
                advancedStats.ATK_Percent, 2,
                advancedStats.Crit_Rate, 2), 3);
            Disk_Drive slot4 = george.CreateDiskDrive(driveSet.Feathered_Fate, Subs(
                advancedStats.DEF, 2,
                advancedStats.ATK, 1,
                advancedStats.ATK_Percent, 2,
                advancedStats.HP, 3), 4, advancedStats.Anomaly_Proficiency, 92.0);
            Disk_Drive slot5 = george.CreateDiskDrive(driveSet.Freedom_Blues, Subs(
                advancedStats.Anomaly_Proficiency, 3,
                advancedStats.HP, 2,
                advancedStats.DEF, 1,
                advancedStats.Crit_Damage, 2), 5, advancedStats.ATK_Percent, 0.30);
            Disk_Drive slot6 = george.CreateDiskDrive(driveSet.Feathered_Fate, Subs(
                advancedStats.ATK, 3,
                advancedStats.Anomaly_Proficiency, 2,
                advancedStats.Crit_Damage, 2,
                advancedStats.DEF_Percent, 1), 6, advancedStats.ATK_Percent, 0.30);

            george.EquipDisk(remielle, slot1);
            george.EquipDisk(remielle, slot2);
            george.EquipDisk(remielle, slot3);
            george.EquipDisk(remielle, slot4);
            george.EquipDisk(remielle, slot5);
            george.EquipDisk(remielle, slot6);

            george.CreateDiskDrive(driveSet.Shadow_Harmony, Subs(
                advancedStats.Crit_Rate, 3,
                advancedStats.Crit_Damage, 3,
                advancedStats.ATK_Percent, 2,
                advancedStats.PEN, 1), 1);
            george.CreateDiskDrive(driveSet.Branch_And_Blade_Song, Subs(
                advancedStats.Crit_Damage, 4,
                advancedStats.Crit_Rate, 2,
                advancedStats.DEF_Percent, 1,
                advancedStats.HP_Percent, 1), 2);
            george.CreateDiskDrive(driveSet.Shadow_Harmony, Subs(
                advancedStats.ATK, 2,
                advancedStats.HP, 2,
                advancedStats.Anomaly_Proficiency, 3,
                advancedStats.PEN_Ratio, 1), 4, advancedStats.Crit_Rate, 0.24);
            george.CreateDiskDrive(driveSet.Freedom_Blues, Subs(
                advancedStats.Anomaly_Proficiency, 4,
                advancedStats.ATK_Percent, 2,
                advancedStats.DEF, 1,
                advancedStats.HP, 2), 5, advancedStats.Damage_bonus, 0.30);

            return george;
        }

        /// <summary>
        /// Builds a four-stat substat map. Callers must keep the roll total at 8 or 9.
        /// </summary>
        private static Dictionary<advancedStats, int> Subs(
            advancedStats stat1, int rolls1,
            advancedStats stat2, int rolls2,
            advancedStats stat3, int rolls3,
            advancedStats stat4, int rolls4)
        {
            return new Dictionary<advancedStats, int>
            {
                { stat1, rolls1 },
                { stat2, rolls2 },
                { stat3, rolls3 },
                { stat4, rolls4 }
            };
        }

        /// <summary>
        /// Main loop: inspect inventory, inspect the roster, or exit.
        /// </summary>
        private static void RunMenu(Player player)
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine($"=== {player.Name} ===");
                Console.WriteLine("1. View inventory");
                Console.WriteLine("2. View characters");
                Console.WriteLine("3. Exit");
                Console.Write("Select an option: ");

                string? choice = Console.ReadLine();
                switch (choice)
                {
                    case "1":
                        PrintInventory(player);
                        break;
                    case "2":
                        ViewCharacters(player);
                        break;
                    case "3":
                        return;
                    default:
                        Console.WriteLine("Unknown option.");
                        break;
                }
            }
        }

        /// <summary>
        /// Prints every owned weapon and disk, split into equipped and unequipped.
        /// </summary>
        private static void PrintInventory(Player player)
        {
            Console.WriteLine();
            Console.WriteLine($"--- {player.Name}'s Inventory ---");
            Console.WriteLine($"Weapons ({player.Weapons.Count} owned, {player.UnequippedWeapons.Count} unequipped)");
            foreach (Weapon weapon in player.Weapons)
            {
                Character? holder = FindWeaponHolder(player, weapon);
                string status = holder != null ? $"equipped on {holder.getName()}" : "unequipped";
                Console.WriteLine($"  {weapon.get_Name()}  [{status}]");
            }

            Console.WriteLine();
            Console.WriteLine($"Disk drives ({player.DiskDrives.Count} owned, {player.UnequippedDiskDrives.Count} unequipped)");
            foreach (Disk_Drive disk in player.DiskDrives.OrderBy(d => d.DiskID))
            {
                Character? holder = FindDiskHolder(player, disk);
                string status = holder != null ? $"equipped on {holder.getName()}" : "unequipped";
                Console.WriteLine($"  ID {disk.DiskID}  Slot {disk.Drive_Num}  {disk.DriveSet}  Main {disk.MainStat} ({disk.MainStatValue})  [{status}]");
                foreach (var sub in disk.GetSubstats())
                    Console.WriteLine($"      {sub.Stat}: {sub.NumRoll} roll(s)");
            }
        }

        /// <summary>
        /// Lists roster characters and lets the user open one to see stats, disks, and weapon.
        /// </summary>
        private static void ViewCharacters(Player player)
        {
            List<Character> roster = player.Characters.ToList();
            if (roster.Count == 0)
            {
                Console.WriteLine("No characters in the roster.");
                return;
            }

            while (true)
            {
                Console.WriteLine();
                Console.WriteLine($"--- {player.Name}'s Characters ---");
                for (int i = 0; i < roster.Count; i++)
                    Console.WriteLine($"{i + 1}. {roster[i].getName()}");
                Console.WriteLine("0. Back");
                Console.Write("Select a character: ");

                string? input = Console.ReadLine();
                if (input == "0")
                    return;
                if (!int.TryParse(input, out int index) || index < 1 || index > roster.Count)
                {
                    Console.WriteLine("Unknown option.");
                    continue;
                }

                roster[index - 1].PrintSummary();
            }
        }

        private static Character? FindWeaponHolder(Player player, Weapon weapon)
        {
            return player.Characters.FirstOrDefault(character => player.GetEquippedWeapon(character) == weapon);
        }

        private static Character? FindDiskHolder(Player player, Disk_Drive disk)
        {
            return player.Characters.FirstOrDefault(character => player.GetEquippedDisks(character).Contains(disk));
        }
    }
}
