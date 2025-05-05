using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.GolemSkills
{
    public class FortificationSkill : Skill
    {
        int extraArmorResist = 1;
        int defaultArmorResist = 0;
        public FortificationSkill()
        {
            name = "Fortification";
            title = $"Тратит все свои ОД и за каждое потраченное очко получает +{extraArmorResist} брони и сопротивления.";
            titleUpg = "+2 брони и сопротивления по умолчанию";
            coolDown = 1;
            coolDownNow = 0;
            requireAP = 1;
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
                int usedAP = requestData.Caster.AP;
                int addArmorResist = usedAP * extraArmorResist + defaultArmorResist;

                ArmorBuff armorBuff = new ArmorBuff(requestData.Caster.Id, addArmorResist, 2);
                requestData.Caster.AddEffect(armorBuff);

                ResistBuff resistBuff = new ResistBuff(requestData.Caster.Id, addArmorResist, 2);
                requestData.Caster.AddEffect(resistBuff);

                requestData.Caster.SpendAP(usedAP);
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
                defaultArmorResist += 2;
                title = $"Тратит все свои ОД и за каждое потраченное очко получает +{extraArmorResist} брони и сопротивления.\n" +
                    $"По умолчанию получает +{defaultArmorResist} брони и сопротивления.";
                return true;
            }
            return false;
        }
    }
}
