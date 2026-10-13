namespace Zenless
{
    class FusionCompiler : Weapon
    {
        public FusionCompiler()
            : base(21015, "Fusion Compiler", 684, advancedStats.PEN_Ratio, 0.24)
        {
            ImagePath = Portrait("Fusion_Compiler.webp");
            // Refinement 1: ATK +12%. A Special or EX Special grants Anomaly Proficiency +25 for 8s, up to 3 stacks.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.ATK_Percent, 0.12,
                    damageBonus.nill, 0,
                    applyConditions.Always, 0,
                    target.self),
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Anomaly_Proficiency, 25,
                    damageBonus.nill, 0,
                    applyConditions.EX_SP_ATK, 8,
                    target.self)
            });
        }
    }
}
