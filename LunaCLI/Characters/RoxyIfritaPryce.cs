namespace Zenless
{
    class RoxyIfritaPryce : Character
    {
        public RoxyIfritaPryce(int mindNum)
            : base(1621, "Roxy Ifrita Pryce", 'S', Specialty.Stun, attackType.Strike, atkElement.Wind, mindNum)
        {
            this.baseHP = 8418;
            this.baseATK = 741;
            this.baseDEF = 641;
            this.baseImpact = 100;
            this.critRate = 0.05;
            this.critDMG = 0.5;
            this.AnomalyMastery = 110;
            this.AnomalyProficiency = 63;
            this.penRatio = 0.0;
            this.baseEnergyRegen = 1.2;
            this.damageBonus = 0;
            this.specialDamageBonus = 0;
            this.sheerForce = 0;
            this.ImagePath = Portrait("Roxy_Ifrita_Pryce_Portrait.webp");
        }
    }
}