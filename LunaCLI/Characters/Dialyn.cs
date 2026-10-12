namespace Zenless
{
    class Dialyn : Character
    {
        public Dialyn(int mindNum)
            : base(1481, "Dialyn", 'S', Specialty.Stun, attackType.Slash, atkElement.Physical, mindNum)
        {
            this.baseHP = 8250;
            this.baseATK = 683;
            this.baseDEF = 639;
            this.baseImpact = 110;
            this.critRate = 0.05;
            this.critDMG = 0.5;
            this.AnomalyMastery = 94;
            this.AnomalyProficiency = 93;
            this.penRatio = 0.0;
            this.baseEnergyRegen = 1.2;
            this.damageBonus = 0;
            this.specialDamageBonus = 0;
            this.sheerForce = 0;
            this.ImagePath = Portrait("Dialyn_Portrait.webp");
        }
    }
}