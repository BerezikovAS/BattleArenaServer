using BattleArenaServer.Effects;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Models.Obstacles;

namespace BattleArenaServer.Services
{
    public class TimingService : ITiming
    {
        private int idActiveHero = 0;
        private int turn = 1;

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
                    if (idActiveHero >= GameData._heroes.Count)
                    {
                        AddUpgradePoints(++turn);
                        idActiveHero = -1;
                    }
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
            // Пробегаемся по обычным преградам
            foreach (var obst in GameData._obstacles)
            {
                if (obst.CasterId == hero.Id)
                {
                    if (--obst.LifeTime <= 0)
                    {
                        Hex? hex = GameData._hexes.FirstOrDefault(x => x.ID == obst.HexId);
                        if (hex != null)
                            hex.RemoveObstacle();
                        AttackService.ContinuousAuraAction();
                    }
                }
            }

            // Теперь по блокирующим перемещение
            List<SolidObstacle> removeObst = new List<SolidObstacle>();
            foreach (var obst in GameData._solidObstacles)
            {
                if (obst.casterId == hero.Id)
                {
                    if (--obst.lifeTime <= 0)
                    {
                        Hex? hex = GameData._hexes.FirstOrDefault(x => x.ID == obst.HexId);
                        if (hex != null)
                            hex.RemoveHero();
                        AttackService.ContinuousAuraAction();
                        removeObst.Add(obst);
                    }
                }
            }
            // Удалем то что уже протухло
            foreach (var obst in removeObst)
                GameData._solidObstacles.Remove(obst);
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

        public void AddUpgradePoints(int turn)
        {
            if (turn % 2 == 0)
                foreach (var hero in GameData._heroes)
                    hero.UpgradePoints++;
        }
    }
}
