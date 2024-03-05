using BattleArenaServer.Effects;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;

namespace BattleArenaServer.Services
{
    public class TimingService : ITiming
    {
        private readonly IField _field;
        private int idActiveHero = 0;
        private List<int> initList;
        public List<Hero> heroList;

        public TimingService(IField field)
        {
            _field = field;
            heroList = field.GetHeroes();
        }

        public int EndTurn()
        {
            DecreaseStatusDuration();
            DecreaseSkillCooldawn(heroList.FirstOrDefault(x => x.Id == idActiveHero));

            heroList.FirstOrDefault(x => x.Id == idActiveHero).AP = 4;

            Hero hero = heroList.FirstOrDefault(x => x.Id > idActiveHero && x.HP > 0) ?? heroList.FirstOrDefault(x => x.Id < idActiveHero && x.HP > 0);
            idActiveHero = hero == null ? -1 : hero.Id;
            return idActiveHero;
        }

        public void DecreaseStatusDuration()
        {
            foreach (var hero in heroList)
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

        public void SetHeroList(List<Hero> _heroes)
        {
            //heroes = _heroes;
        }
    }
}
