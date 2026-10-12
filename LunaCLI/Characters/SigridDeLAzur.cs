namespace Zenless
{
    class SigridDeLAzur : Character
    {
        public SigridDeLAzur(int mindNum)
            : base(1591, "Sigrid de L'Azur", 'S', Specialty.Attack, attackType.Pierce, atkElement.Ice, mindNum)
        {
            this.baseHP = 7788;
            this.baseATK = 863;
            this.baseDEF = 606;
            this.baseImpact = 93;
            this.critRate = 0.05;
            this.critDMG = 0.5;
            this.AnomalyMastery = 92;
            this.AnomalyProficiency = 90;
            this.penRatio = 0.0;
            this.baseEnergyRegen = 1.2;
            this.damageBonus = 0;
            this.specialDamageBonus = 0;
            this.sheerForce = 0;
            this.ImagePath = Portrait("Sigrid_de_L%27Azur_Portrait.webp");
        }
    }
}