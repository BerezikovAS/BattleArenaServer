using BattleArenaServer.Effects;
using BattleArenaServer.Services;
using BattleArenaServer.Skills;

namespace BattleArenaServer.Models
{
    public abstract class Hero
    {
        public int Id { get; set; }
        public string Name { get; set; } = "hero";
        public string Team { get; set; } = "red";
        public int HexId { get; set; }

        public int HP { get; set; }
        public int MaxHP { get; set; }
        public int Armor {  get; set; }
        public int Resist { get; set; }

        public int APtoAttack { get; set; } = 2;
        
        public int AP {  get; set; }

        public int AttackRadius { get; set; }
        public int Dmg { get; set; }

        public int UpgradePoints { get; set; } = 0;

        public delegate bool ApplyDamage(Hero attacker, Hero defender, int dmg);
        public ApplyDamage applyDamage = AttackService.ApplyDamage;

        public delegate int PassiveArmor(Hero attacker, Hero defender);
        public PassiveArmor passiveArmor = delegate { return 0; };

        public delegate int PassiveResistance(Hero attacker, Hero defender);
        public PassiveResistance passiveResistance = delegate { return 0; };

        public delegate int PassiveAttackDamage(Hero attacker, Hero? defender);
        public PassiveAttackDamage passiveAttackDamage = delegate { return 0; };

        public delegate bool AfterAttack(Hero attacker, Hero? defender, int dmg);
        public AfterAttack afterAttack = delegate { return false; };

        public delegate int ModifierAppliedDamage(Hero attacker, Hero defender, int dmg);
        public ModifierAppliedDamage modifierAppliedDamage = delegate { return 0; };

        public void Heal(int _heal)
        {
            HP += _heal;
            if(HP > MaxHP)
                HP = MaxHP;
        }

        public List<Skill> SkillList { get; set; } = new List<Skill> { new EmptySkill(), new EmptySkill(), new EmptySkill(), new EmptySkill() };
        public List<Effect> EffectList { get; set; } = new List<Effect>();
        public List<Aura> AuraList { get; set; } = new List<Aura>();
    }
}
