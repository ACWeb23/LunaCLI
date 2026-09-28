using System.Collections.Generic;

namespace Zenless
{
    class Disk5 : Disk_Drive
    {
        // Was mistakenly named Disk6 in the original, which would not compile.
        public Disk5(int ID, int set, driveSet driveSet, advancedStats primaryStat, double primStatVal, Dictionary<advancedStats, int> substats)
            : base(ID, set)
        {
            DriveSet = driveSet;
            Drive_Num = 5;
            MainStat = primaryStat;
            MainStatValue = primStatVal;
            Load_Substats(substats);
        }
    }
}
