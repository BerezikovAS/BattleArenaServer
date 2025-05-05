using BattleArenaServer.Models;

namespace BattleArenaServer.Skills.MusketeerSKills
{
    public class GunPowerPSkill : PassiveSkill
    {
        int armorPiercing = 2;
        public GunPowerPSkill(Hero hero) : base(hero)
        {
            name = "Gun Power";
            title = $"Мощь огнестрельного оружия игнорирует до {armorPiercing} брони.";
            titleUpg = "+2 к игнорированию брони";
            skillType = Consts.SkillType.Passive;
            hero.armorPiercing += ArmorPiercing;
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
            if (!upgraded)
            {
                upgraded = true;
                hero.armorPiercing -= ArmorPiercing;
                armorPiercing += 2;
                hero.armorPiercing += ArmorPiercing;
                title = $"Мощь огнестрельного оружия игнорирует до {armorPiercing} брони.";
                return true;
            }
            return false;
        }

        private int ArmorPiercing(Hero attacker, Hero defender)
        {
            return armorPiercing;
        }
    }
}
