using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text.Json;
using System.Text.Json.Serialization;

namespace Zenless
{
    /// <summary>
    /// Saves each user under %LocalAppData%\LunaCLI\users and remembers who was open last.
    /// A user file stores the roster, W-Engines, disk drives, and what is equipped.
    /// Portraits that ship with the app are left out; a cleared or replacement image is stored.
    /// </summary>
    static class UserStore
    {
        private const int FileVersion = 1;
        private const int MaxNameLength = 48;

        private static readonly string Root = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "LunaCLI");

        private static readonly string UsersDirectory = Path.Combine(Root, "users");
        private static readonly string SessionPath = Path.Combine(Root, "session.json");

        private static readonly JsonSerializerOptions Json = new JsonSerializerOptions
        {
            WriteIndented = true,
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            PropertyNameCaseInsensitive = true,
            DefaultIgnoreCondition = JsonIgnoreCondition.WhenWritingNull
        };

        /// <summary>
        /// Loads the user who was open last. The first launch saves <paramref name="seed"/> and opens that user.
        /// </summary>
        public static Player LoadActiveOrSeed(Func<Player> seed)
        {
            if (seed == null)
                throw new ArgumentNullException(nameof(seed));

            List<string> names = ListNames();
            if (names.Count == 0)
            {
                Player created = seed();
                Save(created);
                SetActive(created.Name);
                return created;
            }

            string? active = ReadActive();
            if (active != null && names.Any(name => string.Equals(name, active, StringComparison.OrdinalIgnoreCase)))
                return Load(active);

            SetActive(names[0]);
            return Load(names[0]);
        }

        /// <summary>Names of every saved user, in case-insensitive order.</summary>
        public static List<string> ListNames()
        {
            if (!Directory.Exists(UsersDirectory))
                return new List<string>();

            var names = new List<string>();
            foreach (string path in Directory.EnumerateFiles(UsersDirectory, "*.json"))
            {
                if (TryReadName(path, out string name))
                    names.Add(name);
            }

            names.Sort(StringComparer.OrdinalIgnoreCase);
            return names;
        }

        /// <summary>
        /// Creates an empty user and writes the save file. The name is trimmed and must be unique.
        /// </summary>
        public static Player Create(string name)
        {
            string trimmed = RequireName(name);
            string? existing = ListNames().FirstOrDefault(saved => string.Equals(saved, trimmed, StringComparison.OrdinalIgnoreCase));
            if (existing != null)
                throw new InvalidOperationException($"A user named {existing} already exists.");

            Player player = new Player(trimmed);
            Save(player);
            return player;
        }

        /// <summary>Reads the save file for <paramref name="name"/>.</summary>
        public static Player Load(string name)
        {
            string trimmed = name?.Trim() ?? "";
            string? path = FindPath(trimmed);
            if (path == null)
                throw new InvalidOperationException($"No saved user named {trimmed}.");

            try
            {
                return Read(path);
            }
            catch (Exception ex) when (ex is JsonException or ArgumentException or InvalidOperationException or FormatException)
            {
                throw new InvalidDataException($"Saved data for {trimmed} could not be read. {ex.Message}", ex);
            }
        }

        /// <summary>Writes <paramref name="player"/> to that user's save file.</summary>
        public static void Save(Player player)
        {
            if (player == null)
                throw new ArgumentNullException(nameof(player));

            Directory.CreateDirectory(UsersDirectory);
            string path = FindPath(player.Name) ?? PathForNew(player.Name);
            WriteAtomic(path, JsonSerializer.Serialize(Capture(player), Json));
        }

        /// <summary>Remembers which user should open on the next launch.</summary>
        public static void SetActive(string name)
        {
            string trimmed = name?.Trim() ?? "";
            if (trimmed.Length == 0)
                throw new ArgumentException("User name is required.", nameof(name));

            Directory.CreateDirectory(Root);
            WriteAtomic(SessionPath, JsonSerializer.Serialize(new SessionFile { ActiveUser = trimmed }, Json));
        }

        private static Player Read(string path)
        {
            UserFile? file = JsonSerializer.Deserialize<UserFile>(File.ReadAllText(path), Json);
            if (file == null || file.Version != FileVersion || string.IsNullOrWhiteSpace(file.Name))
                throw new InvalidDataException("The save file is missing a name or uses an unknown version.");

            Player player = new Player(file.Name.Trim());
            var weapons = new List<Weapon>();
            foreach (WeaponFile entry in file.Weapons ?? new List<WeaponFile>())
            {
                Weapon weapon = WeaponRoster.Create(entry.Id);
                ApplyImage(path => weapon.ImagePath = path, entry.Image);
                player.AddWeapon(weapon);
                weapons.Add(weapon);
            }

            foreach (DiskFile entry in file.Disks ?? new List<DiskFile>())
            {
                if (!Enum.TryParse(entry.Set, out driveSet set))
                    throw new InvalidDataException($"Unknown drive set '{entry.Set}'.");

                var subs = new Dictionary<advancedStats, int>();
                foreach (KeyValuePair<string, int> sub in entry.Substats ?? new Dictionary<string, int>())
                {
                    if (!Enum.TryParse(sub.Key, out advancedStats stat))
                        throw new InvalidDataException($"Unknown substat '{sub.Key}'.");
                    subs[stat] = sub.Value;
                }

                advancedStats main = advancedStats.nill;
                if (!string.IsNullOrEmpty(entry.MainStat) && !Enum.TryParse(entry.MainStat, out main))
                    throw new InvalidDataException($"Unknown main stat '{entry.MainStat}'.");

                player.RestoreDiskDrive(entry.Id, set, subs, entry.Slot, main, entry.MainStatValue);
                Disk_Drive disk = player.DiskDrives.First(item => item.DiskID == entry.Id);
                ApplyImage(path => disk.ImagePath = path, entry.Image);
            }

            foreach (CharacterFile entry in file.Characters ?? new List<CharacterFile>())
            {
                if (entry.Mindscapes < 0 || entry.Mindscapes > 6)
                    throw new InvalidDataException($"Mindscapes for character {entry.Id} must be from 0 to 6.");

                Character character = CharacterRoster.Create(entry.Id, entry.Mindscapes);
                ApplyImage(imagePath => character.ImagePath = imagePath, entry.Image);
                player.AddCharacter(character);

                if (entry.Weapon is int weaponIndex)
                {
                    if (weaponIndex < 0 || weaponIndex >= weapons.Count)
                        throw new InvalidDataException($"Character {entry.Id} points at a missing W-Engine.");
                    player.EquipWeapon(character, weapons[weaponIndex]);
                }

                foreach (int diskID in entry.Disks ?? new List<int>())
                {
                    Disk_Drive? disk = player.DiskDrives.FirstOrDefault(item => item.DiskID == diskID);
                    if (disk == null)
                        throw new InvalidDataException($"Character {entry.Id} points at missing disk {diskID}.");
                    player.EquipDisk(character, disk);
                }
            }

            return player;
        }

        private static UserFile Capture(Player player)
        {
            List<Weapon> weapons = player.Weapons.ToList();
            var weaponIndex = new Dictionary<Weapon, int>();
            for (int i = 0; i < weapons.Count; i++)
                weaponIndex[weapons[i]] = i;

            return new UserFile
            {
                Version = FileVersion,
                Name = player.Name,
                Characters = player.Characters.Select(character => new CharacterFile
                {
                    Id = character.characterID,
                    Mindscapes = character.Mindscapes,
                    Weapon = character.GetEquippedWeapon() is Weapon equipped ? weaponIndex[equipped] : null,
                    Disks = character.GetEquippedDisks().Select(disk => disk.DiskID).ToList(),
                    Image = StoredImage(character.ImagePath)
                }).ToList(),
                Weapons = weapons.Select(weapon => new WeaponFile
                {
                    Id = weapon.WeaponID,
                    Image = StoredImage(weapon.ImagePath)
                }).ToList(),
                Disks = player.DiskDrives.OrderBy(disk => disk.DiskID).Select(disk => new DiskFile
                {
                    Id = disk.DiskID,
                    Set = disk.DriveSet.ToString(),
                    Slot = disk.Drive_Num,
                    MainStat = disk.MainStat.ToString(),
                    MainStatValue = disk.MainStatValue,
                    Substats = disk.GetSubstats().ToDictionary(sub => sub.Stat.ToString(), sub => sub.NumRoll),
                    Image = StoredImage(disk.ImagePath)
                }).ToList()
            };
        }

        /// <summary>
        /// Null keeps the portrait that ships with the app. An empty string means the image was cleared.
        /// Anything else is a file the user picked.
        /// </summary>
        private static string? StoredImage(string? path)
        {
            if (string.IsNullOrWhiteSpace(path))
                return "";

            string full;
            try
            {
                full = Path.GetFullPath(path);
            }
            catch (Exception ex) when (ex is ArgumentException or NotSupportedException or PathTooLongException)
            {
                return path;
            }

            string root = Path.GetFullPath(AppContext.BaseDirectory);
            if (!root.EndsWith(Path.DirectorySeparatorChar) && !root.EndsWith(Path.AltDirectorySeparatorChar))
                root += Path.DirectorySeparatorChar;
            if (full.StartsWith(root, StringComparison.OrdinalIgnoreCase))
                return null;
            return full;
        }

        private static void ApplyImage(Action<string?> assign, string? stored)
        {
            if (stored == null)
                return;
            assign(stored.Length == 0 ? null : stored);
        }

        private static string RequireName(string name)
        {
            string trimmed = name?.Trim() ?? "";
            if (trimmed.Length == 0)
                throw new ArgumentException("User name is required.", nameof(name));
            if (trimmed.Length > MaxNameLength)
                throw new ArgumentException($"User name must be {MaxNameLength} characters or fewer.", nameof(name));
            if (trimmed.Any(char.IsControl))
                throw new ArgumentException("User name cannot include line breaks.", nameof(name));
            return trimmed;
        }

        private static string? ReadActive()
        {
            if (!File.Exists(SessionPath))
                return null;

            try
            {
                SessionFile? session = JsonSerializer.Deserialize<SessionFile>(File.ReadAllText(SessionPath), Json);
                string name = session?.ActiveUser?.Trim() ?? "";
                return name.Length == 0 ? null : name;
            }
            catch (Exception ex) when (ex is JsonException or IOException)
            {
                return null;
            }
        }

        private static string? FindPath(string name)
        {
            if (name.Length == 0 || !Directory.Exists(UsersDirectory))
                return null;

            foreach (string path in Directory.EnumerateFiles(UsersDirectory, "*.json"))
            {
                if (TryReadName(path, out string found) && string.Equals(found, name, StringComparison.OrdinalIgnoreCase))
                    return path;
            }

            return null;
        }

        private static string PathForNew(string name)
        {
            string slug = Slug(name);
            string candidate = Path.Combine(UsersDirectory, slug + ".json");
            int suffix = 2;
            while (File.Exists(candidate))
            {
                candidate = Path.Combine(UsersDirectory, slug + "-" + suffix + ".json");
                suffix++;
            }

            return candidate;
        }

        private static string Slug(string name)
        {
            char[] invalid = Path.GetInvalidFileNameChars();
            string slug = new string(name.Trim().ToLowerInvariant().Select(letter =>
                letter == ' ' || invalid.Contains(letter) ? '_' : letter).ToArray()).Trim('_');
            return slug.Length == 0 ? "user" : slug;
        }

        private static bool TryReadName(string path, out string name)
        {
            name = "";
            try
            {
                using JsonDocument document = JsonDocument.Parse(File.ReadAllText(path));
                if (!document.RootElement.TryGetProperty("name", out JsonElement element))
                    return false;
                name = element.GetString()?.Trim() ?? "";
                return name.Length > 0;
            }
            catch (Exception ex) when (ex is JsonException or IOException)
            {
                return false;
            }
        }

        private static void WriteAtomic(string path, string json)
        {
            string temp = path + ".tmp";
            File.WriteAllText(temp, json);
            File.Move(temp, path, overwrite: true);
        }

        private sealed class SessionFile
        {
            public string ActiveUser { get; set; } = "";
        }

        private sealed class UserFile
        {
            public int Version { get; set; }
            public string Name { get; set; } = "";
            public List<CharacterFile>? Characters { get; set; }
            public List<WeaponFile>? Weapons { get; set; }
            public List<DiskFile>? Disks { get; set; }
        }

        private sealed class CharacterFile
        {
            public int Id { get; set; }
            public int Mindscapes { get; set; }
            public int? Weapon { get; set; }
            public List<int>? Disks { get; set; }
            public string? Image { get; set; }
        }

        private sealed class WeaponFile
        {
            public int Id { get; set; }
            public string? Image { get; set; }
        }

        private sealed class DiskFile
        {
            public int Id { get; set; }
            public string Set { get; set; } = "";
            public int Slot { get; set; }
            public string? MainStat { get; set; }
            public double MainStatValue { get; set; }
            public Dictionary<string, int>? Substats { get; set; }
            public string? Image { get; set; }
        }
    }
}
