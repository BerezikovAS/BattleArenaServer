using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.DwarfSkills
{
    public class TauntSkill : Skill
    {
        int armor = 3;
        public TauntSkill()
        {
            name = "Taunt";
            title = $"Провоцирует врага атаковать только Вас. Враг не может применять способности, а Вы получаете +{armor} брони.";
            titleUpg = "+1 к броне. Также запрещает применять предметы";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 1;
            range = 1;
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
                TauntDebuff tauntDebuff = new TauntDebuff(requestData.Caster.Id, armor, 2, requestData.Caster.Name, upgraded);
                requestData.Target.AddEffect(tauntDebuff);

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
                armor += 1;
                title = $"Провоцирует врага атаковать только Вас. Враг не может применять способности и предметы, а Вы получаете +{armor} брони.";
                return true;
            }
            return false;
        }
    }
}
