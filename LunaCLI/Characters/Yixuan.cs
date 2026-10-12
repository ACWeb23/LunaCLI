namespace Zenless
{
    class Yixuan : Character
    {
        public Yixuan(int mindNum)
            : base(1371, "Yixuan", 'S', Specialty.Rupture, attackType.Strike, atkElement.Ether, mindNum)
        {
            this.baseHP = 7953;
            this.baseATK = 872;
            this.baseDEF = 441;
            this.baseImpact = 93;
            this.critRate = 0.05;
            this.critDMG = 0.5;
            this.AnomalyMastery = 92;
            this.AnomalyProficiency = 90;
            this.penRatio = 0.0;
            this.baseEnergyRegen = 0.0;
            this.damageBonus = 0;
            this.specialDamageBonus = 0;
            this.sheerForce = 0;
            this.ImagePath = Portrait("Yixuan_Portrait.webp");
        }
    }
}