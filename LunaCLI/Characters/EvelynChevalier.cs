namespace Zenless
{
    class EvelynChevalier : Character
    {
        public EvelynChevalier(int mindNum)
            : base(1321, "Evelyn Chevalier", 'S', Specialty.Attack, attackType.Slash, atkElement.Fire, mindNum)
        {
            this.baseHP = 7788;
            this.baseATK = 854;
            this.baseDEF = 612;
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
            this.ImagePath = Portrait("Evelyn_Chevalier_Portrait.webp");
        }
    }
}