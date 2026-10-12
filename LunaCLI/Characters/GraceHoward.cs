namespace Zenless
{
    class GraceHoward : Character
    {
        public GraceHoward(int mindNum)
            : base(1181, "Grace Howard", 'S', Specialty.Anomaly, attackType.Pierce, atkElement.Electric, mindNum)
        {
            this.baseHP = 7482;
            this.baseATK = 750;
            this.baseDEF = 600;
            this.baseImpact = 83;
            this.critRate = 0.05;
            this.critDMG = 0.5;
            this.AnomalyMastery = 115;
            this.AnomalyProficiency = 116;
            this.penRatio = 0.0;
            this.baseEnergyRegen = 1.2;
            this.damageBonus = 0;
            this.specialDamageBonus = 0;
            this.sheerForce = 0;
            this.ImagePath = Portrait("Grace_Howard_Portrait.webp");
        }
    }
}