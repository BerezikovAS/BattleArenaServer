using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.ElementalistSkills
{
    public class VentusSkill : Skill
    {
        public VentusSkill()
        {
            name = "Ventus";
            title = $"Попутный ветер подгоняет Вас и союзников неподалёку. Даёт одно бесплатное движение в свой ход.";
            titleUpg = "Не требует очков действий. -1 к перезарядке.";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 1;
            nonTarget = false;
            radius = 2;
            range = 0;
            area = Consts.SpellArea.Radius;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new NonTargerAoECastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (request.startRequest(requestData, this))
            {
                if (requestData.Caster != null && requestData.TargetHex != null)
                {
                    foreach (var n in UtilityService.GetHexesRadius(requestData.TargetHex, radius))
                    {
                        if (n.HERO != null && n.HERO.Team == requestData.Caster.Team)
                        {
                            HasteBuff hasteBuff = new HasteBuff(requestData.Caster.Id, 0, 2);
                            n.HERO.AddEffect(hasteBuff);
                        }
                    }
                    requestData.Caster.SpendAP(requireAP);
                    coolDownNow = coolDown;
                    return true;
                }
            }
            return false;
        }

        public override bool UpgradeSkill()
        {
            if (!upgraded)
            {
                upgraded = true;
                coolDown -= 1;
                stats.coolDown -= 1;
                requireAP -= 1;
                stats.requireAP -= 1;
                return true;
            }
            return false;
        }
    }
}
