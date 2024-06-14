using BattleArenaServer.Effects;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;

namespace BattleArenaServer.Services
{
    public class TimingService : ITiming
    {
        private int idActiveHero = 0;

        public TimingService()
        {
        }

        public int GetActiveHero()
        {
            return idActiveHero;
        }

        public void EndTurn()
        {
            Hero? activeHero = GameData._heroes.FirstOrDefault(x => x.Id == idActiveHero);
            if (activeHero != null)
            {
                AttackService.EndTurnAuraAction(activeHero);
                DecreaseSkillCooldawn(activeHero);
                EndTurnStatusApplyEffect(activeHero);
                DecreaseLifeTimeObstacle(activeHero);

                activeHero.AP = 4;
                Hero? hero = null;

                while (hero == null)
                {
                    DecreaseStatusDuration(idActiveHero);
                    idActiveHero++;
                    hero = GameData._heroes.FirstOrDefault(x => x.Id == idActiveHero && x.HP > 0);
                    idActiveHero = idActiveHero >= GameData._heroes.Count ? -1 : idActiveHero;
                }
            }            
        }

        public void DecreaseStatusDuration(int heroId)
        {
            foreach (var hero in GameData._heroes)
            {
                List<Effect> removeEffects = new List<Effect>();
                foreach (var effect in hero.EffectList)
                {
                    if (effect.idCaster == heroId && effect.duration > 1)
                        effect.duration--;
                    else if (effect.idCaster == heroId && effect.duration == 1)
                    {
                        effect.RemoveEffect(hero);
                        removeEffects.Add(effect);
                    }
                }
                foreach (var effect in removeEffects)
                {
                    hero.EffectList.Remove(effect);
                }
            }
        }

        public void DecreaseSkillCooldawn(Hero hero)
        {
            foreach (var skill in hero.SkillList)
            {
                if (skill.coolDownNow > 0)
                    skill.coolDownNow--;
            }
        }

        public void DecreaseLifeTimeObstacle(Hero hero)
        {
            foreach (var obst in GameData._obstacles)
            {
                if (obst.CasterId == hero.Id)
                {
                    if (--obst.LifeTime <= 0)
                    {
                        Hex? hex = GameData._hexes.FirstOrDefault(x => x.ID == obst.HexId);
                        if (hex != null)
                            hex.RemoveObstacle();
                    }
                }
            }
        }

        public void EndTurnStatusApplyEffect(Hero hero)
        {
            if (hero.HP > 0)
            {
                foreach (var effect in hero.EffectList)
                {
                    if (effect.effectType == Consts.EffectType.EndTurn)
                        effect.ApplyEffect(hero);
                }
            }
        }
    }
}
