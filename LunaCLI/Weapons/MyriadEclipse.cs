namespace Zenless
{
    class MyriadEclipse : Weapon
    {
        public MyriadEclipse()
            : base(21025, "Myriad Eclipse", 713, advancedStats.Crit_Rate, 0.24)
        {
            ImagePath = Portrait("Myriad_Eclipse.webp");
            // Refinement 1: CRIT DMG +45%.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Crit_Damage, 0.45,
                    damageBonus.nill, 0,
                    applyConditions.Always, 0,
                    target.self)
            });
        }
    }
}
