using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;
using System.Xml.Linq;
using System;
using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Skills.Crossbowman.Obstacles;

namespace BattleArenaServer.Skills.AeroturgSkills
{
    public class ThunderWaveSkill : Skill
    {
        int dmg = 100;
        int resistReduction = 2;
        public ThunderWaveSkill()
        {
            name = "Thunder Wave";
            title = $"Расталкивает врагов вокруг себя, нанося им {dmg} магического урона. Враги получают немоту.";
            titleUpg = $"Также снижает сопротивление врагов на {resistReduction}";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            radius = 1;
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
                    foreach (var hex in UtilityService.GetHexesRadius(targetHex, 1))
                    {
                        if (hex.HERO != null && hex.HERO.Team != caster.Team)
                        {
                            SilenceDebuff silenceDebuff = new SilenceDebuff(caster.Id, 0, 2);
                            hex.HERO.EffectList.Add(silenceDebuff);

                            if (upgraded)
                            {
                                ResistDebuff resistDebuff = new ResistDebuff(caster.Id, resistReduction, 2);
                                hex.HERO.EffectList.Add(resistDebuff);
                                resistDebuff.ApplyEffect(hex.HERO);
                            }

                            AttackService.SetDamage(caster, hex.HERO, dmg, Consts.DamageType.Magic);

                            Hex? moveHex = UtilityService.GetOneHexOnDirection(targetHex, hex, 2);
                            if (moveHex != null && moveHex.HERO == null)
                                AttackService.MoveHero(hex.HERO, hex, moveHex);
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
                return true;
            }
            return false;
        }
    }
}
