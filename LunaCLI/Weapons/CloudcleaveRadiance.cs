namespace Zenless
{
    class CloudcleaveRadiance : Weapon
    {
        public CloudcleaveRadiance()
            : base(21005, "Cloudcleave Radiance", 743, advancedStats.Crit_Damage, 0.48)
        {
            ImagePath = Portrait("Cloudcleave_Radiance.webp");
            // Refinement 1: Activating an Ether Veil grants DMG +25% and CRIT DMG +25% for 40s.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.nill, 0,
                    damageBonus.All, 0.25,
                    applyConditions.Ether_Veil, 40,
                    target.self),
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Crit_Damage, 0.25,
                    damageBonus.nill, 0,
                    applyConditions.Ether_Veil, 40,
                    target.self)
            });
        }
    }
}
