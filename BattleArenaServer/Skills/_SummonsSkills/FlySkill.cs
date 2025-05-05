using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills._SummonsSkills
{
    public class FlySkill : Skill
    {
        public FlySkill(bool upgraded)
        {
            name = "Fly";
            range = upgraded ? 3 : 2;
            title = $"Беспрепятственный полет до {range} клеток.";
            coolDown = 1;
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
                requestData.CasterHex.RemoveHero();
                requestData.TargetHex.SetHero(requestData.Caster);

                //Обновим ауры
                AttackService.ContinuousAuraAction();

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
                return true;
            }
            return false;
        }
    }
}
