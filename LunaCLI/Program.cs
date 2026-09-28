using System;
using System.Collections.Generic;

namespace Zenless
{
    // ------------------------------------------------------------------
    // PROGRAM
    // ------------------------------------------------------------------
    class Program
    {
        public static void Main(string[] args)
        {
            // 1. Create the character

            Console.WriteLine("Enter your Mindscape Number");
            int Number = int.Parse(Console.ReadLine());
            Remielle remielle = new Remielle(Number);

            // 2. Equip the W-Engine
            remielle.EquipWEngine(new Ode_Of_Resurrected_Wings());

            // 3. Build 6 dummy disk drives (4pc Freedom Blues + 2pc Shadow Harmony)
            var disk1 = new Disk1(1001, 1, driveSet.Feathered_Fate, new Dictionary<advancedStats, int>
            {
                { advancedStats.DEF_Percent, 2 },
                { advancedStats.Crit_Rate, 1 },
                { advancedStats.Anomaly_Proficiency, 2 },
                { advancedStats.ATK_Percent, 4 }
            });

            var disk2 = new Disk2(1002, 1, driveSet.Freedom_Blues, new Dictionary<advancedStats, int>
            {
                { advancedStats.ATK_Percent, 3 },
                { advancedStats.Crit_Damage, 1 },
                { advancedStats.Anomaly_Proficiency, 3 },
                { advancedStats.DEF_Percent, 1 }
            });

            var disk3 = new Disk3(1003, 1, driveSet.Feathered_Fate, new Dictionary<advancedStats, int>
            {
                { advancedStats.ATK, 2 },
                { advancedStats.Anomaly_Proficiency, 2 },
                { advancedStats.ATK_Percent, 2 },
                { advancedStats.Crit_Rate, 2 }
            });

            var disk4 = new Disk4(1004, 1, driveSet.Feathered_Fate, advancedStats.Anomaly_Proficiency, 92.0,
                new Dictionary<advancedStats, int>
                {
                    { advancedStats.DEF, 2 },
                    { advancedStats.ATK, 1 },
                    { advancedStats.ATK_Percent, 2 },
                    { advancedStats.HP, 3 }
                });

            var disk5 = new Disk5(1005, 3, driveSet.Freedom_Blues, advancedStats.ATK_Percent, 0.30,
                new Dictionary<advancedStats, int>
                {
                    { advancedStats.Anomaly_Proficiency, 3 },
                    { advancedStats.HP, 2 },
                    { advancedStats.DEF, 1 },
                    { advancedStats.Crit_Damage, 2 }
                });

            var disk6 = new Disk6(1006, 3, driveSet.Feathered_Fate, advancedStats.ATK_Percent, 0.30,
                new Dictionary<advancedStats, int>
                {
                    { advancedStats.ATK, 3 },
                    { advancedStats.Anomaly_Proficiency, 2 },
                    { advancedStats.Crit_Damage, 2 },
                    { advancedStats.DEF_Percent, 1 }
                });

            remielle.EquipDisk(disk1);
            remielle.EquipDisk(disk2);
            remielle.EquipDisk(disk3);
            remielle.EquipDisk(disk4);
            remielle.EquipDisk(disk5);
            remielle.EquipDisk(disk6);

            // 4. Show the result
            remielle.PrintSummary();
        }
    }
}
