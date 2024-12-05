using BattleArenaServer.Effects.Unique;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.AssassinSkills
{
    public class AdrenalinSkill : Skill
    {
        int heal = 80;
        int extraAP = 2;
        public AdrenalinSkill()
        {
            name = "Adrenalin";
            title = $"Мгновенно даёт +{extraAP} дополнительных очков действия. Однако в следующий ход у Вас будет на {extraAP} ОД меньше.";
            titleUpg = $"Также восстанавливает {heal} ХП.";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 0;
            nonTarget = true;
            area = Consts.SpellArea.NonTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new NontargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null)
            {
                if (upgraded)
                    requestData.Caster.Heal(heal);

                AdrenalinUnique adrenalinUnique = new AdrenalinUnique(requestData.Caster.Id, extraAP, 1);
                requestData.Caster.AddEffect(adrenalinUnique);
                adrenalinUnique.ApplyEffect(requestData.Caster);

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
                title = title + " " + titleUpg;
                return true;
            }
            return false;
        }
    }
}
