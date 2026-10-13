namespace Zenless
{
    class Timeweaver : Weapon
    {
        public Timeweaver()
            : base(21041, "Timeweaver", 713, advancedStats.ATK_Percent, 0.30)
        {
            ImagePath = Portrait("Timeweaver.webp");
            // Refinement 1: Electric Anomaly Buildup Rate +30%. A Special or EX Special against an Anomaly target grants Anomaly Proficiency +75.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Anomaly_Mastery, 0.30,
                    damageBonus.nill, 0,
                    applyConditions.Always, 0,
                    target.self),
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Anomaly_Proficiency, 75,
                    damageBonus.nill, 0,
                    applyConditions.EX_SP_ATK, 15,
                    target.self)
            });
        }
    }
}
