using BattleArenaServer.Effects;
using BattleArenaServer.Interfaces;
using BattleArenaServer.Models;
using BattleArenaServer.Models.Obstacles;
using BattleArenaServer.Models.Summons;

namespace BattleArenaServer.Services
{
    public class TimingService : ITiming
    {
        private int turn = 1;
        GameData gameData;
        public TimingService(GameData gameData)
        {
            this.gameData = gameData;
            gameData.activeTeam = "red";
            gameData.idActiveHero = 0;
        }

        public int GetActiveHero()
        {
            Hero? activeHero = gameData._heroes.FirstOrDefault(x => x.Id == gameData.idActiveHero && x.Team == gameData.activeTeam && x.HP > 0 && x.RespawnTime < 1);
            if (activeHero == null)
            {
                activeHero = gameData._heroes.FirstOrDefault(x => x.Team == gameData.activeTeam && x.HP > 0 && x.RespawnTime < 1);
                if(activeHero != null)
                    gameData.idActiveHero = activeHero.Id;
                else
                    gameData.idActiveHero = /*gameData._heroes.FirstOrDefault(x => x.HP > 0)?.Id ??*/ -1;
            }
            return gameData.idActiveHero;
        }

        public int EndTurn()
        {
            try
            {
                try
                {
                    // Сначала применим все эффекты в конце хода каждого героя команды
                    foreach (var hero in gameData._heroes.Where(x => x.Team == gameData.activeTeam).OrderBy(y => y.Id))
                    {
                        AttackService.EndTurnAuraAction(hero, gameData);
                        EndTurnStatusApplyEffect(hero);
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }

                try
                {
                    // Также пробегаемся по объектам, которые тоже могут иметь ауры
                    foreach (var hero in gameData._solidObstacles.Where(x => x.Team == gameData.activeTeam))
                    {
                        AttackService.EndTurnAuraAction(hero, gameData);
                        EndTurnStatusApplyEffect(hero);
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }

                // Затем начнем уменьшать кд, срок действия эффекта и время жизни созданных объектов
                foreach (var hero in gameData._heroes.Where(x => x.Team == gameData.activeTeam))
                {
                    hero.AP = 4;

                    DecreaseSkillCooldawn(hero);
                    DecreaseItemCooldawn(hero);
                    DecreaseLifeTimeObstacle(hero, Consts.ObstacleLifeTimeDecrease.EndTurn);
                    DecreaseStatusDuration(hero.Id, Consts.EffectDurationType.EndTurn);
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            try
            {
                // Поскольку саммоны находятся в Героях, то их "протухание" делаем отдельно вне цикла
                DecreaseLifeTimeSummons(gameData.activeTeam);

                AddUpgradePoints(++turn);
                gameData.turn = turn;
                if (gameData.activeTeam == "blue")
                {
                    gameData.activeTeam = "red";
                    gameData.userTeamBindings.ActiveTeam = gameData.userTeamBindings.RedTeam;
                    gameData.userTeamBindings.BlueCoins += 10;
                }
                else
                {
                    gameData.activeTeam = "blue";
                    gameData.userTeamBindings.ActiveTeam = gameData.userTeamBindings.BlueTeam;
                    gameData.userTeamBindings.RedCoins += 10;
                }

                foreach (var hero in gameData._heroes.Where(x => x.Team == gameData.activeTeam && x.HP > 0))
                {
                    RefreshStartTurnAbilities(hero);
                    gameData.idActiveHero = hero.Id;
                }
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }

            try
            {
                DecreaseRespawnTime();
                AddVPFromAreaControl();
            }
            catch (Exception ex) { Console.WriteLine(ex.Message); }


            // Сначала применим все эффекты в начале хода каждого героя команды
            foreach (var hero in gameData._heroes.Where(x => x.Team == gameData.activeTeam))
            {
                DecreaseStatusDuration(hero.Id, Consts.EffectDurationType.StartTurn);
                DecreaseLifeTimeObstacle(hero, Consts.ObstacleLifeTimeDecrease.StartTurn);
                AttackService.StartTurnAuraAction(hero, gameData);
                StartTurnStatusApplyEffect(hero);
            }
            // Также пробегаемся по объектам, которые тоже могут иметь ауры
            foreach (var hero in gameData._solidObstacles.Where(x => x.Team == gameData.activeTeam))
            {
                AttackService.StartTurnAuraAction(hero, gameData);
                StartTurnStatusApplyEffect(hero);
            }

            return gameData.idActiveHero;
        }

        /// <summary>
        /// Уменьшение времени респавна героев. Триггерится в начале хода
        /// </summary>
        public void DecreaseRespawnTime()
        {
            foreach (var hero in gameData._heroes)
            {
                if (hero.Team == gameData.activeTeam && hero.RespawnTime > 1)
                {
                    if (--hero.RespawnTime <= 1)
                    {
                        //hero.Respawn(gameData);
                    }
                }
            }
        }

        public void AddVPFromAreaControl()
        {
            foreach (var hex in gameData._hexes)
            {
                if (hex.VP > 0 && hex.HERO != null && hex.HERO.Team != gameData.activeTeam && hex.HERO.IsMainHero)
                {
                    if (hex.HERO.Team == "red")
                        gameData.userTeamBindings.RedVP += hex.VP;
                    else
                        gameData.userTeamBindings.BlueVP += hex.VP;
                }
            }
            gameData.userTeamBindings.CheckWin();
        }

        public void RefreshStartTurnAbilities(Hero hero)
        {
            if (hero.SkillList[0] is PassiveSkill)
                (hero.SkillList[0] as PassiveSkill).refreshEffect(gameData);
        }

        public void DecreaseStatusDuration(int heroId, Consts.EffectDurationType durationType)
        {
            foreach (var hero in gameData._heroes)
            {
                List<Effect> removeEffects = new List<Effect>();
                foreach (var effect in hero.EffectList.FindAll(x => x.durationType == durationType))
                {
                    if (effect.idCaster == heroId && effect.duration > 1)
                        effect.duration--;
                    else if (effect.idCaster == heroId && effect.duration <= 1)
                    {
                        effect.RemoveEffect(hero, gameData);
                        removeEffects.Add(effect);
                    }
                }
                foreach (var effect in removeEffects)
                {
                    hero.EffectList.Remove(effect);
                    effect.ApplyAfterEffect(hero, gameData);
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

        public void DecreaseItemCooldawn(Hero hero)
        {
            foreach (var item in hero.Items)
            {
                if (item.Skill.coolDownNow > 0)
                    item.Skill.coolDownNow--;
            }
        }

        public void DecreaseLifeTimeObstacle(Hero hero, Consts.ObstacleLifeTimeDecrease obstacleLifeTime)
        {
            if (obstacleLifeTime == Consts.ObstacleLifeTimeDecrease.EndTurn)
            {
                // Пробегаемся по обычным преградам
                List<Obstacle> removeHexObst = new List<Obstacle>();
                foreach (var obst in gameData._obstacles)
                {
                    if (obst.CasterId == hero.Id)
                    {
                        if (--obst.LifeTime <= 0)
                        {
                            Hex? hex = gameData._hexes.FirstOrDefault(x => x.ID == obst.HexId);
                            if (hex != null)
                            {
                                removeHexObst.Add(obst);
                                hex.RemoveObstacle();
                            }
                            obst.HexId = -1;
                            AttackService.ContinuousAuraAction(gameData);
                        }
                    }
                }
                // Удалем то что уже протухло
                foreach (var obst in removeHexObst)
                    gameData._obstacles.Remove(obst);

                // Также по поверхностям
                List<FillableObstacle> removeSurf = new List<FillableObstacle>();
                foreach (var surface in gameData._surfaces)
                {
                    if (surface.CasterId == hero.Id)
                    {
                        if (--surface.LifeTime <= 0)
                        {
                            Hex? hex = gameData._hexes.FirstOrDefault(x => x.ID == surface.HexId);
                            if (hex != null)
                                hex.RemoveSurface(surface);
                            surface.HexId = -1;
                            AttackService.ContinuousAuraAction(gameData);
                            removeSurf.Add(surface);
                        }
                    }
                }
                // Удалем то что уже протухло
                foreach (var surface in removeSurf)
                    gameData._surfaces.Remove(surface);
            }

            // Теперь по блокирующим перемещение
            List<SolidObstacle> removeObst = new List<SolidObstacle>();
            foreach (var obst in gameData._solidObstacles)
            {
                if (obst.casterId == hero.Id && obst.obstacleLifeTimeDecrease == obstacleLifeTime)
                {
                    if (--obst.lifeTime <= 0)
                    {
                        Hex? hex = gameData._hexes.FirstOrDefault(x => x.ID == obst.HexId);
                        if (hex != null)
                        {
                            hex.RemoveHero();
                            obst.endLifeEffect(hex, gameData);
                        }
                        obst.HexId = -1;

                        AttackService.ContinuousAuraAction(gameData);
                        removeObst.Add(obst);
                    }
                }
            }
            // Удалем то что уже протухло
            foreach (var obst in removeObst)
                gameData._solidObstacles.Remove(obst);
        }

        public void DecreaseLifeTimeSummons(string team)
        {
            // Также не забываем про саммонов
            List<Summon> summons = new List<Summon>();
            foreach (var summon in gameData._heroes)
            {
                if (summon is Summon)
                {
                    if (summon.Team == team && --(summon as Summon).lifeTime <= 0)
                    {
                        Hex? hex = gameData._hexes.FirstOrDefault(x => x.ID == summon.HexId);
                        if (hex != null)
                            hex.RemoveHero();

                        AttackService.ContinuousAuraAction(gameData);
                        summons.Add(summon as Summon);
                    }
                }
            }
            // Удалем то что уже протухло
            foreach (var summon in summons)
                gameData._heroes.Remove(summon);
        }

        public void EndTurnStatusApplyEffect(Hero hero)
        {
            if (hero.HP > 0)
            {
                List<Effect> appliedEffects = new List<Effect>();

                hero.EffectList = hero.EffectList.OrderBy(x => x.type).ToList();
                Effect[] effects = hero.EffectList.ToArray();
                for (int i = 0; i < effects.Count(); i++)
                {
                    int effectsCount = effects.Count();
                    if (effects[i].effectType == Consts.EffectType.EndTurn && !appliedEffects.Contains(effects[i])) // Дважды один эффект не отрабатываем
                    {
                        appliedEffects.Add(effects[i]);
                        effects[i].ApplyEffect(hero, gameData);
                    }

                    if (effectsCount != effects.Count()) // Если из коллекции удалили элемент, пробегаемся по новой
                        i = 0;
                }
            }
            if (hero.HP <= 0)
            {
                foreach (var effect in hero.EffectList)
                    effect.RemoveEffect(hero, gameData);
                hero.EffectList.Clear();
            }
        }

        public void StartTurnStatusApplyEffect(Hero hero)
        {
            if (hero.HP > 0)
            {
                List<Effect> appliedEffects = new List<Effect>();

                hero.EffectList = hero.EffectList.OrderBy(x => x.type).ToList();
                Effect[] effects = hero.EffectList.ToArray();
                for (int i = 0; i < effects.Count(); i++)
                {
                    int effectsCount = effects.Count();
                    if (effects[i].effectType == Consts.EffectType.StartTurn && !appliedEffects.Contains(effects[i])) // Дважды один эффект не отрабатываем
                    {
                        appliedEffects.Add(effects[i]);
                        effects[i].ApplyEffect(hero, gameData);
                    }

                    if (effectsCount != effects.Count()) // Если из коллекции удалили элемент, пробегаемся по новой
                        i = 0;
                }
            }
        }

        public void AddUpgradePoints(int turn)
        {
            if (turn % 8 == 1)
                foreach (var hero in gameData._heroes)
                    if (hero.IsMainHero)
                        hero.UpgradePoints++;
        }
    }
}
