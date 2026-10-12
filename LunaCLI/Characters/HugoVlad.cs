namespace Zenless
{
    class HugoVlad : Character
    {
        public HugoVlad(int mindNum)
            : base(1291, "Hugo Vlad", 'S', Specialty.Attack, attackType.Slash, atkElement.Ice, mindNum)
        {
            this.baseHP = 7940;
            this.baseATK = 844;
            this.baseDEF = 616;
            this.baseImpact = 95;
            this.critRate = 0.05;
            this.critDMG = 0.5;
            this.AnomalyMastery = 86;
            this.AnomalyProficiency = 90;
            this.penRatio = 0.0;
            this.baseEnergyRegen = 1.2;
            this.damageBonus = 0;
            this.specialDamageBonus = 0;
            this.sheerForce = 0;
            this.ImagePath = Portrait("Hugo_Vlad_Portrait.webp");
        }
    }
}