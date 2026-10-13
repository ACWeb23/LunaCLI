using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Threading;
using Microsoft.UI.Dispatching;
using WinRT;

namespace Zenless
{
    // ------------------------------------------------------------------
    // PROGRAM
    // ------------------------------------------------------------------
    class Program
    {
        private const int AttachParentProcess = -1;

        /// <summary>
        /// WinUI window by default. --terminal opens the text menu.
        /// </summary>
        [STAThread]
        public static int Main(string[] args)
        {
            if (!TryChooseInterface(args, out bool useTerminal, out bool helpOnly, out string? error))
            {
                EnsureConsole();
                if (!string.IsNullOrEmpty(error))
                    Console.WriteLine(error);
                PrintUsage();
                return helpOnly ? 0 : 1;
            }

            if (useTerminal)
            {
                EnsureConsole();
                RunMenu(Watch(UserStore.LoadActiveOrSeed(LoadStar)));
                return 0;
            }

            return RunWindow();
        }

        /// <summary>
        /// No arguments open the window. --terminal / -t opens the text menu.
        /// </summary>
        private static bool TryChooseInterface(string[] args, out bool useTerminal, out bool helpOnly, out string? error)
        {
            useTerminal = false;
            helpOnly = false;
            error = null;
            bool window = false;
            bool help = false;
            var unknown = new List<string>();

            foreach (string arg in args)
            {
                switch (arg.ToLowerInvariant())
                {
                    case "--ui":
                    case "-u":
                    case "/ui":
                        window = true;
                        break;
                    case "--terminal":
                    case "-t":
                    case "/terminal":
                        useTerminal = true;
                        break;
                    case "--help":
                    case "-h":
                    case "-?":
                    case "/?":
                    case "/help":
                        help = true;
                        break;
                    default:
                        unknown.Add(arg);
                        break;
                }
            }

            if (unknown.Count > 0)
                error = "Unknown argument: " + string.Join(" ", unknown);
            else if (window && useTerminal)
                error = "Use either --ui or --terminal.";

            if (help && unknown.Count == 0 && !(window && useTerminal))
            {
                helpOnly = true;
                return false;
            }

            return error == null;
        }

        private static void PrintUsage()
        {
            Console.WriteLine("LunaCLI [--terminal]");
            Console.WriteLine();
            Console.WriteLine("  (no arguments)   Windowed interface");
            Console.WriteLine("  --ui, -u         Windowed interface");
            Console.WriteLine("  --terminal, -t   Text menu in this console");
            Console.WriteLine("  --help, -h       Show this help");
        }

        /// <summary>
        /// Starts the WinUI message loop. The console menu is not shown in this mode.
        /// </summary>
        private static int RunWindow()
        {
            try
            {
                App.UsePlayer(UserStore.LoadActiveOrSeed(LoadStar));
                ComWrappersSupport.InitializeComWrappers();
                Microsoft.UI.Xaml.Application.Start(callback =>
                {
                    var context = new DispatcherQueueSynchronizationContext(DispatcherQueue.GetForCurrentThread());
                    SynchronizationContext.SetSynchronizationContext(context);
                    new App();
                });
                return 0;
            }
            catch (Exception ex)
            {
                EnsureConsole();
                Console.Error.WriteLine(ex);
                return 1;
            }
        }

        /// <summary>
        /// A windowed build has no console until one is attached. A console build already has one.
        /// </summary>
        private static void EnsureConsole()
        {
            if (GetConsoleWindow() != IntPtr.Zero)
                return;

            if (!AttachConsole(AttachParentProcess))
                AllocConsole();

            Console.SetOut(new StreamWriter(Console.OpenStandardOutput()) { AutoFlush = true });
            Console.SetError(new StreamWriter(Console.OpenStandardError()) { AutoFlush = true });
            Console.SetIn(new StreamReader(Console.OpenStandardInput()));
        }

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AttachConsole(int dwProcessId);

        [DllImport("kernel32.dll", SetLastError = true)]
        private static extern bool AllocConsole();

        [DllImport("kernel32.dll")]
        private static extern IntPtr GetConsoleWindow();

        /// <summary>
        /// Follows later roster and inventory edits and writes them to this user's save file.
        /// </summary>
        private static Player Watch(Player player)
        {
            player.Changed += () => UserStore.Save(player);
            return player;
        }

        /// <summary>
        /// Saves the user who is open, then follows <paramref name="next"/>.
        /// </summary>
        private static Player Activate(Player current, Player next)
        {
            UserStore.Save(current);
            UserStore.SetActive(next.Name);
            return Watch(next);
        }

        /// <summary>
        /// First-run account. Later launches read Star from the save file instead of building this again.
        /// </summary>
        private static Player LoadStar()
        {
            Player star = new Player("Star");
            star.AddCharacter(new AsabaHarumasa(0));
            star.AddCharacter(new Banyue(0));
            star.AddCharacter(new HoshimiMiyabi(0));
            star.AddCharacter(new Soldier0Anby(0));
            return star;
        }

        /// <summary>
        /// Main loop: inspect the roster, add an agent, create a disk, equip a disk, switch users, or exit.
        /// </summary>
        private static void RunMenu(Player player)
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine($"=== {player.Name} ===");
                Console.WriteLine("1. View inventory");
                Console.WriteLine("2. View characters");
                Console.WriteLine("3. Add character");
                Console.WriteLine("4. Create disk drive");
                Console.WriteLine("5. Equip disk drive");
                Console.WriteLine("6. New user");
                Console.WriteLine("7. Switch user");
                Console.WriteLine("8. Exit");
                Console.Write("Select an option: ");

                switch (Console.ReadLine())
                {
                    case "1":
                        PrintInventory(player);
                        break;
                    case "2":
                        ViewCharacters(player);
                        break;
                    case "3":
                        AddCharacter(player);
                        break;
                    case "4":
                        CreateDisk(player);
                        break;
                    case "5":
                        EquipDiskMenu(player);
                        break;
                    case "6":
                        player = CreateUser(player);
                        break;
                    case "7":
                        player = ChooseUser(player);
                        break;
                    case "8":
                        UserStore.Save(player);
                        return;
                    default:
                        Console.WriteLine("Unknown option.");
                        break;
                }
            }
        }

        private static Player CreateUser(Player current)
        {
            Console.Write("New user name: ");
            string? name = Console.ReadLine();
            if (string.IsNullOrWhiteSpace(name))
            {
                Console.WriteLine("User name is required.");
                return current;
            }

            try
            {
                Player created = UserStore.Create(name);
                Console.WriteLine($"Created {created.Name}.");
                return Activate(current, created);
            }
            catch (Exception ex) when (ex is ArgumentException or InvalidOperationException or IOException)
            {
                Console.WriteLine(ex.Message);
                return current;
            }
        }

        private static Player ChooseUser(Player current)
        {
            List<string> names = UserStore.ListNames();
            if (names.Count == 0)
            {
                Console.WriteLine("No saved users.");
                return current;
            }

            Console.WriteLine();
            Console.WriteLine("--- Switch user ---");
            for (int i = 0; i < names.Count; i++)
            {
                string mark = string.Equals(names[i], current.Name, StringComparison.OrdinalIgnoreCase) ? "  (current)" : "";
                Console.WriteLine($"{i + 1}. {names[i]}{mark}");
            }
            Console.WriteLine("0. Back");
            int index = ReadChoice("Select a user: ", names.Count);
            if (index == 0)
                return current;

            try
            {
                Player selected = UserStore.Load(names[index - 1]);
                Console.WriteLine($"Opened {selected.Name}.");
                return Activate(current, selected);
            }
            catch (Exception ex) when (ex is InvalidDataException or IOException or ArgumentException or InvalidOperationException)
            {
                Console.WriteLine(ex.Message);
                return current;
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

        private static void AddCharacter(Player player)
        {
            List<Character> missing = CharacterRoster.CreateAll()
                .Where(agent => player.GetCharacter(agent.characterID) == null)
                .OrderBy(agent => agent.getName())
                .ToList();
            if (missing.Count == 0)
            {
                Console.WriteLine("Every agent is already in the roster.");
                return;
            }

            Console.WriteLine();
            Console.WriteLine("--- Add character ---");
            for (int i = 0; i < missing.Count; i++)
            {
                Character agent = missing[i];
                Console.WriteLine($"{i + 1}. {agent.getName()}  [{agent.rank}]  {agent.specialty} / {agent.atkElement}");
            }
            Console.WriteLine("0. Back");
            int index = ReadChoice("Select an agent: ", missing.Count);
            if (index == 0)
                return;

            int mindscapes = ReadBounded("Mindscapes unlocked (0-6): ", 0, 6);
            Character created = CharacterRoster.Create(missing[index - 1].characterID, mindscapes);
            player.AddCharacter(created);
            Console.WriteLine($"Added {created.getName()}.");
        }

        private static void CreateDisk(Player player)
        {
            driveSet[] sets = Enum.GetValues<driveSet>().OrderBy(Disk_Drive.SetName).ToArray();
            Console.WriteLine();
            Console.WriteLine("--- Create disk drive ---");
            for (int i = 0; i < sets.Length; i++)
                Console.WriteLine($"{i + 1}. {Disk_Drive.SetName(sets[i])}");
            Console.WriteLine("0. Back");
            int setIndex = ReadChoice("Select a drive set: ", sets.Length);
            if (setIndex == 0)
                return;

            int slot = ReadBounded("Slot (1-6): ", 1, 6);
            advancedStats main = advancedStats.nill;
            double mainValue = 0;
            if (slot >= 4)
            {
                main = PickStat("Main stat", Disk_Drive.MainStatOptions(slot));
                double suggested = Disk_Drive.SuggestedMainValue(main);
                Console.Write($"Main stat value (fractions for percents, suggested {suggested}): ");
                if (!double.TryParse(Console.ReadLine(), out mainValue) || mainValue <= 0)
                {
                    mainValue = suggested;
                    Console.WriteLine($"Using {mainValue}.");
                }
            }

            var subs = new Dictionary<advancedStats, int>();
            Console.WriteLine("Pick 4 different substats. The roll counts must total 8 or 9.");
            for (int n = 1; n <= 4; n++)
            {
                List<advancedStats> remaining = Disk_Drive.SubstatOptions.Where(stat => !subs.ContainsKey(stat)).ToList();
                advancedStats stat = PickStat($"Substat {n}", remaining);
                subs[stat] = ReadBounded($"Rolls for {stat}: ", 1, 6);
            }

            try
            {
                Disk_Drive disk = player.CreateDiskDrive(sets[setIndex - 1], subs, slot, main, mainValue);
                Console.WriteLine($"Created {Disk_Drive.SetName(disk.DriveSet)} slot {disk.Drive_Num} (ID {disk.DiskID}).");
            }
            catch (ArgumentException ex)
            {
                Console.WriteLine(ex.Message);
            }
        }

        private static void EquipDiskMenu(Player player)
        {
            while (true)
            {
                Console.WriteLine();
                Console.WriteLine("--- Equip disk drive ---");
                Console.WriteLine("1. Equip a disk");
                Console.WriteLine("2. Unequip a disk");
                Console.WriteLine("0. Back");
                Console.Write("Select an option: ");
                switch (Console.ReadLine())
                {
                    case "1":
                        EquipOwnedDisk(player);
                        break;
                    case "2":
                        UnequipOwnedDisk(player);
                        break;
                    case "0":
                        return;
                    default:
                        Console.WriteLine("Unknown option.");
                        break;
                }
            }
        }

        private static void EquipOwnedDisk(Player player)
        {
            List<Disk_Drive> disks = player.UnequippedDiskDrives.OrderBy(disk => disk.Drive_Num).ThenBy(disk => disk.DiskID).ToList();
            if (disks.Count == 0)
            {
                Console.WriteLine("No unequipped disk drives.");
                return;
            }

            Console.WriteLine();
            for (int i = 0; i < disks.Count; i++)
            {
                Disk_Drive disk = disks[i];
                Console.WriteLine($"{i + 1}. ID {disk.DiskID}  Slot {disk.Drive_Num}  {Disk_Drive.SetName(disk.DriveSet)}  {disk.MainStat} ({disk.MainStatValue})");
            }
            Console.WriteLine("0. Back");
            int diskIndex = ReadChoice("Select a disk: ", disks.Count);
            if (diskIndex == 0)
                return;

            List<Character> roster = player.Characters.OrderBy(character => character.getName()).ToList();
            for (int i = 0; i < roster.Count; i++)
            {
                Disk_Drive? occupied = roster[i].GetDiskInSlot(disks[diskIndex - 1].Drive_Num);
                string slotNote = occupied == null ? "empty" : $"replaces {Disk_Drive.SetName(occupied.DriveSet)}";
                Console.WriteLine($"{i + 1}. {roster[i].getName()}  (slot {disks[diskIndex - 1].Drive_Num} {slotNote})");
            }
            Console.WriteLine("0. Back");
            int characterIndex = ReadChoice("Select a character: ", roster.Count);
            if (characterIndex == 0)
                return;

            Disk_Drive chosen = disks[diskIndex - 1];
            Character target = roster[characterIndex - 1];
            player.EquipDisk(target, chosen);
            Console.WriteLine($"Equipped {Disk_Drive.SetName(chosen.DriveSet)} in slot {chosen.Drive_Num} on {target.getName()}.");
        }

        private static void UnequipOwnedDisk(Player player)
        {
            List<Character> roster = player.Characters
                .Where(character => character.GetEquippedDisks().Count > 0)
                .OrderBy(character => character.getName())
                .ToList();
            if (roster.Count == 0)
            {
                Console.WriteLine("No character has a disk equipped.");
                return;
            }

            for (int i = 0; i < roster.Count; i++)
                Console.WriteLine($"{i + 1}. {roster[i].getName()}");
            Console.WriteLine("0. Back");
            int characterIndex = ReadChoice("Select a character: ", roster.Count);
            if (characterIndex == 0)
                return;

            List<Disk_Drive> equipped = roster[characterIndex - 1].GetEquippedDisks().ToList();
            for (int i = 0; i < equipped.Count; i++)
                Console.WriteLine($"{i + 1}. Slot {equipped[i].Drive_Num}  {Disk_Drive.SetName(equipped[i].DriveSet)}");
            Console.WriteLine("0. Back");
            int diskIndex = ReadChoice("Select a slot: ", equipped.Count);
            if (diskIndex == 0)
                return;

            Disk_Drive removed = equipped[diskIndex - 1];
            player.UnequipDisk(roster[characterIndex - 1], removed.Drive_Num);
            Console.WriteLine($"Unequipped slot {removed.Drive_Num}.");
        }

        /// <summary>Returns 1 through <paramref name="count"/>, or 0 to go back.</summary>
        private static int ReadChoice(string prompt, int count)
        {
            while (true)
            {
                Console.Write(prompt);
                string? input = Console.ReadLine();
                if (input == "0")
                    return 0;
                if (int.TryParse(input, out int index) && index >= 1 && index <= count)
                    return index;
                Console.WriteLine("Unknown option.");
            }
        }

        private static int ReadBounded(string prompt, int min, int max)
        {
            while (true)
            {
                Console.Write(prompt);
                if (int.TryParse(Console.ReadLine(), out int value) && value >= min && value <= max)
                    return value;
                Console.WriteLine($"Enter a number from {min} to {max}.");
            }
        }

        private static advancedStats PickStat(string label, IReadOnlyList<advancedStats> options)
        {
            Console.WriteLine(label);
            for (int i = 0; i < options.Count; i++)
                Console.WriteLine($"{i + 1}. {options[i]}");
            return options[ReadBounded("Select a stat: ", 1, options.Count) - 1];
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
