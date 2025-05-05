using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.ShadowSkills
{
    public class ShadowWalkSkill : Skill
    {
        public ShadowWalkSkill()
        {
            name = "Shadow Walk";
            range = 3;
            title = $"Мгновенно перемещает Вас в тенях на расстояние до {range} клеток.";
            titleUpg = "Не требует очков действия";
            coolDown = 3;
            coolDownNow = 0;
            requireAP = 1;
            nonTarget = false;
            area = Consts.SpellArea.Radius;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Physical;
        }

        public new ISkillCastRequest request => new HexTargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.CasterHex != null && requestData.TargetHex != null && requestData.TargetHex.IsFree())
            {
                AttackService.MoveHero(requestData.Caster, requestData.CasterHex, requestData.TargetHex);

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
                requireAP = 0;
                return true;
            }
            return false;
        }
    }
}
