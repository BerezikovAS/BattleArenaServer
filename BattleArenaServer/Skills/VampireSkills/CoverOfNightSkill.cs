using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.VampireSkills
{
    public class CoverOfNightSkill : Skill
    {
        int extraResist = 3;
        public CoverOfNightSkill()
        {
            name = "Cover Of Night";
            title = $"Укрывает себя и созников в ночи. Герои получают +{extraResist} сопротивления, а атакующие их враги дополнительно теряют 1 ОД.";
            titleUpg = "+2 к сопротивлению";
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

        public override bool Cast(RequestData requestData, GameData gameData)
        {
            if (request.startRequest(requestData, this, gameData))
            {
                if (requestData.Caster != null && requestData.TargetHex != null)
                {
                    foreach (var n in UtilityService.GetHexesRadius(requestData.TargetHex, radius, gameData))
                    {
                        if (n.HERO != null && n.HERO.Team == requestData.Caster.Team)
                        {
                            CoverOfNightBuff coverOfNightBuff = new CoverOfNightBuff(requestData.Caster.Id, extraResist, 2);
                            n.HERO.AddEffect(coverOfNightBuff, gameData);
                        }
                    }
                    requestData.Caster.SpendAP(requireAP, gameData);
                    coolDownNow = coolDown;
                    return true;
                }
            }
            return false;
        }

        public override bool UpgradeSkill(GameData gameData)
        {
            if (!upgraded)
            {
                upgraded = true;
                extraResist += 2;
                title = $"Укрывает себя и созников в ночи. Герои получают +{extraResist} сопротивления, а атакующие их враги дополнительно теряют 1 ОД.";
                return true;
            }
            return false;
        }
    }
}
