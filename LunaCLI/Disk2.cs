using System.Collections.Generic;

namespace Zenless
{
    class Disk2 : Disk_Drive
    {
        public Disk2(int ID, int set, driveSet driveSet, Dictionary<advancedStats, int> substats)
            : base(ID, set)
        {
            DriveSet = driveSet;
            Drive_Num = 2;
            MainStat = advancedStats.ATK;
            MainStatValue = FixedMainValue(MainStat);
            Load_Substats(substats);
            UseSetIcon();
        }
    }
}
