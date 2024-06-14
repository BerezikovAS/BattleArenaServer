using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;
using BattleArenaServer.Effects.Debuffs;

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

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.TargetHex != null)
            {
                foreach (var hex in UtilityService.GetHexesRadius(requestData.TargetHex, 1))
                {
                    if (hex.HERO != null && hex.HERO.Team != requestData.Caster.Team)
                    {
                        SilenceDebuff silenceDebuff = new SilenceDebuff(requestData.Caster.Id, 0, 2);
                        hex.HERO.EffectList.Add(silenceDebuff);

                        if (upgraded)
                        {
                            ResistDebuff resistDebuff = new ResistDebuff(requestData.Caster.Id, resistReduction, 2);
                            hex.HERO.EffectList.Add(resistDebuff);
                            resistDebuff.ApplyEffect(hex.HERO);
                        }

                        AttackService.SetDamage(requestData.Caster, hex.HERO, dmg, Consts.DamageType.Magic);

                        Hex? moveHex = UtilityService.GetOneHexOnDirection(requestData.TargetHex, hex, 2);
                        if (moveHex != null && moveHex.IsFree())
                            AttackService.MoveHero(hex.HERO, hex, moveHex);
                    }
                }

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
                return true;
            }
            return false;
        }
    }
}
