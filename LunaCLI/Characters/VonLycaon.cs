namespace Zenless
{
    class VonLycaon : Character
    {
        public VonLycaon(int mindNum)
            : base(1141, "Von Lycaon", 'S', Specialty.Stun, attackType.Strike, atkElement.Ice, mindNum)
        {
            this.baseHP = 8416;
            this.baseATK = 653;
            this.baseDEF = 606;
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
            this.ImagePath = Portrait("Von_Lycaon_Portrait.webp");
        }
    }
}