using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.ArtistSkills
{
    public class ClimaxSkill : Skill
    {
        int percentDmg = 100;
        public ClimaxSkill()
        {
            name = "Climax";
            title = $"Кульминация мелодии увеличивает следующий полученный урон целью на {percentDmg}%";
            titleUpg = "+35% к увеличению урона, +1 к дальности";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 1;
            range = 2;
            nonTarget = false;
            area = Consts.SpellArea.EnemyTarget;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new EnemyTargetCastRequest();

        public override bool Cast(RequestData requestData, GameData gameData)
        {
            if (!request.startRequest(requestData, this, gameData))
                return false;

            if (requestData.Caster != null && requestData.Target != null)
            {
                ClimaxDebuff climaxDebuff = new ClimaxDebuff(requestData.Caster.Id, percentDmg, 2);
                requestData.Target.AddEffect(climaxDebuff, gameData);

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
                percentDmg += 35;
                range += 1;
                stats.range += 1;
                title = $"Кульминация мелодии увеличивает следующий полученный урон целью на {percentDmg}%";
                return true;
            }
            return false;
        }
    }
}
