namespace Zenless
{
    class JoyauDore : Weapon
    {
        public JoyauDore()
            : base(21021, "Joyau Dore", 713, advancedStats.EnergyRegen, 0.60)
        {
            ImagePath = Portrait("Joyau_Dore.webp");
            // Refinement 1: Anomaly Proficiency +70. At 2 Wind EX stacks, squad Anomaly Proficiency +60.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Anomaly_Proficiency, 70,
                    damageBonus.nill, 0,
                    applyConditions.Always, 0,
                    target.self),
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Anomaly_Proficiency, 60,
                    damageBonus.nill, 0,
                    applyConditions.EX_SP_ATK, 40,
                    target.squad)
            });
        }
    }
}
