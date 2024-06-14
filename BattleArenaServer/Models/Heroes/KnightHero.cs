using BattleArenaServer.Skills.Knight;
using BattleArenaServer.Skills.KnightSkills;
using BattleArenaServer.Skills.KnightSkills.Auras;

namespace BattleArenaServer.Models.Heroes
{
    public class KnightHero : Hero
    {
        public KnightHero() {
            Id = 0;
            Name = "Knight";
            Team = "blue";

            MaxHP = HP = 1000;
            Armor = 5;
            Resist = 3;

            UpgradePoints = 1;

            AP = 4;

            AttackRadius = 1;
            Dmg = 100;

            SkillList[0] = new SelfHealSkill();
            SkillList[1] = new ShieldBashSkill();
            SkillList[2] = new BodyGuardSkill();
            SkillList[3] = new FormationAttackSkill();

            AuraList.Add(new HighShieldAura());
        }
    }
}
