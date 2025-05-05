using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.DruidSkills
{
    public class PrimalFurySkill : Skill
    {
        public PrimalFurySkill()
        {
            name = "Primal Fury";
            dmg = 80;
            title = $"Наделяет себя или союзника первобытной свирепостью. Следующая атака цели нанесёт дополнительно {dmg} урона.";
            titleUpg = "+40 к урону, -1 к перезарядке";
            coolDown = 3;
            coolDownNow = 0;
            requireAP = 1;
            range = 3;
            nonTarget = false;
            area = Consts.SpellArea.AllyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new AllyTargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (requestData.Target != null && requestData.Caster != null)
            {
                if (!request.startRequest(requestData, this))
                    return false;

                PrimalFuryBuff primalFuryBuff = new PrimalFuryBuff(requestData.Caster.Id, dmg, 3);
                requestData.Target.AddEffect(primalFuryBuff);

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
                coolDown -= 1;
                stats.coolDown -= 1;
                title = $"Наделяет себя или союзника первобытной свирепостью. Следующая атака цели нанесёт дополнительно {dmg} урона.";
                return true;
            }
            return false;
        }
    }
}
