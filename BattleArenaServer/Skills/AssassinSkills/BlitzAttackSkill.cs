using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.AssassinSkills
{
    public class BlitzAttackSkill : Skill
    {
        public BlitzAttackSkill()
        {
            name = "Blitz Attack";
            dmg = 130;
            title = $"Мгновенно переместитесь к врагу и нанесите ему {dmg} физического урона.";
            titleUpg = "+70 к урону, +1 к дальности";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 1;
            range = 3;
            nonTarget = false;
            area = Consts.SpellArea.EnemyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Physical;
        }

        public new ISkillCastRequest request => new EnemyTargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.CasterHex != null && requestData.Target != null && requestData.TargetHex != null)
            {
                List<Hex> emptyHexes = UtilityService.GetHexesRadius(requestData.TargetHex, 1).Where(x => x.IsFree()).ToList();
                if (emptyHexes.Count == 0)
                    return false;

                //Ищем случайный ближайший гекс рядом с целью
                int minDist = emptyHexes.Min(x => x.Distance(requestData.CasterHex));
                List<Hex> nearestHexes = emptyHexes.Where(x => x.Distance(requestData.CasterHex) == minDist).ToList();
                Random rnd = new Random();
                Hex moveHex = nearestHexes[rnd.Next(nearestHexes.Count)];

                //Прыгаем поближе к врагу
                AttackService.MoveHero(requestData.Caster, requestData.CasterHex, moveHex);

                //Атакуем
                AttackService.SetDamage(requestData.Caster, requestData.Target, dmg, dmgType);
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
                dmg += 70;
                range += 1;
                stats.range += 1;
                title = $"Мгновенно переместитесь к врагу и нанесите ему {dmg} физического урона.";
                return true;
            }
            return false;
        }
    }
}
