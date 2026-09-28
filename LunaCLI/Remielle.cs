namespace Zenless
{
    // ------------------------------------------------------------------
    // CHARACTERS
    // ------------------------------------------------------------------
    class Remielle : Character
    {
        public Remielle(int mindNum)
            : base(1581, "Remielle Dan", 'S', Specialty.Anomaly, attackType.Slash, atkElement.Lumiflux, mindNum)
        {
            this.baseHP = 7482;
            this.baseATK = 823;
            this.baseDEF = 600;
            this.baseImpact = 83;
            this.critRate = 0.05;
            this.critDMG = 0.5;
            this.AnomalyMastery = 115;
            this.AnomalyProficiency = 170;
            this.penRatio = 0.0;
            this.baseEnergyRegen = 1.2;
            this.damageBonus = 0;
            this.specialDamageBonus = 0;
            this.sheerForce = 0;
            // W-Engine is now equipped from Main via EquipWEngine().
        }
    }
}
