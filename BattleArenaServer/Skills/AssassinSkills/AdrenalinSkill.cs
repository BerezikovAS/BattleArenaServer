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
            title = $"Мгновенно даёт +{extraAP} дополнительных очков действия и восстанавливает {heal} ХП. Однако в следующий ход у Вас будет на {extraAP} ОД меньше.";
            titleUpg = $"+40 к восстановлению ХП. -1 к перезарядке.";
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
                heal += 40;
                coolDown -= 1;
                stats.coolDown -= 1;
                title = $"Мгновенно даёт +{extraAP} дополнительных очков действия и восстанавливает {heal} ХП. Однако в следующий ход у Вас будет на {extraAP} ОД меньше.";
                return true;
            }
            return false;
        }
    }
}
