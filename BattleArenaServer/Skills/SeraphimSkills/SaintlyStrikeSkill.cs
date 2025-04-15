using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.SeraphimSkills
{
    public class SaintlyStrikeSkill : Skill
    {
        int addKd = 1;
        public SaintlyStrikeSkill()
        {
            name = "Saintly Strike";
            dmg = 160;
            title = $"Праведный удар карает врага, нанося ему {dmg} маг. урона и откладывая перезарядку его способностей в откате на {addKd}.";
            titleUpg = "Удар затрагивает три клетки перед собой.";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            range = 1;
            area = Consts.SpellArea.EnemyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Magic;
        }

        public override bool Cast(RequestData requestData)
        {
            if (upgraded)
                request = new LineCastRequest();
            else
                request = new EnemyTargetCastRequest();

            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.CasterHex != null && requestData.TargetHex != null)
            {
                List<Hero> attackedHeroes = new List<Hero>();
                if (upgraded)
                    foreach (var n in UtilityService.GetHexesCone(requestData.CasterHex, requestData.TargetHex, radius))
                    {
                        if (n.HERO != null && n.HERO.Team != requestData.Caster.Team)
                            attackedHeroes.Add(n.HERO);
                    }
                else if (requestData.Target != null)
                    attackedHeroes.Add(requestData.Target);

                foreach (var hero in attackedHeroes)
                {
                    foreach (var skill in hero.SkillList)
                    {
                        if (skill.coolDownNow > 0)
                            skill.coolDownNow += addKd;
                    }

                    AttackService.SetDamage(requestData.Caster, hero, dmg, dmgType);
                }

                requestData.Caster.AP -= requireAP;
                coolDownNow = coolDown;
                return true;
            }

            return false;
        }

        public override bool UpgradeSkill()
        {
            if (!upgraded)
            {
                upgraded = true;
                area = Consts.SpellArea.Conus;
                radius = 1;
                title = $"Праведный удар карает врагов, нанося им {dmg} чистого урона и откладывая перезарядку их способностей в откате на {addKd}.";
                return true;
            }
            return false;
        }
    }
}
