using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Services;
using BattleArenaServer.SkillCastRequests;

namespace BattleArenaServer.Skills.CultistSkills
{
    public class RitualSkill : Skill
    {
        public RitualSkill()
        {
            name = "Ritual";
            dmg = 80;
            title = $"Добавляет 1 очко ритуала и наносит врагам по соседству {dmg} магического урона. Когда кол-во очков достигает 4, вы трансформируетесь в Древнего.";
            titleUpg = "+20 к урону, +1 к радиусу";
            coolDown = 1;
            coolDownNow = 0;
            requireAP = 1;
            radius = 1;
            range = 0;
            area = Consts.SpellArea.Radius;
            stats = new SkillStats(coolDown, requireAP, range, radius);
            dmgType = Consts.DamageType.Magic;
        }

        public new ISkillCastRequest request => new NonTargerAoECastRequest();

        public override bool Cast(RequestData requestData)
        {
            if (!request.startRequest(requestData, this))
                return false;

            if (requestData.Caster != null)
            {
                int ritualPower = 0;
                var ritual = requestData.Caster.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.Ritual));

                //Добавляем эффект ритуала, или просто увеличиваем значение, если он уже есть
                if (ritual != null)
                {
                    ritual.ApplyEffect(requestData.Caster);
                    ritualPower = ritual.value;
                }
                else
                {
                    RitualUnique ritualUnique = new RitualUnique(requestData.Caster.Id, 0, 100);
                    requestData.Caster.AddEffect(ritualUnique);
                }

                //Наносим урон
                foreach (var hex in UtilityService.GetHexesRadius(requestData.TargetHex, radius))
                {
                    if (hex.HERO != null && hex.HERO.Team != requestData.Caster.Team)
                        AttackService.SetDamage(requestData.Caster, hex.HERO, dmg, dmgType);
                }

                //Достигли нужного значения. Превращаемся
                if (ritualPower >= 4)
                {
                    requestData.Caster.Name = "Cthulhu";
                    requestData.Caster.MaxHP += 500;
                    requestData.Caster.HP += 500;
                    requestData.Caster.Armor += 4;
                    requestData.Caster.Resist += 2;
                    requestData.Caster.Dmg += 25;

                    requestData.Caster.SkillList[4] = new DevourSkill();
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
                dmg += 20;
                radius += 1;
                stats.radius += 1;
                title = $"Добавляет 1 очко ритуала и наносит врагам по соседству {dmg} магического урона. Когда кол-во очков достигает 4, вы трансформируетесь в Древнего.";
                return true;
            }
            return false;
        }
    }
}
