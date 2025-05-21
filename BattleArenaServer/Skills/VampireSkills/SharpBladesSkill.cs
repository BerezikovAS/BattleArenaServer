using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.VampireSkills
{
    public class SharpBladesSkill : Skill
    {
        int bleedingDmg = 30;
        public SharpBladesSkill()
        {
            name = "Sharp Blades";
            dmg = 150;
            title = $"Наносит врагам по соседству {dmg} маг. урона и накладывает на них кровотечение на 2 хода, которое отнимает по {bleedingDmg} ХП за раунд.";
            titleUpg = "+1 гекс к зоне поражения, +10 к урону от кровотечения";
            coolDown = 4;
            coolDownNow = 0;
            requireAP = 2;
            nonTarget = false;
            range = 1;
            radius = 1;
            area = Consts.SpellArea.SmallConus;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Magic;
        }

        public new ISkillCastRequest request => new LineCastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null && requestData.TargetHex != null && requestData.CasterHex != null)
            {
                List<Hex> targetHexes = new List<Hex>();
                if (upgraded)
                    targetHexes = UtilityService.GetHexesCone(requestData.CasterHex, requestData.TargetHex, radius);
                else
                    targetHexes = UtilityService.GetHexesSmallCone(requestData.CasterHex, requestData.TargetHex, radius);

                foreach (var n in targetHexes)
                {
                    if (n.HERO != null && n.HERO.Team != requestData.Caster.Team)
                    {
                        BleedingDebuff bleedingDebuff = new BleedingDebuff(requestData.Caster.Id, bleedingDmg, 3);
                        n.HERO.AddEffect(bleedingDebuff);

                        AttackService.SetDamage(requestData.Caster, n.HERO, dmg, dmgType);
                    }
                }

                requestData.Caster.SpendAP(requireAP);
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
                bleedingDmg += 10;
                area = Consts.SpellArea.Conus;
                title = $"Наносит врагам по соседству {dmg} маг. урона и накладывает на них кровотечение на 2 хода, которое отнимает по {bleedingDmg} ХП за раунд.";
                return true;
            }
            return false;
        }
    }
}
