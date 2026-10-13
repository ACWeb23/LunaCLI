namespace Zenless
{
    class RiotSuppressorMarkVI : Weapon
    {
        public RiotSuppressorMarkVI()
            : base(21029, "Riot Suppressor Mark VI", 713, advancedStats.Crit_Damage, 0.48)
        {
            ImagePath = Portrait("Riot_Suppressor_Mark_VI.webp");
            // Refinement 1: CRIT Rate +15%.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Crit_Rate, 0.15,
                    damageBonus.nill, 0,
                    applyConditions.Always, 0,
                    target.self)
            });
        }
    }
}
