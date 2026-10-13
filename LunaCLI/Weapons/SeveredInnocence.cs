namespace Zenless
{
    class SeveredInnocence : Weapon
    {
        public SeveredInnocence()
            : base(21032, "Severed Innocence", 713, advancedStats.Crit_Damage, 0.48)
        {
            ImagePath = Portrait("Severed_Innocence.webp");
            // Refinement 1: CRIT DMG +30%. A Basic Attack, Special, or Aftershock grants CRIT DMG +10%, up to 3 stacks.
            setPassive(new[]
            {
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Crit_Damage, 0.30,
                    damageBonus.nill, 0,
                    applyConditions.Always, 0,
                    target.self),
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Crit_Damage, 0.10,
                    damageBonus.nill, 0,
                    applyConditions.BasicATK, 30,
                    target.self),
                new Buff(
                    specialResource.nill, 0,
                    advancedStats.Crit_Damage, 0.10,
                    damageBonus.nill, 0,
                    applyConditions.Aftershock, 30,
                    target.self)
            });
        }
    }
}
