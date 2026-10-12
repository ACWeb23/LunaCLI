namespace Zenless
{
    class NekomiyaMana : Character
    {
        public NekomiyaMana(int mindNum)
            : base(1021, "Nekomiya Mana", 'S', Specialty.Attack, attackType.Slash, atkElement.Physical, mindNum)
        {
            this.baseHP = 7560;
            this.baseATK = 835;
            this.baseDEF = 587;
            this.baseImpact = 92;
            this.critRate = 0.05;
            this.critDMG = 0.5;
            this.AnomalyMastery = 97;
            this.AnomalyProficiency = 96;
            this.penRatio = 0.0;
            this.baseEnergyRegen = 1.2;
            this.damageBonus = 0;
            this.specialDamageBonus = 0;
            this.sheerForce = 0;
            this.ImagePath = Portrait("Nekomiya_Mana_Portrait.webp");
        }
    }
}