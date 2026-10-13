namespace Zenless
{
    class HalfSugarBunny : Weapon
    {
        public HalfSugarBunny()
            : base(21017, "Half-Sugar Bunny", 713, advancedStats.HP_Percent, 0.30)
        {
            ImagePath = Portrait("Half-Sugar_Bunny.webp");
            // Refinement 1: Squad ATK +10% and Max HP +10%. An Ether Veil grants squad CRIT DMG +30% for 60s.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.ATK_Percent, 0.10,
                    damageBonus.nill, 0,
                    applyConditions.Always, 0,
                    target.squad),
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.HP_Percent, 0.10,
                    damageBonus.nill, 0,
                    applyConditions.Always, 0,
                    target.squad),
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Crit_Damage, 0.30,
                    damageBonus.nill, 0,
                    applyConditions.Ether_Veil, 60,
                    target.squad)
            });
        }
    }
}
