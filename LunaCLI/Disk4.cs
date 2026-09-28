using System.Collections.Generic;

namespace Zenless
{
    class Disk4 : Disk_Drive
    {
        public Disk4(int ID, int set, driveSet driveSet, advancedStats primaryStat, double primStatVal, Dictionary<advancedStats, int> substats)
            : base(ID, set)
        {
            DriveSet = driveSet;
            Drive_Num = 4;
            MainStat = primaryStat;
            MainStatValue = primStatVal;
            Load_Substats(substats);
        }
    }
}
