namespace Zenless
{
    class LuciaElowen : Character
    {
        public LuciaElowen(int mindNum)
            : base(1451, "Lucia Elowen", 'S', Specialty.Support, attackType.Strike, atkElement.Ether, mindNum)
        {
            this.baseHP = 8477;
            this.baseATK = 683;
            this.baseDEF = 594;
            this.baseImpact = 83;
            this.critRate = 0.05;
            this.critDMG = 0.5;
            this.AnomalyMastery = 96;
            this.AnomalyProficiency = 95;
            this.penRatio = 0.0;
            this.baseEnergyRegen = 1.2;
            this.damageBonus = 0;
            this.specialDamageBonus = 0;
            this.sheerForce = 0;
            this.ImagePath = Portrait("Lucia_Elowen_Portrait.webp");
        }
    }
}