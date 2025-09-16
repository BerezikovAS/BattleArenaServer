using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.LizardmanSkills
{
    public class LizardSkinSkill : Skill
    {
        int extraResist = 2;
        int percentDeflect = 40;
        public LizardSkinSkill()
        {
            name = "Lizard Skin";
            title = $"Вы получаете +{extraResist} к сопротивлению, а {percentDeflect}% полученного магического урона отражается во врагов по соседству.";
            titleUpg = $"+20% к отражению магического урона, +1 к сопротивлению";
            coolDown = 4;
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
                    LizardSkinBuff lizardSkinBuff = new LizardSkinBuff(requestData.Caster.Id, percentDeflect, 3, extraResist);
                    requestData.Caster.AddEffect(lizardSkinBuff, gameData);

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
                extraResist += 1;
                percentDeflect += 20;
                title = $"Вы получаете +{extraResist} к сопротивлению, а {percentDeflect}% полученного магического урона отражается во врагов по соседству.";
                return true;
            }
            return false;
        }
    }
}
