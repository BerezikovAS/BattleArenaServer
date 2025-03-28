﻿using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.AbominationSkills
{
    public class BloodCurseSkill : Skill
    {
        int percent = 20;
        public BloodCurseSkill()
        {
            name = "Blood Curse";
            title = $"Проклинает врага. Когда он получает урон, то {percent}% от потерянного процента ХП восстанавливается Вам.";
            titleUpg = "+10% к восстановлению. -1 к перезарядке";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 1;
            range = 2;
            nonTarget = false;
            area = Consts.SpellArea.EnemyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new EnemyTargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.Target != null)
            {
                BloodCurseDebuff bloodCurseDebuff = new BloodCurseDebuff(requestData.Caster.Id, percent, 2);
                requestData.Target.AddEffect(bloodCurseDebuff);

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
                percent += 10;
                title = $"Проклинает врага. Когда он получает урон, то {percent}% от потерянного процента ХП восстанавливается Вам.";
                return true;
            }
            return false;
        }
    }
}
