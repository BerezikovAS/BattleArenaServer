﻿using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.Knight
{
    public class BodyGuardSkill : Skill
    {
        int defence = 2;
        int duration = 2;
        public BodyGuardSkill()
        {
            name = "Body guard";
            title = $"Дает +{defence} брони и +{defence} сопротивления. Переносит 50% чистого урона с союзника на себя";
            titleUpg = "Действует 2 хода";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            range = 1;
            nonTarget = false;
            area = Consts.SpellArea.AllyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new FriendTargetCastRequest();

        public override bool Cast(Hero caster, Hero? target, Hex? targetHex)
        {
            if (request.startRequest(caster, target, targetHex, this))
            {
                if (caster != null && target != null)
                {
                    caster.AP -= requireAP;

                    BodyGuardBuff bodyGuardBuff = new BodyGuardBuff(caster.Id, defence, duration);
                    target.EffectList.Add(bodyGuardBuff);
                    bodyGuardBuff.ApplyEffect(target);

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
                duration += 1;
                return true;
            }
            return false;
        }
    }
}