namespace Zenless
{
    class Aria : Character
    {
        public Aria(int mindNum)
            : base(1501, "Aria", 'S', Specialty.Anomaly, attackType.Strike, atkElement.Ether, mindNum)
        {
            this.baseHP = 7749;
            this.baseATK = 788;
            this.baseDEF = 619;
            this.baseImpact = 87;
            this.critRate = 0.05;
            this.critDMG = 0.5;
            this.AnomalyMastery = 115;
            this.AnomalyProficiency = 116;
            this.penRatio = 0.0;
            this.baseEnergyRegen = 1.2;
            this.damageBonus = 0;
            this.specialDamageBonus = 0;
            this.sheerForce = 0;
            this.ImagePath = Portrait("Aria_Portrait.webp");
        }
    }
}