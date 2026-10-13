namespace Zenless
{
    class SolExuvia : Weapon
    {
        public SolExuvia()
            : base(21034, "Sol Exuvia", 713, advancedStats.ATK_Percent, 0.30)
        {
            ImagePath = Portrait("Sol_Exuvia.webp");
            // Refinement 1: CRIT Rate +20%.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Crit_Rate, 0.20,
                    damageBonus.nill, 0,
                    applyConditions.Always, 0,
                    target.self)
            });
        }
    }
}
