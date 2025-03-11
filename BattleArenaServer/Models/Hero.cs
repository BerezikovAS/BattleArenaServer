using BattleArenaServer.Effects;
using BattleArenaServer.Services;
using BattleArenaServer.Skills;
using BattleArenaServer.Skills._CommonSkills;

namespace BattleArenaServer.Models
{
    public abstract class Hero
    {
        public Hero(int Id, string Team)
        {
            this.Id = Id;
            this.Team = Team;
            this.Heal += BaseHeal;
        }

        public int Id { get; set; }
        public string Name { get; set; } = "hero";
        public string Team { get; set; } = "red";
        public int HexId { get; set; }

        public int HP { get; set; }
        public int MaxHP { get; set; }
        public int Armor { get; set; } = 0;
        public int Resist { get; set; } = 0;

        public int APtoAttack { get; set; } = 2;

        public int AP { get; set; } = 0;

        public int AttackRadius { get; set; } = 0;
        public int Dmg { get; set; } = 0;

        public int UpgradePoints { get; set; } = 0;

        public Consts.HeroType type { get; set; } = Consts.HeroType.Hero;

        public Skill MoveSkill { get; } = new MoveSkill();
        //public Skill MoveSkill { get; } = new MoveSkill();

        public StatsEffect StatsEffect { get; set; } = new StatsEffect();

        public delegate bool ApplyDamage(Hero? attacker, Hero defender, int dmg);
        public ApplyDamage applyDamage = AttackService.ApplyDamage;

        public delegate int PassiveArmor(Hero? attacker, Hero defender);
        public PassiveArmor passiveArmor = delegate { return 0; };

        public delegate int PassiveResistance(Hero attacker, Hero defender);
        public PassiveResistance passiveResistance = delegate { return 0; };

        public delegate int PassiveAttackDamage(Hero attacker, Hero? defender);
        public PassiveAttackDamage passiveAttackDamage = delegate { return 0; };

        public delegate bool BeforeAttack(Hero attacker, Hero defender, int dmg);
        public BeforeAttack beforeAttack = delegate { return false; };

        public delegate bool AfterAttack(Hero attacker, Hero defender, int dmg);
        public AfterAttack afterAttack = delegate { return false; };

        public delegate int ModifierAppliedDamage(Hero? attacker, Hero defender, int dmg);
        public ModifierAppliedDamage modifierAppliedDamage = delegate { return 0; };

        public delegate void BeforeSpellCast(Hero attacker, Hero? defender, Skill skill);
        public BeforeSpellCast beforeSpellCast = delegate { };

        public delegate void AfterMove(Hero hero, Hex? currentHex, Hex targetHex);
        public AfterMove afterMove = delegate { };

        public delegate void AfterReceiveDmg(Hero hero, Hero? attacker, int dmg);
        public AfterReceiveDmg afterReceiveDmg = delegate { };

        public delegate void HealDelegate(int heal);
        public HealDelegate Heal = delegate { };

        public void BaseHeal(int heal)
        {
            HP += heal;
            if (HP > MaxHP)
                HP = MaxHP;
        }

        public virtual void AddEffect(Effect effect)
        {
            EffectList.Add(effect);
        }

        public List<Skill> SkillList { get; set; } = new List<Skill> { new EmptySkill(), new EmptySkill(), new EmptySkill(), new EmptySkill(), new EmptySkill() };
        public List<Effect> EffectList { get; set; } = new List<Effect>();
        public List<Aura> AuraList { get; set; } = new List<Aura>();
    }
}
