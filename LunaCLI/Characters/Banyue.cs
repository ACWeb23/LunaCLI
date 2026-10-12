namespace Zenless
{
    class Banyue : Character
    {
        public Banyue(int mindNum)
            : base(1471, "Banyue", 'S', Specialty.Rupture, attackType.Strike, atkElement.Fire, mindNum)
        {
            this.baseHP = 8497;
            this.baseATK = 784;
            this.baseDEF = 445;
            this.baseImpact = 95;
            this.critRate = 0.05;
            this.critDMG = 0.5;
            this.AnomalyMastery = 90;
            this.AnomalyProficiency = 89;
            this.penRatio = 0.0;
            this.baseEnergyRegen = 0.0;
            this.damageBonus = 0;
            this.specialDamageBonus = 0;
            this.sheerForce = 0;
            this.ImagePath = Portrait("Banyue_Portrait.webp");
        }
    }
}