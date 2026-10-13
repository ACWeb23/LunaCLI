namespace Zenless
{
    class YesterdayCalls : Weapon
    {
        public YesterdayCalls()
            : base(21045, "Yesterday Calls", 713, advancedStats.Crit_Rate, 0.24)
        {
            ImagePath = Portrait("Yesterday_Calls.webp");
            // Refinement 1: At 3 stacks of the Physical EX buff, squad CRIT DMG +30% for 40s.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Crit_Damage, 0.30,
                    damageBonus.nill, 0,
                    applyConditions.max_Stacks, 40,
                    target.squad)
            });
        }
    }
}
