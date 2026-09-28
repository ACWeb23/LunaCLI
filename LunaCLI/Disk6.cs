using System.Collections.Generic;

namespace Zenless
{
    class Disk6 : Disk_Drive
    {
        public Disk6(int ID, int set, driveSet driveSet, advancedStats primaryStat, double primStatVal, Dictionary<advancedStats, int> substats)
            : base(ID, set)
        {
            DriveSet = driveSet;
            Drive_Num = 6;
            MainStat = primaryStat;
            MainStatValue = primStatVal;
            Load_Substats(substats);
        }
    }
}
