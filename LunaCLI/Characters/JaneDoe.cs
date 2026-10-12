namespace Zenless
{
    class JaneDoe : Character
    {
        public JaneDoe(int mindNum)
            : base(1261, "Jane Doe", 'S', Specialty.Anomaly, attackType.Slash, atkElement.Physical, mindNum)
        {
            this.baseHP = 7788;
            this.baseATK = 805;
            this.baseDEF = 606;
            this.baseImpact = 86;
            this.critRate = 0.05;
            this.critDMG = 0.5;
            this.AnomalyMastery = 112;
            this.AnomalyProficiency = 114;
            this.penRatio = 0.0;
            this.baseEnergyRegen = 1.2;
            this.damageBonus = 0;
            this.specialDamageBonus = 0;
            this.sheerForce = 0;
            this.ImagePath = Portrait("Jane_Doe_Portrait.webp");
        }
    }
}