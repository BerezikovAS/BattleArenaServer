using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.MusketeerSKills
{
    public class RiposteSkill : Skill
    {
        int armor = 3;
        int percentDmg = 60;
        public RiposteSkill()
        {
            name = "Riposte";
            title = $"Вы становитесь в защитную контратакующую стойку, получая {armor} брони и отвечая на атаки в ближнем бою, нанося {percentDmg}% своего урона атакующему.";
            titleUpg = $"+1 к броне, +15% к ответному урону";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 1;
            nonTarget = true;
            area = Consts.SpellArea.NonTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new NontargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (request.startRequest(requestData, this))
            {
                if (requestData.Caster != null)
                {
                    RiposteBuff riposteBuff = new RiposteBuff(requestData.Caster.Id, armor, 2, percentDmg);
                    requestData.Caster.AddEffect(riposteBuff);

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
                armor += 1;
                percentDmg += 15;
                title = $"Вы становитесь в защитную контратакующую стойку, получая {armor} брони и отвечая на атаки в ближнем бою, нанося {percentDmg}% своего урона атакующему.";
                return true;
            }
            return false;
        }
    }
}
