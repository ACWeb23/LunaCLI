namespace Zenless
{
    class ClaretFlint : Character
    {
        public ClaretFlint(int mindNum)
            : base(1611, "Claret Flint", 'S', Specialty.Armorer, attackType.Slash, atkElement.Electric, mindNum)
        {
            this.baseHP = 5654;
            this.baseATK = 626;
            this.baseDEF = 441;
            this.baseImpact = 93;
            this.critRate = 0.05;
            this.critDMG = 0.5;
            this.AnomalyMastery = 80;
            this.AnomalyProficiency = 79;
            this.penRatio = 0.0;
            this.baseEnergyRegen = 0.0;
            this.damageBonus = 0;
            this.specialDamageBonus = 0;
            this.sheerForce = 0;
            this.ImagePath = Portrait("Claret_Flint_Portrait.webp");
        }
    }
}