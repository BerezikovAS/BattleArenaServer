using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.LizardmanSkills
{
    public class DodgeSkill : Skill
    {
        public DodgeSkill()
        {
            name = "Dodge";
            title = $"Игнориует весь урон и эффекты от одной атаки.";
            titleUpg = $"Не требует ОД";
            coolDown = 3;
            coolDownNow = 0;
            requireAP = 1;
            nonTarget = true;
            area = Consts.SpellArea.NonTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new NontargetCastRequest();

        public override bool Cast(RequestData requestData, GameData gameData)
        {
            if (request.startRequest(requestData, this, gameData))
            {
                if (requestData.Caster != null)
                {
                    DodgeBuff dodgeBuff = new DodgeBuff(requestData.Caster.Id, 0, 3);
                    requestData.Caster.AddEffect(dodgeBuff, gameData);

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
                requireAP = 0;
                return true;
            }
            return false;
        }
    }
}
