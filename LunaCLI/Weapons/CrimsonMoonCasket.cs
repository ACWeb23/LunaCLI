namespace Zenless
{
    class CrimsonMoonCasket : Weapon
    {
        public CrimsonMoonCasket()
            : base(21007, "Crimson Moon Casket", 713, advancedStats.EnergyRegen, 0.60)
        {
            ImagePath = Portrait("Crimson_Moon_Casket.webp");
            // Refinement 1: CRIT Rate +24%. A Wind EX Special grants squad DMG +20% for 50s.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Crit_Rate, 0.24,
                    damageBonus.nill, 0,
                    applyConditions.Always, 0,
                    target.self),
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.nill, 0,
                    damageBonus.All, 0.20,
                    applyConditions.EX_SP_ATK, 50,
                    target.squad)
            });
        }
    }
}
