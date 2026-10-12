namespace Zenless
{
    class Promeia : Character
    {
        public Promeia(int mindNum)
            : base(1541, "Promeia", 'S', Specialty.Anomaly, attackType.Slash, atkElement.Ice, mindNum)
        {
            this.baseHP = 7788;
            this.baseATK = 797;
            this.baseDEF = 612;
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
            this.ImagePath = Portrait("Promeia_Portrait.webp");
        }
    }
}