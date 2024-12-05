using BattleArenaServer.Effects;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Models.Obstacles;

namespace BattleArenaServer.Services
{
    public class TimingService : ITiming
    {
        private int turn = 1;

        public TimingService()
        {
            GameData.activeTeam = "red";
            GameData.idActiveHero = 0;
        }

        public int GetActiveHero()
        {
            return GameData.idActiveHero;
        }

        public int EndTurn()
        {
            foreach (var hero in GameData._heroes.Where(x => x.Team == GameData.activeTeam))
            {
                //Effect? modifyAP = hero.EffectList.FirstOrDefault(x => x.Name == "Adrenalin");
                //hero.AP = modifyAP == null ? 4 : 4 + modifyAP.value;
                hero.AP = 4;

                AttackService.EndTurnAuraAction(hero);
                DecreaseSkillCooldawn(hero);
                EndTurnStatusApplyEffect(hero);
                DecreaseLifeTimeObstacle(hero);

                DecreaseStatusDuration(hero.Id);
            }

            AddUpgradePoints(++turn);
            GameData.activeTeam = GameData.activeTeam == "blue" ? "red" : "blue";

            foreach (var hero in GameData._heroes.Where(x => x.Team == GameData.activeTeam && x.HP > 0))
            {
                RefreshStartTurnAbilities(hero);
                GameData.idActiveHero = hero.Id;
            }
            return GameData.idActiveHero;
        }

        public void RefreshStartTurnAbilities(Hero hero)
        {
            if (hero.SkillList[0] is PassiveSkill)
                (hero.SkillList[0] as PassiveSkill).refreshEffect();
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

            // Также по поверхностям
            List<FillableObstacle> removeSurf = new List<FillableObstacle>();
            foreach (var surface in GameData._surfaces)
            {
                if (surface.CasterId == hero.Id)
                {
                    if (--surface.LifeTime <= 0)
                    {
                        Hex? hex = GameData._hexes.FirstOrDefault(x => x.ID == surface.HexId);
                        if (hex != null)
                            hex.RemoveSurface(surface);
                        AttackService.ContinuousAuraAction();
                        removeSurf.Add(surface);
                    }
                }
            }
            // Удалем то что уже протухло
            foreach (var surface in removeSurf)
                GameData._surfaces.Remove(surface);

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
                hero.EffectList = hero.EffectList.OrderBy(x => x.type).ToList();
                foreach (var effect in hero.EffectList)
                {
                    if (effect.effectType == Consts.EffectType.EndTurn)
                        effect.ApplyEffect(hero);
                }
            }
        }

        public void StartTurnStatusApplyEffect(Hero hero)
        {
            if (hero.HP > 0)
            {
                hero.EffectList = hero.EffectList.OrderBy(x => x.type).ToList();
                foreach (var effect in hero.EffectList)
                {
                    if (effect.effectType == Consts.EffectType.StartTurn)
                        effect.ApplyEffect(hero);
                }
            }
        }

        public void AddUpgradePoints(int turn)
        {
            if (turn % 4 == 1)
                foreach (var hero in GameData._heroes)
                    hero.UpgradePoints++;
        }
    }
}
