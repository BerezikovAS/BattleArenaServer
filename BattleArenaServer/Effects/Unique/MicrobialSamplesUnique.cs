using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Unique
{
    public class MicrobialSamplesUnique : Effect
    {
        bool[] valAchived = new bool[6];
        int[] stats = new int[4]; //0 - броня, 1 - резист, 2 - атака, 3 - ХП
        bool removed = false;
        int isUpgraded = 0;

        public MicrobialSamplesUnique(int _idCaster, int _value, int _duration, bool _isUpgraded)
        {
            Name = "MicrobialSamples";
            type = Consts.StatusEffect.Unique;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            effectType = Consts.EffectType.Instant;
            isUpgraded = _isUpgraded ? 1 : 0;
            effectTags.Add(Consts.EffectTag.MicrobialSamples);
        }

        public override void ApplyEffect(Hero _hero)
        {
            removed = false;
            
            if (value + (1 * isUpgraded) >= 5 && !valAchived[0]) // 5 // +1 Брони
            {
                _hero.Armor += 1;
                stats[0] += 1;
                valAchived[0] = true;
            }
            if (value + (2 * isUpgraded) >= 10 && !valAchived[1]) // 10 // +1 Резист
            {
                _hero.Resist += 1;
                stats[1] += 1;
                valAchived[1] = true;
            }
            if (value + (3 * isUpgraded) >= 15 && !valAchived[2]) // 15 // +10 Урона
            {
                _hero.Dmg += 10;
                stats[2] += 10;
                valAchived[2] = true;
            }
            if (value + (4 * isUpgraded) >= 20 && !valAchived[3]) // 20 // +100 ХП
            {
                _hero.MaxHP += 100;
                _hero.HP += 100;
                stats[3] += 100;
                valAchived[3] = true;
            }
            if (value + (5 * isUpgraded) >= 25 && !valAchived[4]) // 25 // +1 Брони +1 Резист
            {
                _hero.Armor += 1;
                _hero.Resist += 1;
                stats[0] += 1;
                stats[1] += 1;
                valAchived[4] = true;
            }
            if (value + (6 * isUpgraded) >= 30 && !valAchived[5]) // 30 // +10 Урона +50 ХП
            {
                _hero.Dmg += 10;
                _hero.MaxHP += 50;
                _hero.HP += 50;
                stats[2] += 10;
                stats[3] += 50;
                valAchived[5] = true;
            }

            description = $"Накоплено образцов: {value}";
            if (stats[0] > 0)
                description += $"\n+{stats[0]} брони";
            if (stats[1] > 0)
                description += $"\n+{stats[1]} сопротивления";
            if (stats[2] > 0)
                description += $"\n+{stats[2]} урона";
            if (stats[3] > 0)
                description += $"\n+{stats[3]} ХП";
        }

        public override void RemoveEffect(Hero _hero)
        {
            if (!removed)
            {
                _hero.Armor -= stats[0];
                _hero.Resist -= stats[1];
                _hero.Dmg -= stats[2];
                _hero.MaxHP -= stats[3];
                _hero.HP -= stats[3];
                if (_hero.HP <= 0)
                    _hero.HP = 1;
                removed = true;
            }
        }
    }
}
