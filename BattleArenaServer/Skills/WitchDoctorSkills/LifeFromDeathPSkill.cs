using BattleArenaServer.Models;
using BattleArenaServer.Services;

namespace BattleArenaServer.Skills.WitchDoctorSkills
{
    public class LifeFromDeathPSkill : PassiveSkill
    {
        int percentHeal = 15;
        public LifeFromDeathPSkill(Hero hero) : base(hero)
        {
            name = "Life From Death";
            title = $"Получая урон, Вы восстанавливаете ХП союзникам вокруг. Размер лечения равняется {percentHeal}% от полученного урона.";
            titleUpg = $"+7% к силе восстановления";
            skillType = Consts.SkillType.Passive;
            hero.afterReceiveDmg += AfterReceiveDmgDelegate;
        }

        public override bool Cast(RequestData requestData)
        {
            return false;
        }

        public override void refreshEffect()
        {
        }

        public override bool UpgradeSkill()
        {
            upgraded = true;
            hero.afterReceiveDmg -= AfterReceiveDmgDelegate;
            percentHeal += 7;
            hero.afterReceiveDmg += AfterReceiveDmgDelegate;
            title = $"Получая урон, Вы восстанавливаете ХП союзникам вокруг. Размер лечения равняется {percentHeal}% от полученного урона.";
            return true;
        }

        private void AfterReceiveDmgDelegate(Hero defender, Hero? attacker, int dmg, Consts.DamageType dmgType)
        {
            Hex? defenderHex = GameData._hexes.FirstOrDefault(x => x.ID == defender.HexId);
            if (defenderHex != null)
            {
                foreach (var hex in UtilityService.GetHexesRadius(defenderHex, 1))
                    if (hex.HERO != null && hex.HERO.Team == defender.Team && hex.HERO.Id != defender.Id)
                        hex.HERO.Heal((int)(Convert.ToDouble(dmg) * Convert.ToDouble(percentHeal) / 100));
            }
        }
    }
}
