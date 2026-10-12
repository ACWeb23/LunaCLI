namespace Zenless
{
    class Lighter : Character
    {
        public Lighter(int mindNum)
            : base(1161, "Lighter", 'S', Specialty.Stun, attackType.Strike, atkElement.Fire, mindNum)
        {
            this.baseHP = 8253;
            this.baseATK = 722;
            this.baseDEF = 612;
            this.baseImpact = 119;
            this.critRate = 0.05;
            this.critDMG = 0.5;
            this.AnomalyMastery = 91;
            this.AnomalyProficiency = 90;
            this.penRatio = 0.0;
            this.baseEnergyRegen = 1.2;
            this.damageBonus = 0;
            this.specialDamageBonus = 0;
            this.sheerForce = 0;
            this.ImagePath = Portrait("Lighter_Portrait.webp");
        }
    }
}