﻿using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.NecromancerSkills
{
    public class ChainOfPainSkill : Skill
    {
        int percentTransfer = 50;
        public ChainOfPainSkill()
        {
            name = "Chain Of Pain";
            dmg = 70;
            title = $"Соединяет себя и врага некротической цепью, нанося тому {dmg} магического урона. {percentTransfer}% полученного Вами урона наносится и врагу.";
            titleUpg = "+30% к получению урона врагом.";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 1;
            range = 2;
            nonTarget = false;
            area = Consts.SpellArea.EnemyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Magic;
        }

        public new ISkillCastRequest request => new EnemyTargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.Target != null && requestData.TargetHex != null)
            {
                ChainOfPainDebuff chainOfPainDebuff = new ChainOfPainDebuff(requestData.Caster.Id, percentTransfer, 2);
                requestData.Target.AddEffect(chainOfPainDebuff);
                
                AttackService.SetDamage(requestData.Caster, requestData.Target, dmg, dmgType);

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
                percentTransfer += 30;
                title = $"Соединяет себя и врага некротической цепью, нанося тому {dmg} магического урона. {percentTransfer}% полученного Вами урона наносится и врагу.";
                return true;
            }
            return false;
        }
    }
}
