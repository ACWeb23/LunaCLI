namespace Zenless
{
    class CaesarKing : Character
    {
        public CaesarKing(int mindNum)
            : base(1071, "Caesar King", 'S', Specialty.Defense, attackType.Slash, atkElement.Physical, mindNum)
        {
            this.baseHP = 9526;
            this.baseATK = 636;
            this.baseDEF = 753;
            this.baseImpact = 105;
            this.critRate = 0.05;
            this.critDMG = 0.5;
            this.AnomalyMastery = 87;
            this.AnomalyProficiency = 90;
            this.penRatio = 0.0;
            this.baseEnergyRegen = 1.2;
            this.damageBonus = 0;
            this.specialDamageBonus = 0;
            this.sheerForce = 0;
            this.ImagePath = Portrait("Caesar_King_Portrait.webp");
        }
    }
}