using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Models.Summons;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills._SummonsSkills
{
    public class HealingSpraySkill : Skill
    {
        int heal = 70;
        public HealingSpraySkill(bool upgraded)
        {
            name = "Healing Spray";
            title = $"Восстанавливает союзнику {heal} ХП.";
            coolDown = 1;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            range = 1;
            area = Consts.SpellArea.AllyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);

            heal = upgraded ? 90 : 70;
        }

        public new ISkillCastRequest request => new AllyTargetCastRequest();

        public override bool Cast(RequestData requestData, GameData gameData)
        {
            if (!request.startRequest(requestData, this, gameData))
                return false;

            if (requestData.Caster != null && requestData.Target != null && requestData.Target is not Summon)
            {
                requestData.Target.Heal(heal);

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
                return true;
            }
            return false;
        }
    }
}
