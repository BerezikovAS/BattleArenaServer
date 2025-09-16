using BattleArenaServer.Models;
using BattleArenaServer.Services;

namespace BattleArenaServer.Effects.Buffs
{
    public class LizardSkinBuff : Effect
    {
        int resist = 0;
        public LizardSkinBuff(int _idCaster, int _value, int _duration, int _resist)
        {
            Name = "LizardSkin";
            type = Consts.StatusEffect.Buff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            resist = _resist;
            description = $"+{resist} к сопротивлению. {value}% полученного магического урона отражается во врагов вокруг.";
        }

        public override void ApplyEffect(Hero _hero, GameData gameData)
        {
            _hero.StatsEffect.Resist += resist;
            _hero.beforeReceiveDmg += BeforeReceiveDmgDelegate;
        }

        public override void RemoveEffect(Hero _hero, GameData gameData)
        {
            _hero.StatsEffect.Resist -= resist;
            _hero.beforeReceiveDmg -= BeforeReceiveDmgDelegate;
        }

        private void BeforeReceiveDmgDelegate(Hero defender, Hero? attacker, int dmg, Consts.DamageType dmgType, GameData gameData)
        {
            if (dmgType != Consts.DamageType.Magic)
                return;

            Hex? defenderHex = gameData._hexes.FirstOrDefault(x => x.HERO != null && x.HERO.Id == defender.Id);
            if (defenderHex == null)
                return;

            int dmgDeflect = (int)(Convert.ToDouble(dmg) * Convert.ToDouble(value) / 100);
            foreach (Hex hex in UtilityService.GetHexesRadius(defenderHex, 1, gameData))
            {
                if (hex.HERO != null && hex.HERO.Team != defender.Team)
                    AttackService.SetDamage(defender, hex.HERO, dmgDeflect, Consts.DamageType.Magic, gameData);
            }
        }
    }
}
