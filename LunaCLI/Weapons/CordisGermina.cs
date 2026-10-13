namespace Zenless
{
    class CordisGermina : Weapon
    {
        public CordisGermina()
            : base(21006, "Cordis Germina", 713, advancedStats.Crit_Rate, 0.24)
        {
            ImagePath = Portrait("Cordis_Germina.webp");
            // Refinement 1: CRIT Rate +15%. A Basic Attack or EX Special grants Electric DMG +12.5%, up to 2 stacks.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Crit_Rate, 0.15,
                    damageBonus.nill, 0,
                    applyConditions.Always, 0,
                    target.self),
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.nill, 0,
                    damageBonus.Electric, 0.125,
                    applyConditions.BasicATK, 40,
                    target.self),
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.nill, 0,
                    damageBonus.Electric, 0.125,
                    applyConditions.EX_SP_ATK, 40,
                    target.self)
            });
        }
    }
}
