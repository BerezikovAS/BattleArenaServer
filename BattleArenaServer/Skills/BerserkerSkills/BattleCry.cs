using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using System.Xml.Linq;
using System;
using BattleArenaServer.Interfaces;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.BerserkerSkills
{
    public class BattleCry : Skill
    {
        int decreaseArmor = 2;
        public BattleCry()
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

        public override bool Cast(Hero caster, Hero? target, Hex? targetHex)
        {
            if (request.startRequest(caster, target, targetHex, this))
            {
                if (caster != null && targetHex != null)
                {
                    foreach (var n in UtilityService.GetHexesRadius(targetHex, radius))
                    {
                        if (n.HERO != null && n.HERO.Team != caster.Team)
                        {
                            int decrArmor = decreaseArmor;
                            if (upgraded && targetHex.Distance(n) == 1)
                                decrArmor = 2 * decreaseArmor;

                            ArmorDebuff armorDebuff = new ArmorDebuff(caster.Id, decrArmor, 2);
                            n.HERO.EffectList.Add(armorDebuff);
                            armorDebuff.ApplyEffect(n.HERO);
                        }
                    }
                    caster.AP -= requireAP;
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
