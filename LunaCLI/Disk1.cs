using System.Collections.Generic;

namespace Zenless
{
    class Disk1 : Disk_Drive
    {
        public Disk1(int ID, int set, driveSet driveSet, Dictionary<advancedStats, int> substats)
            : base(ID, set)
        {
            DriveSet = driveSet;
            Drive_Num = 1;
            MainStat = advancedStats.HP;
            MainStatValue = FixedMainValue(MainStat);
            Load_Substats(substats);
            UseSetIcon();
        }
    }
}
