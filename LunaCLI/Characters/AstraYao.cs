namespace Zenless
{
    class AstraYao : Character
    {
        public AstraYao(int mindNum)
            : base(1311, "Astra Yao", 'S', Specialty.Support, attackType.Strike, atkElement.Ether, mindNum)
        {
            this.baseHP = 8609;
            this.baseATK = 640;
            this.baseDEF = 600;
            this.baseImpact = 83;
            this.critRate = 0.05;
            this.critDMG = 0.5;
            this.AnomalyMastery = 93;
            this.AnomalyProficiency = 92;
            this.penRatio = 0.0;
            this.baseEnergyRegen = 1.2;
            this.damageBonus = 0;
            this.specialDamageBonus = 0;
            this.sheerForce = 0;
            this.ImagePath = Portrait("Astra_Yao_Portrait.webp");
        }
    }
}