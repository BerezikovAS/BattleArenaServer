using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.WitchDoctorSkills
{
    public class VoodooPuppetSkill : Skill
    {
        public VoodooPuppetSkill()
        {
            name = "Voodoo Puppet";
            title = $"Завораживает врага, отчего тот будет повторять Ваши движения.";
            titleUpg = "+1 к дальности. Способность не тратит ОД.";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 1;
            range = 2;
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
                VoodooPuppetDebuff voodooPuppetDebuff = new VoodooPuppetDebuff(requestData.Caster.Id, 0, 1);
                requestData.Target.AddEffect(voodooPuppetDebuff);

                requestData.Caster.AP -= requireAP;
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
                range += 1;
                stats.range += 1;
                requireAP = 0;
                stats.requireAP = 0;
                return true;
            }
            return false;
        }
    }
}
