using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.TinkerSkill
{
    public class CopperCageSkill : Skill
    {
        public CopperCageSkill()
        {
            name = "Copper Cage";
            dmg = 60;
            title = $"Наносит {dmg} магического урона цели и над ней нависает медная клетка, если цель не передвинется хотя бы на 2 клетки, то получит еще {dmg} урона " +
                "и обездвижется.";
            titleUpg = "+25 к урону";
            coolDown = 3;
            coolDownNow = 0;
            requireAP = 1;
            nonTarget = false;
            range = 2;
            area = Consts.SpellArea.EnemyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Magic;
        }

        public new ISkillCastRequest request => new EnemyTargetCastRequest();

        public override bool Cast(RequestData requestData, GameData gameData)
        {
            if (!request.startRequest(requestData, this, gameData))
                return false;

            if (requestData.Caster != null && requestData.Target != null)
            {
                CopperCageDebuff copperCageDebuff = new CopperCageDebuff(requestData.Caster.Id, 2, 2, dmg);
                requestData.Target.AddEffect(copperCageDebuff, gameData);
                
                AttackService.SetDamage(requestData.Caster, requestData.Target, dmg, dmgType, gameData);

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
                dmg += 25;
                title = $"Наносит {dmg} магического урона цели и над ней нависает медная клетка, если цель не передвинется хотя бы на 2 клетки, то получит еще {dmg} урона " +
                    "и обездвижется.";
                return true;
            }
            return false;
        }
    }
}
