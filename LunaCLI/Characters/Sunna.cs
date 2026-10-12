namespace Zenless
{
    class Sunna : Character
    {
        public Sunna(int mindNum)
            : base(1491, "Sunna", 'S', Specialty.Support, attackType.Strike, atkElement.Physical, mindNum)
        {
            this.baseHP = 8477;
            this.baseATK = 675;
            this.baseDEF = 600;
            this.baseImpact = 98;
            this.critRate = 0.05;
            this.critDMG = 0.5;
            this.AnomalyMastery = 96;
            this.AnomalyProficiency = 95;
            this.penRatio = 0.0;
            this.baseEnergyRegen = 1.0;
            this.damageBonus = 0;
            this.specialDamageBonus = 0;
            this.sheerForce = 0;
            this.ImagePath = Portrait("Sunna_Portrait.webp");
        }
    }
}