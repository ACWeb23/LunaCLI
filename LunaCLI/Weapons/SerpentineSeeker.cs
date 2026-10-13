namespace Zenless
{
    class SerpentineSeeker : Weapon
    {
        public SerpentineSeeker()
            : base(21031, "Serpentine Seeker", 713, advancedStats.EnergyRegen, 0.60)
        {
            ImagePath = Portrait("Serpentine_Seeker.webp");
            // Refinement 1: CRIT Rate +25%.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Crit_Rate, 0.25,
                    damageBonus.nill, 0,
                    applyConditions.Always, 0,
                    target.self)
            });
        }
    }
}
