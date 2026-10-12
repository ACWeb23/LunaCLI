namespace Zenless
{
    class BurniceWhite : Character
    {
        public BurniceWhite(int mindNum)
            : base(1171, "Burnice White", 'S', Specialty.Anomaly, attackType.Pierce, atkElement.Fire, mindNum)
        {
            this.baseHP = 7368;
            this.baseATK = 788;
            this.baseDEF = 600;
            this.baseImpact = 83;
            this.critRate = 0.05;
            this.critDMG = 0.5;
            this.AnomalyMastery = 118;
            this.AnomalyProficiency = 120;
            this.penRatio = 0.0;
            this.baseEnergyRegen = 1.2;
            this.damageBonus = 0;
            this.specialDamageBonus = 0;
            this.sheerForce = 0;
            this.ImagePath = Portrait("Burnice_White_Portrait.webp");
        }
    }
}