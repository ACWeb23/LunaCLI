using System.Collections.Generic;

namespace Zenless
{
    class Disk3 : Disk_Drive
    {
        public Disk3(int ID, int set, driveSet driveSet, Dictionary<advancedStats, int> substats)
            : base(ID, set)
        {
            DriveSet = driveSet;
            Drive_Num = 3;
            MainStat = advancedStats.DEF;
            MainStatValue = FixedMainValue(MainStat);
            Load_Substats(substats);
            UseSetIcon();
        }
    }
}
