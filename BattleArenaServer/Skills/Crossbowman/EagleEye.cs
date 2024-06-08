using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;
using System.Xml.Linq;
using System;

namespace BattleArenaServer.Skills.Crossbowman
{
    public class EagleEye : Skill
    {
        int decreaseArmor = 2;
        public EagleEye()
        {
            name = "EagleEye";
            title = $"Увеличивает дальность атаки на 1 и урон на 20.";
            titleUpg = "+";
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
