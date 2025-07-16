using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.SnowQueenSkills
{
    public class MirrorShieldSkill : Skill
    {
        int extraResist = 2;
        public MirrorShieldSkill()
        {
            name = "Mirror Shield";
            title = $"Защищает себя или союзника зеркальным щитом, который даёт +{extraResist} сопротивления и отражает накладываемые негативные эффекты обратно во врага.";
            titleUpg = "+2 к сопротивлению";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 1;
            range = 2;
            nonTarget = false;
            area = Consts.SpellArea.AllyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new AllyTargetCastRequest();

        public override bool Cast(RequestData requestData, GameData gameData)
        {
            if (requestData.Target != null && requestData.Caster != null)
            {
                if (!request.startRequest(requestData, this, gameData))
                    return false;

                MirrorShieldBuff mirrorShieldBuff = new MirrorShieldBuff(requestData.Caster.Id, extraResist, 2);
                requestData.Target.AddEffect(mirrorShieldBuff, gameData);

                requestData.Caster.SpendAP(requireAP, gameData);
                coolDownNow = coolDown;
                return true;
            }
            return false;
        }

        public override bool UpgradeSkill(GameData gameData)
        {
            if (!upgraded)
            {
                upgraded = true;
                extraResist += 2;
                title = $"Защищает себя или союзника зеркальным щитом, который даёт +{extraResist} сопротивления и отражает накладываемые негативные эффекты обратно во врага.";
                return true;
            }
            return false;
        }
    }
}
