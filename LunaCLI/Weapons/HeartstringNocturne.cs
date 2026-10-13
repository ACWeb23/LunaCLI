namespace Zenless
{
    class HeartstringNocturne : Weapon
    {
        public HeartstringNocturne()
            : base(21018, "Heartstring Nocturne", 713, advancedStats.Crit_Rate, 0.24)
        {
            ImagePath = Portrait("Heartstring_Nocturne.webp");
            // Refinement 1: CRIT DMG +50%.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Crit_Damage, 0.50,
                    damageBonus.nill, 0,
                    applyConditions.Always, 0,
                    target.self)
            });
        }
    }
}
