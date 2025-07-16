using BattleArenaServer.Models;
using BattleArenaServer.Services;

namespace BattleArenaServer.Effects.Debuffs
{
    public class CopperCageDebuff : Effect
    {
        int dmg = 0;
        public CopperCageDebuff(int _idCaster, int _value, int _duration, int _dmg)
        {
            Name = "CopperCage";
            type = Consts.StatusEffect.Debuff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            dmg = _dmg;
            description = $"Если цель не переместится на {value} клеток, то будет обездвижена и получит {dmg} магического урона.";
        }

        public override void ApplyEffect(Hero hero, GameData gameData)
        {
            hero.afterMove += MovementCommited;
        }

        public override void RemoveEffect(Hero hero, GameData gameData)
        {
            hero.afterMove -= MovementCommited;
        }

        public override void ApplyAfterEffect(Hero hero, GameData gameData)
        {
            Hero? attacker = gameData._heroes.FirstOrDefault(x => x.Id == idCaster);
            AttackService.SetDamage(attacker, hero, dmg, Consts.DamageType.Magic, gameData);

            RootDebuff rootDebuff = new RootDebuff(idCaster, 0, 1);
            rootDebuff.ApplyEffect(hero, gameData);
            hero.AddEffect(rootDebuff, gameData);
        }

        private void MovementCommited(Hero hero, Hex? currentHex, Hex targetHex, GameData gameData)
        {
            if (currentHex != null)
            {
                int distance = currentHex.Distance(targetHex);
                value -= distance;

                if (value <= 0)
                {
                    RemoveEffect(hero, gameData);
                    hero.EffectList.Remove(this);
                }
            }
        }
    }
}
