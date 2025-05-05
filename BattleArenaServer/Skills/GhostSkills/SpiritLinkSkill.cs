using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.GhostSkills
{
    public class SpiritLinkSkill : Skill
    {
        public SpiritLinkSkill()
        {
            name = "Spirit Link";
            title = $"Сцепляет себя с союзником невидимой связью. Ваши ОД становятся общими в использовании.";
            titleUpg = "+1 к дальности, -1 к перезарядке";
            coolDown = 5;
            coolDownNow = 0;
            requireAP = 0;
            range = 2;
            nonTarget = false;
            area = Consts.SpellArea.FriendTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new FriendTargetCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (requestData.Target != null && requestData.Caster != null && requestData.Target.IsMainHero)
            {
                if (!request.startRequest(requestData, this))
                    return false;

                SpiritLinkBuff spiritLinkTarget = new SpiritLinkBuff(requestData.Caster.Id, 0, 1, requestData.Caster.Name);
                requestData.Target.AddEffect(spiritLinkTarget);

                SpiritLinkBuff spiritLinkCaster = new SpiritLinkBuff(requestData.Target.Id, 0, 1, requestData.Target.Name);
                requestData.Caster.AddEffect(spiritLinkCaster);

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
                coolDown -= 1;
                stats.coolDown -= 1;
                range += 1;
                stats.range += 1;
                return true;
            }
            return false;
        }
    }
}
