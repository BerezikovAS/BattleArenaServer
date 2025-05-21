using BattleArenaServer.Models;
using BattleArenaServer.Models.Obstacles;
using BattleArenaServer.Services;

namespace BattleArenaServer.Effects.Unique
{
    public class FrostBallUnique : Effect
    {
        int bounces = 0;
        bool firstBounce = false;
        Hero caster;
        public FrostBallUnique(int _idCaster, int _value, int _duration, int _bounces, Hero _caster)
        {
            Name = "FrostBall";
            type = Consts.StatusEffect.Unique;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            bounces = _bounces;
            caster = _caster;
            description = $"В конце хода морозный снаряд отскочит в союзника в радиусе 2-х клеток и нанесет тому {value} урона. Осталось отскоков: {bounces}";
            effectType = Consts.EffectType.EndTurn;
        }

        public override void ApplyEffect(Hero _hero)
        {
            if (firstBounce) //При первом применении не делаем отскок
            {
                firstBounce = false;
                return;
            }

            //Ищем героя в которого отскочит снаряд
            Hex? heroHex = GameData._hexes.FirstOrDefault(x => x.HERO != null && x.HERO.Id == _hero.Id);
            Hero? targetHero = null;
            if (heroHex != null)
            {
                int radius = 1;
                while (targetHero == null && radius <= 2)
                {
                    List<Hero?> heroes = GameData._hexes.FindAll(x => x.HERO != null && x.HERO.Team == _hero.Team && x.HERO.Id != _hero.Id && x.HERO is not SolidObstacle
                        && x.Distance(heroHex) == radius).Select(y => y.HERO).ToList();

                    if (heroes.Count() >= 1)
                    {
                        Random rnd = new Random();
                        targetHero = heroes[rnd.Next(heroes.Count())];
                    }

                    radius++;
                }
            }

            if (targetHero != null && --bounces >= 0)
            {
                description = $"В конце хода морозный снаряд отскочит в союзника в радиусе 2-х клеток и нанесет тому {value} урона. Осталось отскоков: {bounces}";
                if (_hero.Id < targetHero.Id)
                    firstBounce = true; //Эффекты конца хода для героя с большим Id будут отрабатываться после этого героя. Поэтому не делаем еще один отскок

                AttackService.SetDamage(caster, targetHero, value, Consts.DamageType.Magic);
                _hero.EffectList.Remove(this);
                targetHero.AddEffect(this);
            }
            else
            {
                _hero.EffectList.Remove(this);
            }
        }

        public override void RemoveEffect(Hero _hero)
        {

        }
    }
}
