namespace Zenless
{
    class FlamemakerShaker : Weapon
    {
        public FlamemakerShaker()
            : base(21012, "Flamemaker Shaker", 713, advancedStats.ATK_Percent, 0.30)
        {
            ImagePath = Portrait("Flamemaker_Shaker.webp");
            // Refinement 1: An EX Special or Assist grants DMG +3.5% per stack. At 5 stacks, Anomaly Proficiency +50.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.nill, 0,
                    damageBonus.All, 0.035,
                    applyConditions.EX_SP_ATK, 6,
                    target.self),
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Anomaly_Proficiency, 50,
                    damageBonus.nill, 0,
                    applyConditions.off_Field, 6,
                    target.self)
            });
        }
    }
}
