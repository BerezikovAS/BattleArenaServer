using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.LizardmanSkills
{
    public class BodyHealingSkill : Skill
    {
        int regeneration = 60;
        int statReduce = 1;
        public BodyHealingSkill()
        {
            name = "Body Healing";
            title = $"Потеряйте {statReduce} броню и сопротивление, чтобы получить регенерацию в {regeneration} ХП на 2 хода.";
            titleUpg = "+40 к регенерации";
            coolDown = 2;
            coolDownNow = 0;
            requireAP = 0;
            nonTarget = true;
            area = Consts.SpellArea.NonTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new NontargetCastRequest();

        public override bool Cast(RequestData requestData, GameData gameData)
        {
            if (!request.startRequest(requestData, this, gameData))
                return false;

            if (requestData.Caster != null)
            {
                requestData.Caster.Armor -= statReduce;
                requestData.Caster.Resist -= statReduce;

                RegenerationBuff regenerationBuff = new RegenerationBuff(requestData.Caster.Id, regeneration, 2);
                requestData.Caster.AddEffect(regenerationBuff, gameData);

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
                regeneration += 40;
                title = $"Потеряйте {statReduce} броню и сопротивление, чтобы получить регенерацию в {regeneration} ХП на 2 хода.";
                return true;
            }
            return false;
        }
    }
}
