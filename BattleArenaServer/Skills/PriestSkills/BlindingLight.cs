using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.Priest
{
    public class BlindingLight : Skill
    {
        public BlindingLight()
        {
            name = "BlindingLight";
            title = "Враги в области действия теряют 100 ХП и получают ослепление";
            titleUpg = "-1 к перезарядке, +1 к дальности";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            range = 2;
            radius = 1;
            area = Consts.SpellArea.Radius;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new RangeAoECastRequest();

        public override void Cancel()
        {
            throw new NotImplementedException();
        }

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
                            BlindDebuff blindDebuff = new BlindDebuff(caster.Id, 0, 2);
                            n.HERO.EffectList.Add(blindDebuff);
                            blindDebuff.ApplyEffect(n.HERO);
                            AttackService.SetDamage(caster, n.HERO, 100, Consts.DamageType.Pure);
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
                range += 1;
                coolDown -= 1;
                stats.range += 1;
                stats.coolDown -= 1;
                return true;
            }
            return false;
        }
    }
}
