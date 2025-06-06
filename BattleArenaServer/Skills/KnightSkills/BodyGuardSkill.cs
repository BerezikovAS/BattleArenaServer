﻿using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.Knight
{
    public class BodyGuardSkill : Skill
    {
        int defence = 3;
        int duration = 3;
        public BodyGuardSkill()
        {
            name = "Body guard";
            title = $"Дает +{defence} брони и +{defence} сопротивления. Переносит 50% чистого урона с союзника на себя";
            titleUpg = "Действует 2 хода";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 1;
            range = 1;
            nonTarget = false;
            area = Consts.SpellArea.FriendTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new FriendTargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.Target != null)
            {
                BodyGuardBuff bodyGuardBuff = new BodyGuardBuff(requestData.Caster.Id, defence, duration);
                requestData.Target.AddEffect(bodyGuardBuff);

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
                duration += 1;
                title += "\n" + titleUpg;
                return true;
            }
            return false;
        }
    }
}
