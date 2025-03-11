namespace BattleArenaServer.Models.Summons
{
    public class SkeletonSummon : Hero
    {
        public SkeletonSummon(int Id, string Team, int HP, int armor, int resist, int attackRadius, int dmg) : base(Id, Team)
        {
            Name = "Skeleton";

            MaxHP = this.HP = HP;
            Armor = armor;
            Resist = resist;

            AP = 4;

            AttackRadius = attackRadius;
            Dmg = dmg;
        }
    }
}
