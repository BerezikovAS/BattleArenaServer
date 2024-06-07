using BattleArenaServer.Interfaces;
using BattleArenaServer.Skills.Knight;
using BattleArenaServer.Skills.KnightSkills;
using BattleArenaServer.Skills.KnightSkills.Auras;

namespace BattleArenaServer.Models.Heroes
{
    public class Knight : Hero
    {
        public Knight() {
            Id = 0;
            Name = "Knight";
            Team = "blue";

            MaxHP = HP = 1000;
            Armor = 5;
            Resist = 3;

            AP = 4;

            AttackRadius = 1;
            Dmg = 100;

            SkillList[0] = new SelfHeal();
            SkillList[1] = new ShieldBash();
            SkillList[2] = new BodyGuard();
            SkillList[3] = new FormationAttack();

            AuraList.Add(new HighShieldAura());
        }
    }
}
