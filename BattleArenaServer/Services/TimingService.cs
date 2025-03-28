﻿using BattleArenaServer.Effects;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Models.Obstacles;
using BattleArenaServer.Models.Summons;

namespace BattleArenaServer.Services
{
    public class TimingService : ITiming
    {
        private int turn = 1;
        private bool isProcessed = true;

        public TimingService()
        {
            GameData.activeTeam = "red";
            GameData.idActiveHero = 0;
        }

        public int GetActiveHero()
        {
            Hero? activeHero = GameData._heroes.FirstOrDefault(x => x.Id == GameData.idActiveHero && x.HP > 0);
            if (activeHero == null)
            {
                activeHero = GameData._heroes.FirstOrDefault(x => x.Team == GameData.activeTeam && x.HP > 0);
                if(activeHero != null)
                    GameData.idActiveHero = activeHero.Id;
                else
                    GameData.idActiveHero = GameData._heroes.FirstOrDefault(x => x.HP > 0).Id;
            }
            return GameData.idActiveHero;
        }

        public int EndTurn()
        {
            if (!isProcessed)
                return GameData.idActiveHero;

            isProcessed = false;
            // Сначала применим все эффекты в конце хода каждого героя команды
            foreach (var hero in GameData._heroes.Where(x => x.Team == GameData.activeTeam))
            {
                AttackService.EndTurnAuraAction(hero);
                EndTurnStatusApplyEffect(hero);
            }
            // Также пробегаемся по объектам, которые тоже могут иметь ауры
            foreach (var hero in GameData._solidObstacles.Where(x => x.Team == GameData.activeTeam))
            {
                AttackService.EndTurnAuraAction(hero);
                EndTurnStatusApplyEffect(hero);
            }

            // Затем начнем уменьшать кд, срок действия эффекта и время жизни созданных объектов
            foreach (var hero in GameData._heroes.Where(x => x.Team == GameData.activeTeam))
            {
                hero.AP = 4;
                DecreaseSkillCooldawn(hero);
                DecreaseLifeTimeObstacle(hero);

                DecreaseStatusDuration(hero.Id);
            }

            // Поскольку саммоны находятся в Героях, то их "протухание" делаем отдельно вне цикла
            DecreaseLifeTimeSummons(GameData.activeTeam);

            AddUpgradePoints(++turn);
            if (GameData.activeTeam == "blue")
            {
                GameData.activeTeam = "red";
                GameData.userTeamBindings.ActiveTeam = GameData.userTeamBindings.RedTeam;
            }
            else
            {
                GameData.activeTeam = "blue";
                GameData.userTeamBindings.ActiveTeam = GameData.userTeamBindings.BlueTeam;
            }

            foreach (var hero in GameData._heroes.Where(x => x.Team == GameData.activeTeam && x.HP > 0))
            {
                RefreshStartTurnAbilities(hero);
                GameData.idActiveHero = hero.Id;
            }
            isProcessed = true;
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
                    else if (effect.idCaster == heroId && effect.duration <= 1)
                    {
                        effect.RemoveEffect(hero);
                        removeEffects.Add(effect);
                    }
                }
                foreach (var effect in removeEffects)
                {
                    hero.EffectList.Remove(effect);
                    effect.ApplyAfterEffect(hero);
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
                        obst.HexId = -1;
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
                        surface.HexId = -1;
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
                        {
                            obst.endLifeEffect(hex);
                            hex.RemoveHero();
                        }
                        obst.HexId = -1;

                        AttackService.ContinuousAuraAction();
                        removeObst.Add(obst);
                    }
                }
            }
            // Удалем то что уже протухло
            foreach (var obst in removeObst)
                GameData._solidObstacles.Remove(obst);
        }

        public void DecreaseLifeTimeSummons(string team)
        {
            // Также не забываем про саммонов
            List<Summon> summons = new List<Summon>();
            foreach (var summon in GameData._heroes)
            {
                if (summon is Summon)
                {
                    if (summon.Team == team && --(summon as Summon).lifeTime <= 0)
                    {
                        Hex? hex = GameData._hexes.FirstOrDefault(x => x.ID == summon.HexId);
                        if (hex != null)
                            hex.RemoveHero();

                        AttackService.ContinuousAuraAction();
                        summons.Add(summon as Summon);
                    }
                }
            }
            // Удалем то что уже протухло
            foreach (var summon in summons)
                GameData._heroes.Remove(summon);
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
                    if (hero is not Summon)
                        hero.UpgradePoints++;
        }
    }
}
