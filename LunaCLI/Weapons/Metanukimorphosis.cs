namespace Zenless
{
    class Metanukimorphosis : Weapon
    {
        public Metanukimorphosis()
            : base(21024, "Metanukimorphosis", 713, advancedStats.EnergyRegen, 0.60)
        {
            ImagePath = Portrait("Metanukimorphosis.webp");
            // Refinement 1: An Aftershock grants squad Anomaly Proficiency +60 for 40s.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Anomaly_Proficiency, 60,
                    damageBonus.nill, 0,
                    applyConditions.Aftershock, 40,
                    target.squad)
            });
        }
    }
}
