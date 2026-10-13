namespace Zenless
{
    class CrimsonThirst : Weapon
    {
        public CrimsonThirst()
            : base(21008, "Crimson Thirst", 431, advancedStats.DEF_Percent, 0.48, usesDefenseBase: true)
        {
            ImagePath = Portrait("Crimson_Thirst.webp");
            // Refinement 1: CRIT Rate +25% and Electric DMG +15%.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Crit_Rate, 0.25,
                    damageBonus.nill, 0,
                    applyConditions.Always, 0,
                    target.self),
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.nill, 0,
                    damageBonus.Electric, 0.15,
                    applyConditions.Always, 0,
                    target.self)
            });
        }
    }
}
