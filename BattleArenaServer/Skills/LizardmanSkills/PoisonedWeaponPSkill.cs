using BattleArenaServer.Effects;
using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Models;

namespace BattleArenaServer.Skills.LizardmanSkills
{
    public class PoisonedWeaponPSkill : PassiveSkill
    {
        int poisonPercent = 6;
        public PoisonedWeaponPSkill(Hero hero) : base(hero)
        {
            name = "Poisoned Weapon";
            title = $"Ваши атаки накладывают яд, который отнимает {poisonPercent}% от оставшегося ХП врага.";
            titleUpg = "+4% к силе яда";
            skillType = Consts.SkillType.Passive;
            hero.beforeAttack += PoisonedWeapon;
        }

        public override bool Cast(RequestData requestData, GameData gameData)
        {
            return false;
        }

        public override void refreshEffect(GameData gameData)
        {
        }

        public override bool UpgradeSkill(GameData gameData)
        {
            if (!upgraded)
            {
                upgraded = true;
                hero.beforeAttack -= PoisonedWeapon;
                poisonPercent += 5;
                hero.beforeAttack += PoisonedWeapon;
                title = $"Ваши атаки накладывают яд, который отнимает {poisonPercent}% от оставшегося ХП врага.";
                return true;
            }
            return false;
        }

        private bool PoisonedWeapon(Hero attacker, Hero defender, int dmg, GameData gameData)
        {
            Effect? poison = defender.EffectList.FirstOrDefault(x => x.Name == "Poison" && x.idCaster == attacker.Id);
            if (poison == null)
            {
                PoisonDebuff poisonDebuff = new PoisonDebuff(attacker.Id, poisonPercent, 2);
                defender.AddEffect(poisonDebuff, gameData);
            }
            else if (poison.value < poisonPercent || poison.duration < 2)
            {
                poison.RemoveEffect(defender, gameData);
                defender.EffectList.Remove(poison);

                PoisonDebuff poisonDebuff = new PoisonDebuff(attacker.Id, poisonPercent, 2);
                defender.AddEffect(poisonDebuff, gameData);
            }
            return true;
        }
    }
}
