namespace BattleArenaServer.Models.Summons
{
    public class SkeletonSummon : Summon
    {
        public SkeletonSummon(int Id, string Team, int HP, int armor, int resist, int attackRadius, int dmg, int casterId, int lifeTime) 
            : base(Id, Team, casterId, lifeTime)
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
