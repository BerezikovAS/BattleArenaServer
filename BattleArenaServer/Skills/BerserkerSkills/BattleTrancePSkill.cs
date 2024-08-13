using BattleArenaServer.Models;
using BattleArenaServer.Services;

namespace BattleArenaServer.Skills.BerserkerSkills
{
    public class BattleTrancePSkill : PassiveSkill
    {
        int cntEnemiesPenalty = 1;
        string skillDesc = ", кроме первого";
        public BattleTrancePSkill(Hero hero) : base(hero)
        {
            name = "Battle Trance";
            title = $"Чем больше врагов вокруг, тем яростнее атаки. +20% к урону от атак за каждого врага вокруг{skillDesc}.";
            titleUpg = "И первый враг даёт бонус к урону";
            skillType = Consts.SkillType.Passive;
            hero.passiveAttackDamage += BattleTrance;
        }

        public override bool Cast(RequestData requestData)
        {
            return false;
        }

        public override void refreshEffect()
        {
        }

        public override bool UpgradeSkill()
        {
            if (!upgraded)
            {
                upgraded = true;
                hero.passiveAttackDamage -= BattleTrance;
                cntEnemiesPenalty = 0;
                skillDesc = "";
                hero.passiveAttackDamage += BattleTrance;
                title = $"Чем больше врагов вокруг, тем яростнее атаки. +20% к урону от атак за каждого врага вокруг{skillDesc}.";
                return true;
            }
            return false;
        }

        private int BattleTrance(Hero attacker, Hero? defender)
        {
            Hex? attackerHex = GameData._hexes.FirstOrDefault(x => x.HERO?.Id == attacker.Id);

            int enemiesCount = 0;
            double extraDmg = 0;
            if (attackerHex != null)
            {
                foreach (var n in UtilityService.GetHexesRadius(attackerHex, 1))
                {
                    if (n.HERO != null && n.HERO.Team != attacker.Team && n.HERO.type != Consts.HeroType.Obstacle)
                        enemiesCount++;
                }
                extraDmg = (attacker.Dmg + attacker.StatsEffect.Dmg) * ((enemiesCount - cntEnemiesPenalty) * 0.2);
                return (int)(Math.Round(extraDmg));
            }
            return 0;
        }
    }
}
