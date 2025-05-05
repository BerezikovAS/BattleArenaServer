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
            this.AddEffect += BaseAddEffect;
            this.SpendAP += BaseSpendAP;
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

        public bool IsMainHero { get; set; } = true;
        public int VP { get; set; } = 10;
        public int GoldReward { get; set; } = 10;
        public int DamageDealed { get; set; } = 0; //Используется для эффектов завязанных на нанесенном герое уроне

        public int RespawnTime {  get; set; } = 0;

        public Consts.HeroType type { get; set; } = Consts.HeroType.Hero;

        public Skill MoveSkill { get; set; } = new MoveSkill();

        public StatsEffect StatsEffect { get; set; } = new StatsEffect();

        #region Delegates
        public delegate bool ApplyDamage(Hero? attacker, Hero defender, int dmg, Consts.DamageType dmgType);
        public ApplyDamage applyDamage = AttackService.ApplyDamage;

        public delegate int PassiveArmor(Hero? attacker, Hero defender);
        public PassiveArmor passiveArmor = delegate { return 0; };

        public int GetPassiveArmor(Hero? attacker, Hero defender)
        {
            int value = 0;
            var invocationList = passiveArmor.GetInvocationList();
            foreach (var invocation in invocationList)
                value += ((PassiveArmor)invocation)(attacker, defender);
            return value;
        }

        public delegate int PassiveResistance(Hero attacker, Hero defender);
        public PassiveResistance passiveResistance = delegate { return 0; };

        public int GetPassiveResistance(Hero attacker, Hero defender)
        {
            int value = 0;
            var invocationList = passiveResistance.GetInvocationList();
            foreach (var invocation in invocationList)
                value += ((PassiveResistance)invocation)(attacker, defender);
            return value;
        }

        public delegate int PassiveAttackDamage(Hero attacker, Hero? defender);
        public PassiveAttackDamage passiveAttackDamage = delegate { return 0; };

        public int GetPassiveAttackDamage(Hero attacker, Hero? defender)
        {
            int value = 0;
            var invocationList = passiveAttackDamage.GetInvocationList();
            foreach (var invocation in invocationList)
                value += ((PassiveAttackDamage)invocation)(attacker, defender);
            return value;
        }

        public delegate int ArmorPiercing(Hero attacker, Hero defender);
        public ArmorPiercing armorPiercing = delegate { return 0; };

        public int GetArmorPiercing(Hero attacker, Hero defender)
        {
            int value = 0;
            var invocationList = armorPiercing.GetInvocationList();
            foreach (var invocation in invocationList)
                value += ((ArmorPiercing)invocation)(attacker, defender);
            return value;
        }

        public delegate int ResistPiercing(Hero attacker, Hero defender);
        public ResistPiercing resistPiercing = delegate { return 0; };

        public int GetResistPiercing(Hero attacker, Hero defender)
        {
            int value = 0;
            var invocationList = resistPiercing.GetInvocationList();
            foreach (var invocation in invocationList)
                value += ((ResistPiercing)invocation)(attacker, defender);
            return value;
        }

        public delegate bool BeforeAttack(Hero attacker, Hero defender, int dmg);
        public BeforeAttack beforeAttack = delegate { return false; };

        public delegate bool BeforeReceivedAttack(Hero attacker, Hero defender, int dmg);
        public BeforeReceivedAttack beforeReceivedAttack = delegate { return false; };

        public delegate bool AfterAttack(Hero attacker, Hero defender, int dmg, Consts.DamageType dmgType);
        public AfterAttack afterAttack = delegate { return false; };

        public delegate bool AfterReceivedAttack(Hero attacker, Hero defender, int dmg);
        public AfterReceivedAttack afterReceivedAttack = delegate { return false; };

        public delegate int ModifierAppliedDamage(Hero? attacker, Hero defender, int dmg, Consts.DamageType dmgType);
        public ModifierAppliedDamage modifierAppliedDamage = delegate { return 0; };

        public int GetModifierAppliedDamage(Hero? attacker, Hero defender, int dmg, Consts.DamageType dmgType)
        {
            int value = 0;
            var invocationList = modifierAppliedDamage.GetInvocationList();
            foreach (var invocation in invocationList)
                value += ((ModifierAppliedDamage)invocation)(attacker, defender, dmg, dmgType);
            return value;
        }

        public delegate void BeforeSpellCast(Hero attacker, Hero? defender, Skill skill);
        public BeforeSpellCast beforeSpellCast = delegate { };

        public delegate void AfterSpellCast(Hero attacker, Hero? defender, Skill skill);
        public BeforeSpellCast afterSpellCast = delegate { };

        public delegate void AfterMove(Hero hero, Hex? currentHex, Hex targetHex);
        public AfterMove afterMove = delegate { };

        public delegate void AfterReceiveDmg(Hero hero, Hero? attacker, int dmg, Consts.DamageType dmgType);
        public AfterReceiveDmg afterReceiveDmg = delegate { };

        public delegate void HealDelegate(int heal);
        public HealDelegate Heal = delegate { };

        public delegate void AddEffectDelegate(Effect effect);
        public AddEffectDelegate AddEffect = delegate { };

        public delegate void SpendAPDelegate(int spendAP);
        public SpendAPDelegate SpendAP = delegate { };
        #endregion

        public virtual void Respawn()
        {
            foreach (var effect in EffectList)
                effect.RemoveEffect(this);
            EffectList.Clear();

            foreach (var item in Items)
                item.ApplyEffect(this);
        }

        public void BaseHeal(int heal)
        {
            HP += heal;
            if (HP > MaxHP)
                HP = MaxHP;
        }

        public void BaseAddEffect(Effect effect)
        {
            EffectList.Add(effect);
            if (effect.effectType == Consts.EffectType.Instant)
                effect.ApplyEffect(this);
        }

        public void ImmunAddEffect(Effect effect)
        {
            if (effect.type == Consts.StatusEffect.Debuff)
                return;

            EffectList.Add(effect);
            if (effect.effectType == Consts.EffectType.Instant)
                effect.ApplyEffect(this);
        }

        public void BaseSpendAP(int apCount)
        {
            AP -= apCount;
            if (AP < 0)
                AP = 0;
        }

        public void SpiritLinkSpendAP(int apCount)
        {
            AP -= apCount;
            if (AP < 0)
            {
                Effect? spiritLink = EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.SpiritLink));
                if (spiritLink != null)
                {
                    Hero? anotherHero = GameData._heroes.FirstOrDefault(x => x.Id == spiritLink.idCaster);
                    if (anotherHero != null)
                    {
                        anotherHero.SpendAP(AP * -1);
                    }
                }
                AP = 0;
            }
        }

        public List<Skill> SkillList { get; set; } = new List<Skill> { new EmptySkill(), new EmptySkill(), new EmptySkill(), new EmptySkill(), new EmptySkill() };
        public List<Effect> EffectList { get; set; } = new List<Effect>();
        public List<Aura> AuraList { get; set; } = new List<Aura>();
        public List<Item> Items { get; set; } = new List<Item> { };
    }
}
