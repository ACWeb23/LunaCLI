namespace Zenless
{
    class KrakensCradle : Weapon
    {
        public KrakensCradle()
            : base(21023, "Kraken's Cradle", 713, advancedStats.HP_Percent, 0.30)
        {
            ImagePath = Portrait("Kraken%27s_Cradle.webp");
            // Refinement 1: CRIT Rate +20% while HP is at or below 50%.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Crit_Rate, 0.20,
                    damageBonus.nill, 0,
                    applyConditions.off_Field, 0,
                    target.self)
            });
        }
    }
}
