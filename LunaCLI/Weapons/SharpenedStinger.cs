namespace Zenless
{
    class SharpenedStinger : Weapon
    {
        public SharpenedStinger()
            : base(21033, "Sharpened Stinger", 713, advancedStats.Anomaly_Proficiency, 90)
        {
            ImagePath = Portrait("Sharpened_Stinger.webp");
            // Refinement 1: A Dash Attack grants Physical DMG +12% for 10s, up to 3 stacks. At max stacks, Anomaly Buildup Rate +40%.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.nill, 0,
                    damageBonus.Physical, 0.12,
                    applyConditions.dashATK, 10,
                    target.self),
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Anomaly_Mastery, 0.40,
                    damageBonus.nill, 0,
                    applyConditions.max_Stacks, 0,
                    target.self)
            });
        }
    }
}
