namespace Zenless
{
    class NeonFantasies : Weapon
    {
        public NeonFantasies()
            : base(21026, "Neon Fantasies", 713, advancedStats.Anomaly_Mastery, 0.30)
        {
            ImagePath = Portrait("Neon_Fantasies.webp");
            // Refinement 1: Anomaly Proficiency +90. An Ether EX Special or Basic Attack grants squad DMG +15%, up to 2 stacks.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Anomaly_Proficiency, 90,
                    damageBonus.nill, 0,
                    applyConditions.Always, 0,
                    target.self),
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.nill, 0,
                    damageBonus.All, 0.15,
                    applyConditions.EX_SP_ATK, 40,
                    target.squad)
            });
        }
    }
}
