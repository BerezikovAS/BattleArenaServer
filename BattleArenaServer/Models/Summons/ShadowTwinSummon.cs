namespace BattleArenaServer.Models.Summons
{
    public class ShadowTwinSummon : Summon
    {
        public ShadowTwinSummon(int Id, string Team, int casterId, int lifeTime, Hero target)
            : base(Id, Team, casterId, lifeTime)
        {
            Name = target.Name;

            MaxHP = HP = target.HP;
            Armor = target.Armor;
            Resist = target.Resist;

            AP = 4;

            AttackRadius = target.AttackRadius;
            Dmg = target.Dmg;

            SkillList[0] = CreateSkillOfSameType(target.SkillList[0], this);
        }

        private Skill CreateSkillOfSameType(Skill skill, Hero hero)
        {
            Type type = skill.GetType();

            if (type.IsSubclassOf(typeof(Skill)) || type == typeof(Skill))
            {
                return (Skill)Activator.CreateInstance(type, hero);
            }

            throw new InvalidOperationException("Тип не является дочерним классом Skill");
        }
    }
}
