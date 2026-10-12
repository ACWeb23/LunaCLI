namespace Zenless
{
    class AliceThymefield : Character
    {
        public AliceThymefield(int mindNum)
            : base(1401, "Alice Thymefield", 'S', Specialty.Anomaly, attackType.Slash, atkElement.Physical, mindNum)
        {
            this.baseHP = 7673;
            this.baseATK = 880;
            this.baseDEF = 606;
            this.baseImpact = 86;
            this.critRate = 0.05;
            this.critDMG = 0.5;
            this.AnomalyMastery = 106;
            this.AnomalyProficiency = 118;
            this.penRatio = 0.0;
            this.baseEnergyRegen = 1.2;
            this.damageBonus = 0;
            this.specialDamageBonus = 0;
            this.sheerForce = 0;
        }
    }
}