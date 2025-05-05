using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Models.Obstacles;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.ChaosSkills
{
    public class BattleConfusionSkill : Skill
    {
        public BattleConfusionSkill()
        {
            name = "Battle Confusion";
            dmg = 25;
            title = $"За каждого героя в области действия, Вы либо восстанавливаете 25 ХП, либо получаете 2 брони, либо 2 сопротивления.";
            titleUpg = "+1 к радиусу, +3 к дальности";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 1;
            nonTarget = false;
            range = 0;
            radius = 2;
            area = Consts.SpellArea.Radius;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Physical;
        }

        public new ISkillCastRequest request => new RangeAoECastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.TargetHex != null)
            {
                int[] statsSkill = new int[3];
                Random rnd = new Random();
                foreach (var n in UtilityService.GetHexesRadius(requestData.TargetHex, radius))
                {
                    if (n.HERO != null && n.HERO is not SolidObstacle)
                    {
                        statsSkill[rnd.Next(0,3)] += 1;
                    }
                }

                if (statsSkill[0] > 0)
                    requestData.Caster.Heal(statsSkill[0] * 25);

                if (statsSkill[1] > 0)
                {
                    ArmorBuff armorBuff = new ArmorBuff(requestData.Caster.Id, statsSkill[1] * 2, 2);
                    requestData.Caster.AddEffect(armorBuff);
                }

                if (statsSkill[2] > 0)
                {
                    ResistBuff resistBuff = new ResistBuff(requestData.Caster.Id, statsSkill[2] * 2, 2);
                    requestData.Caster.AddEffect(resistBuff);
                }

                requestData.Caster.SpendAP(requireAP);
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
                range += 3;
                radius += 1;
                stats.range += 3;
                stats.radius += 1;
                return true;
            }
            return false;
        }
    }
}
