using BattleArenaServer.Effects;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Skills;

namespace BattleArenaServer.Models
{
    public abstract class Hero
    {
        public int Id { get; set; }
        public string Name { get; set; } = "hero";
        public string Team { get; set; } = "red";

        public int HP { get; set; }
        public int MaxHP { get; set; }
        public int Armor {  get; set; }
        public int Resist { get; set; }
        
        public int AP {  get; set; }

        public int AttackRadius { get; set; }
        public int Dmg { get; set; }

        public void Heal(int _heal)
        {
            HP += _heal;
            if(HP > MaxHP)
                HP = MaxHP;
        }

        public List<Skill> SkillList { get; set; } = new List<Skill> { new EmptySkill(), new EmptySkill(), new EmptySkill(), new EmptySkill() };
        public List<Effect> EffectList { get; set; } = new List<Effect>();
    }
}
