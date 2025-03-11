using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.ElementalistSkills
{
    public class AquaSkill : Skill
    {
        private int reduceResist = 2;
        public AquaSkill()
        {
            name = "Aqua";
            dmg = 140;
            title = $"Обрушивает мощный град на врагов в области, нанося {dmg} магического урона и снижает сопротивление магии на {reduceResist}";
            titleUpg = "+1 к дальности, +2 к снижению сопротивления";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            range = 2;
            radius = 1;
            area = Consts.SpellArea.Radius;
            dmgType = Consts.DamageType.Magic;
            stats = new SkillStats(coolDown, requireAP, range, radius);
        }

        public new ISkillCastRequest request => new RangeAoECastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.TargetHex != null)
            {
                foreach (var n in UtilityService.GetHexesRadius(requestData.TargetHex, radius))
                {
                    if (n.HERO != null && n.HERO.Team != requestData.Caster.Team)
                    {
                        AttackService.SetDamage(requestData.Caster, n.HERO, dmg, Consts.DamageType.Magic);

                        if (n.HERO != null)
                        {
                            ResistDebuff resistDebuff = new ResistDebuff(requestData.Caster.Id, reduceResist, 2);
                            n.HERO.AddEffect(resistDebuff);
                            resistDebuff.ApplyEffect(n.HERO);
                        }
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
                range += 1;
                stats.range += 1;
                reduceResist += 2;
                return true;
            }
            return false;
        }
    }
}
