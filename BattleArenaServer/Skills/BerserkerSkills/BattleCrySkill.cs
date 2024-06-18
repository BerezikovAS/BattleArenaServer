using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.Interfaces;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.BerserkerSkills
{
    public class BattleCrySkill : Skill
    {
        int decreaseArmor = 2;
        public BattleCrySkill()
        {
            name = "Battle Cry";
            title = $"Боевой клич устрашает врагов и снижает их броню на {decreaseArmor}.";
            titleUpg = "Снижение брони для рядомстоящих врагов удваивается. -1 к длительности перезарядки";
            coolDown = 3;
            coolDownNow = 0;
            requireAP = 1;
            nonTarget = false;
            radius = 2;
            range = 0;
            area = Consts.SpellArea.Radius;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new NonTargerAoECastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (request.startRequest(requestData, this))
            {
                if (requestData.Caster != null && requestData.TargetHex != null)
                {
                    foreach (var n in UtilityService.GetHexesRadius(requestData.TargetHex, radius))
                    {
                        if (n.HERO != null && n.HERO.Team != requestData.Caster.Team)
                        {
                            int decrArmor = decreaseArmor;
                            if (upgraded && requestData.TargetHex.Distance(n) == 1)
                                decrArmor = 2 * decreaseArmor;

                            ArmorDebuff armorDebuff = new ArmorDebuff(requestData.Caster.Id, decrArmor, 2);
                            n.HERO.AddEffect(armorDebuff);
                            armorDebuff.ApplyEffect(n.HERO);
                        }
                    }
                    requestData.Caster.AP -= requireAP;
                    coolDownNow = coolDown;
                    return true;
                }
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
                return true;
            }
            return false;
        }
    }
}
