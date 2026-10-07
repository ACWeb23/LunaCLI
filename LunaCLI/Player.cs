using System;
using System.Collections.Generic;
using System.Linq;

namespace Zenless
{
    /// <summary>
    /// Owns a unique roster of characters plus every weapon and disk drive the player has.
    /// Equipped items stay on the character; unequipped items are held here until they are equipped again.
    /// </summary>
    class Player
    {
        public string Name { get; }

        private readonly Dictionary<int, Character> characters = new Dictionary<int, Character>();
        private readonly HashSet<Weapon> weapons = new HashSet<Weapon>();
        private readonly Dictionary<int, Disk_Drive> diskDrives = new Dictionary<int, Disk_Drive>();
        private readonly HashSet<Weapon> unequippedWeapons = new HashSet<Weapon>();
        private readonly Dictionary<int, Disk_Drive> unequippedDiskDrives = new Dictionary<int, Disk_Drive>();

        /// <summary>
        /// Characters currently in the roster. Uniqueness is enforced by <see cref="Character.characterID"/>.
        /// </summary>
        public IReadOnlyCollection<Character> Characters => characters.Values;

        /// <summary>
        /// Every weapon the player owns, whether it is equipped or sitting in inventory.
        /// </summary>
        public IReadOnlyCollection<Weapon> Weapons => weapons;

        /// <summary>
        /// Every disk drive the player owns, whether it is equipped or sitting in inventory.
        /// </summary>
        public IReadOnlyCollection<Disk_Drive> DiskDrives => diskDrives.Values;

        /// <summary>
        /// Weapons that are owned but not currently equipped on any character.
        /// </summary>
        public IReadOnlyCollection<Weapon> UnequippedWeapons => unequippedWeapons;

        /// <summary>
        /// Disk drives that are owned but not currently equipped on any character.
        /// </summary>
        public IReadOnlyCollection<Disk_Drive> UnequippedDiskDrives => unequippedDiskDrives.Values;

        /// <summary>
        /// Creates a player with the given display name and empty inventories.
        /// </summary>
        /// <param name="name">The player's name. Must not be null or blank.</param>
        /// <exception cref="ArgumentException">Thrown when <paramref name="name"/> is null or whitespace.</exception>
        public Player(string name)
        {
            if (string.IsNullOrWhiteSpace(name))
                throw new ArgumentException("Player name is required.", nameof(name));
            Name = name;
        }

        /// <summary>
        /// Adds a character to the roster. The same <see cref="Character.characterID"/> cannot be added twice.
        /// If the character already has a W-Engine or disk drives equipped, those items are registered as owned
        /// and treated as equipped (they are not placed in the unequipped inventories).
        /// </summary>
        /// <param name="character">The character to add. Must not be null.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="character"/> is null.</exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when a character with the same ID is already in the roster,
        /// or when any equipment already on the character is owned by this player.
        /// </exception>
        public void AddCharacter(Character character)
        {
            if (character == null)
                throw new ArgumentNullException(nameof(character));
            if (characters.ContainsKey(character.characterID))
                throw new InvalidOperationException($"Character {character.getName()} is already in the roster.");

            foreach (Disk_Drive disk in character.GetEquippedDisks())
            {
                if (diskDrives.ContainsKey(disk.DiskID))
                    throw new InvalidOperationException($"Disk {disk.DiskID} is already owned.");
            }

            Weapon? equippedWeapon = character.GetEquippedWeapon();
            if (equippedWeapon != null && weapons.Contains(equippedWeapon))
                throw new InvalidOperationException("That weapon is already in the inventory.");

            characters.Add(character.characterID, character);

            if (equippedWeapon != null)
                weapons.Add(equippedWeapon);

            foreach (Disk_Drive disk in character.GetEquippedDisks())
                diskDrives.Add(disk.DiskID, disk);
        }

        /// <summary>
        /// Adds a weapon to the player's collection and places it in the unequipped inventory.
        /// Uniqueness is by object instance, so two separate copies of the same W-Engine type are allowed.
        /// </summary>
        /// <param name="weapon">The weapon to add. Must not be null.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="weapon"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when this exact weapon instance is already owned.</exception>
        public void AddWeapon(Weapon weapon)
        {
            if (weapon == null)
                throw new ArgumentNullException(nameof(weapon));
            if (!weapons.Add(weapon))
                throw new InvalidOperationException("That weapon is already in the inventory.");

            unequippedWeapons.Add(weapon);
        }

        /// <summary>
        /// Builds a new disk drive and adds it to the unequipped inventory.
        /// Choose the drive set first, then supply exactly four distinct substats.
        /// The four <paramref name="driveSubStats"/> roll counts must add up to 8 or 9.
        /// Slots 1-3 use a fixed main stat. Slots 4-6 also need <paramref name="mainStat"/> and <paramref name="mainStatValue"/>.
        /// </summary>
        /// <param name="driveSet">The disk set this piece belongs to.</param>
        /// <param name="driveSubStats">Exactly four advanced stats mapped to their roll counts. Each count must be at least 1.</param>
        /// <param name="slot">Drive slot 1 through 6. Determines which disk class and default main stat are used.</param>
        /// <param name="mainStat">Required main stat for slots 4-6. Ignored for slots 1-3.</param>
        /// <param name="mainStatValue">Required main-stat value for slots 4-6. Ignored for slots 1-3.</param>
        /// <returns>The created disk drive, now owned and unequipped.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="driveSubStats"/> is null.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown when <paramref name="slot"/> is not between 1 and 6.</exception>
        /// <exception cref="ArgumentException">
        /// Thrown when there are not exactly four substats, a substat is invalid,
        /// a roll count is below 1, or the four roll counts do not total 8 or 9.
        /// Also thrown when slots 4-6 are missing a main stat or value.
        /// </exception>
        public Disk_Drive CreateDiskDrive(
            driveSet driveSet,
            Dictionary<advancedStats, int> driveSubStats,
            int slot,
            advancedStats mainStat = advancedStats.nill,
            double mainStatValue = 0)
        {
            if (driveSubStats == null)
                throw new ArgumentNullException(nameof(driveSubStats));
            if (slot < 1 || slot > 6)
                throw new ArgumentOutOfRangeException(nameof(slot), "Disk slot must be between 1 and 6.");

            ValidateSubstats(driveSubStats);

            int diskID = AllocateDiskId();
            int setID = (int)driveSet;
            Disk_Drive disk;

            switch (slot)
            {
                case 1:
                    disk = new Disk1(diskID, setID, driveSet, driveSubStats);
                    break;
                case 2:
                    disk = new Disk2(diskID, setID, driveSet, driveSubStats);
                    break;
                case 3:
                    disk = new Disk3(diskID, setID, driveSet, driveSubStats);
                    break;
                case 4:
                    RequireFlexibleMainStat(slot, mainStat, mainStatValue);
                    disk = new Disk4(diskID, setID, driveSet, mainStat, mainStatValue, driveSubStats);
                    break;
                case 5:
                    RequireFlexibleMainStat(slot, mainStat, mainStatValue);
                    disk = new Disk5(diskID, setID, driveSet, mainStat, mainStatValue, driveSubStats);
                    break;
                default:
                    RequireFlexibleMainStat(slot, mainStat, mainStatValue);
                    disk = new Disk6(diskID, setID, driveSet, mainStat, mainStatValue, driveSubStats);
                    break;
            }

            AddDiskDrive(disk);
            return disk;
        }

        /// <summary>
        /// Adds a disk drive to the player's collection and places it in the unequipped inventory.
        /// Uniqueness is enforced by <see cref="Disk_Drive.DiskID"/>.
        /// </summary>
        /// <param name="disk">The disk drive to add. Must not be null.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="disk"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when a disk with the same ID is already owned.</exception>
        public void AddDiskDrive(Disk_Drive disk)
        {
            if (disk == null)
                throw new ArgumentNullException(nameof(disk));
            if (diskDrives.ContainsKey(disk.DiskID))
                throw new InvalidOperationException($"Disk {disk.DiskID} is already in the inventory.");

            diskDrives.Add(disk.DiskID, disk);
            unequippedDiskDrives.Add(disk.DiskID, disk);
        }

        /// <summary>
        /// Equips a owned, unequipped weapon onto a roster character.
        /// If that character already has a W-Engine, the previous weapon is moved back into the unequipped inventory.
        /// </summary>
        /// <param name="character">The roster character who will receive the weapon.</param>
        /// <param name="weapon">The weapon to equip. Must already be in this player's inventory.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="character"/> or <paramref name="weapon"/> is null.</exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the character is not in the roster, the weapon is not owned,
        /// or the weapon is already equipped on a different character.
        /// </exception>
        public void EquipWeapon(Character character, Weapon weapon)
        {
            EnsureOwnedCharacter(character);
            if (weapon == null)
                throw new ArgumentNullException(nameof(weapon));
            if (!weapons.Contains(weapon))
                throw new InvalidOperationException("Weapon is not in the player's inventory.");
            if (!unequippedWeapons.Contains(weapon) && character.GetEquippedWeapon() != weapon)
                throw new InvalidOperationException("Weapon is already equipped on another character.");

            Weapon? previous = character.GetEquippedWeapon();
            if (previous != null && previous != weapon)
                unequippedWeapons.Add(previous);

            unequippedWeapons.Remove(weapon);
            character.EquipWEngine(weapon);
        }

        /// <summary>
        /// Removes the W-Engine from a roster character and returns it to the unequipped inventory.
        /// </summary>
        /// <param name="character">The roster character whose weapon should be unequipped.</param>
        /// <returns>The weapon that was unequipped, or null if the character had none.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="character"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the character is not in this player's roster.</exception>
        public Weapon? UnequipWeapon(Character character)
        {
            EnsureOwnedCharacter(character);
            Weapon? weapon = character.UnequipWEngine();
            if (weapon != null)
                unequippedWeapons.Add(weapon);
            return weapon;
        }

        /// <summary>
        /// Equips a owned, unequipped disk drive onto a roster character.
        /// Disks occupy slots 1-6. If that slot is already filled, the previous disk is moved back
        /// into the unequipped inventory.
        /// </summary>
        /// <param name="character">The roster character who will receive the disk.</param>
        /// <param name="disk">The disk drive to equip. Must already be in this player's inventory.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="character"/> or <paramref name="disk"/> is null.</exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when the character is not in the roster, the disk is not owned,
        /// or the disk is already equipped on a different character.
        /// </exception>
        public void EquipDisk(Character character, Disk_Drive disk)
        {
            EnsureOwnedCharacter(character);
            if (disk == null)
                throw new ArgumentNullException(nameof(disk));
            if (!diskDrives.ContainsKey(disk.DiskID) || diskDrives[disk.DiskID] != disk)
                throw new InvalidOperationException("Disk drive is not in the player's inventory.");

            bool alreadyOnThisCharacter = character.GetDiskInSlot(disk.Drive_Num) == disk;
            if (!unequippedDiskDrives.ContainsKey(disk.DiskID) && !alreadyOnThisCharacter)
                throw new InvalidOperationException("Disk drive is already equipped on another character.");

            Disk_Drive? previous = character.GetDiskInSlot(disk.Drive_Num);
            if (previous != null && previous != disk)
                unequippedDiskDrives[previous.DiskID] = previous;

            unequippedDiskDrives.Remove(disk.DiskID);
            character.EquipDisk(disk);
        }

        /// <summary>
        /// Removes the disk drive in the given slot from a roster character and returns it to the unequipped inventory.
        /// </summary>
        /// <param name="character">The roster character whose disk should be unequipped.</param>
        /// <param name="slot">The disk slot to clear (1-6).</param>
        /// <returns>The disk that was unequipped, or null if that slot was empty.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="character"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the character is not in this player's roster.</exception>
        public Disk_Drive? UnequipDisk(Character character, int slot)
        {
            EnsureOwnedCharacter(character);
            Disk_Drive? disk = character.UnequipDisk(slot);
            if (disk != null)
                unequippedDiskDrives[disk.DiskID] = disk;
            return disk;
        }

        /// <summary>
        /// Returns the W-Engine currently equipped on the given roster character.
        /// </summary>
        /// <param name="character">The roster character to inspect.</param>
        /// <returns>The equipped weapon, or null if none is equipped.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="character"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the character is not in this player's roster.</exception>
        public Weapon? GetEquippedWeapon(Character character)
        {
            EnsureOwnedCharacter(character);
            return character.GetEquippedWeapon();
        }

        /// <summary>
        /// Returns the disk drives currently equipped on the given roster character, ordered by slot.
        /// </summary>
        /// <param name="character">The roster character to inspect.</param>
        /// <returns>A read-only list of equipped disks. Empty if none are equipped.</returns>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="character"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the character is not in this player's roster.</exception>
        public IReadOnlyList<Disk_Drive> GetEquippedDisks(Character character)
        {
            EnsureOwnedCharacter(character);
            return character.GetEquippedDisks();
        }

        /// <summary>
        /// Looks up a roster character by ID.
        /// </summary>
        /// <param name="characterID">The unique character ID to search for.</param>
        /// <returns>The matching character, or null if that ID is not in the roster.</returns>
        public Character? GetCharacter(int characterID)
        {
            characters.TryGetValue(characterID, out Character? character);
            return character;
        }

        /// <summary>
        /// Requires exactly four distinct advanced stats whose roll counts sum to 8 or 9.
        /// </summary>
        private static void ValidateSubstats(Dictionary<advancedStats, int> driveSubStats)
        {
            if (driveSubStats.Count != 4)
                throw new ArgumentException("A disk drive must have exactly 4 advanced stats.");
            if (driveSubStats.ContainsKey(advancedStats.nill))
                throw new ArgumentException("Substats cannot be nill.");
            if (driveSubStats.Keys.Any(stat => Disk_Drive.SubstatRollValue(stat) <= 0))
                throw new ArgumentException("One or more substats are not valid disk substats.");
            if (driveSubStats.Values.Any(rolls => rolls < 1))
                throw new ArgumentException("Each of the 4 advanced stats must have at least 1 roll.");

            int totalRolls = driveSubStats.Values.Sum();
            if (totalRolls < 8 || totalRolls > 9)
                throw new ArgumentException("DriveSubStats must total between 8 and 9 rolls across all 4 advanced stats.");
        }

        /// <summary>
        /// Slots 4-6 do not have a fixed main stat, so both the stat and its value must be provided.
        /// </summary>
        private static void RequireFlexibleMainStat(int slot, advancedStats mainStat, double mainStatValue)
        {
            if (mainStat == advancedStats.nill)
                throw new ArgumentException($"Slot {slot} requires a main stat.");
            if (mainStatValue <= 0)
                throw new ArgumentException($"Slot {slot} requires a main-stat value greater than 0.");
        }

        /// <summary>
        /// Returns the next unused disk ID so newly created drives stay unique in this player's inventory.
        /// </summary>
        private int AllocateDiskId()
        {
            int diskID = 1;
            while (diskDrives.ContainsKey(diskID))
                diskID++;
            return diskID;
        }

        /// <summary>
        /// Confirms that the given character instance is in this player's roster.
        /// Matching ID is not enough; the object must be the same instance that was added.
        /// </summary>
        /// <param name="character">The character to validate.</param>
        /// <exception cref="ArgumentNullException">Thrown when <paramref name="character"/> is null.</exception>
        /// <exception cref="InvalidOperationException">Thrown when the character is not in this player's roster.</exception>
        private void EnsureOwnedCharacter(Character character)
        {
            if (character == null)
                throw new ArgumentNullException(nameof(character));
            if (!characters.TryGetValue(character.characterID, out Character? owned) || owned != character)
                throw new InvalidOperationException("Character is not in this player's roster.");
        }
    }
}
