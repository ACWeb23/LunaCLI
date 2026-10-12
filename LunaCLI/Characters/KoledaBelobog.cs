namespace Zenless
{
    class KoledaBelobog : Character
    {
        public KoledaBelobog(int mindNum)
            : base(1101, "Koleda Belobog", 'S', Specialty.Stun, attackType.Strike, atkElement.Fire, mindNum)
        {
            this.baseHP = 8127;
            this.baseATK = 660;
            this.baseDEF = 594;
            this.baseImpact = 116;
            this.critRate = 0.05;
            this.critDMG = 0.5;
            this.AnomalyMastery = 97;
            this.AnomalyProficiency = 96;
            this.penRatio = 0.0;
            this.baseEnergyRegen = 1.2;
            this.damageBonus = 0;
            this.specialDamageBonus = 0;
            this.sheerForce = 0;
            this.ImagePath = Portrait("Koleda_Belobog_Portrait.webp");
        }
    }
}