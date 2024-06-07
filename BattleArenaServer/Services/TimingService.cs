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

        public int EndTurn()
        {
            Hero? activeHero = GameData._heroes.FirstOrDefault(x => x.Id == idActiveHero);
            if (activeHero != null)
            {
                DecreaseStatusDuration();
                DecreaseSkillCooldawn(activeHero);

                activeHero.AP = 4;
                Hero? hero = null;

                while (hero == null)
                {
                    idActiveHero++;
                    hero = GameData._heroes.FirstOrDefault(x => x.Id == idActiveHero && x.HP > 0);
                    idActiveHero = idActiveHero >= GameData._heroes.Count ? -1 : idActiveHero;
                }
            }            
            return idActiveHero;
        }

        public void DecreaseStatusDuration()
        {
            foreach (var hero in GameData._heroes)
            {
                List<Effect> removeEffects = new List<Effect>();
                foreach (var effect in hero.EffectList)
                {
                    if (effect.idCaster == idActiveHero && effect.duration > 1)
                        effect.duration--;
                    else if (effect.idCaster == idActiveHero && effect.duration == 1)
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

        public void DecreaseSkillCooldawn(Hero _hero)
        {
            foreach (var skill in _hero.SkillList)
            {
                if (skill.coolDownNow > 0)
                    skill.coolDownNow--;
            }
        }
    }
}
