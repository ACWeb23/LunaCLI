namespace Zenless
{
    class FrostfallSickle : Weapon
    {
        public FrostfallSickle()
            : base(21014, "Frostfall Sickle", 713, advancedStats.Anomaly_Mastery, 0.30)
        {
            ImagePath = Portrait("Frostfall_Sickle.webp");
            // Refinement 1: An Ice Special or EX Special grants Ice DMG +20% for 40s, up to 2 stacks.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.nill, 0,
                    damageBonus.Ice, 0.20,
                    applyConditions.EX_SP_ATK, 40,
                    target.self)
            });
        }
    }
}
