namespace Zenless
{
    class StarlightBillyKid : Character
    {
        public StarlightBillyKid(int mindNum)
            : base(1531, "Starlight - Billy Kid", 'S', Specialty.Rupture, attackType.Slash, atkElement.Physical, mindNum)
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
            this.ImagePath = Portrait("Starlight_-_Billy_Kid_Portrait.webp");
        }
    }
}