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

        public TimingService()
        {
            GameData.activeTeam = "red";
            GameData.idActiveHero = 0;
        }

        public int GetActiveHero()
        {
            Hero? activeHero = GameData._heroes.FirstOrDefault(x => x.Id == GameData.idActiveHero && x.HP > 0 && x.RespawnTime < 1);
            if (activeHero == null)
            {
                activeHero = GameData._heroes.FirstOrDefault(x => x.Team == GameData.activeTeam && x.HP > 0 && x.RespawnTime < 1);
                if(activeHero != null)
                    GameData.idActiveHero = activeHero.Id;
                else
                    GameData.idActiveHero = GameData._heroes.FirstOrDefault(x => x.HP > 0)?.Id ?? -1;
            }
            return GameData.idActiveHero;
        }

        public int EndTurn()
        {
            try
            {
                try
                {
                    // Сначала применим все эффекты в конце хода каждого героя команды
                    foreach (var hero in GameData._heroes.Where(x => x.Team == GameData.activeTeam).OrderBy(y => y.Id))
                    {
                        AttackService.EndTurnAuraAction(hero);
                        EndTurnStatusApplyEffect(hero);
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }

                try
                {
                    // Также пробегаемся по объектам, которые тоже могут иметь ауры
                    foreach (var hero in GameData._solidObstacles.Where(x => x.Team == GameData.activeTeam))
                    {
                        AttackService.EndTurnAuraAction(hero);
                        EndTurnStatusApplyEffect(hero);
                    }
                }
                catch (Exception ex) { Console.WriteLine(ex.Message); }

                // Затем начнем уменьшать кд, срок действия эффекта и время жизни созданных объектов
                foreach (var hero in GameData._heroes.Where(x => x.Team == GameData.activeTeam))
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
                DecreaseLifeTimeSummons(GameData.activeTeam);

                AddUpgradePoints(++turn);
                GameData.turn = turn;
                if (GameData.activeTeam == "blue")
                {
                    GameData.activeTeam = "red";
                    GameData.userTeamBindings.ActiveTeam = GameData.userTeamBindings.RedTeam;
                    GameData.userTeamBindings.BlueCoins += 10;
                }
                else
                {
                    GameData.activeTeam = "blue";
                    GameData.userTeamBindings.ActiveTeam = GameData.userTeamBindings.BlueTeam;
                    GameData.userTeamBindings.RedCoins += 10;
                }

                foreach (var hero in GameData._heroes.Where(x => x.Team == GameData.activeTeam && x.HP > 0))
                {
                    RefreshStartTurnAbilities(hero);
                    GameData.idActiveHero = hero.Id;
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
            foreach (var hero in GameData._heroes.Where(x => x.Team == GameData.activeTeam))
            {
                DecreaseStatusDuration(hero.Id, Consts.EffectDurationType.StartTurn);
                DecreaseLifeTimeObstacle(hero, Consts.ObstacleLifeTimeDecrease.StartTurn);
                AttackService.StartTurnAuraAction(hero);
                StartTurnStatusApplyEffect(hero);
            }
            // Также пробегаемся по объектам, которые тоже могут иметь ауры
            foreach (var hero in GameData._solidObstacles.Where(x => x.Team == GameData.activeTeam))
            {
                AttackService.StartTurnAuraAction(hero);
                StartTurnStatusApplyEffect(hero);
            }

            return GameData.idActiveHero;
        }

        /// <summary>
        /// Уменьшение времени респавна героев. Триггерится в начале хода
        /// </summary>
        public void DecreaseRespawnTime()
        {
            foreach (var hero in GameData._heroes)
            {
                if (hero.Team == GameData.activeTeam && hero.RespawnTime > 1)
                {
                    if (--hero.RespawnTime <= 1)
                    {
                        hero.Respawn();
                    }
                }
            }
        }

        public void AddVPFromAreaControl()
        {
            foreach (var hex in GameData._hexes)
            {
                if (hex.VP > 0 && hex.HERO != null && hex.HERO.Team != GameData.activeTeam && hex.HERO.IsMainHero)
                {
                    if (hex.HERO.Team == "red")
                        GameData.userTeamBindings.RedVP += hex.VP;
                    else
                        GameData.userTeamBindings.BlueVP += hex.VP;
                }
            }
        }

        public void RefreshStartTurnAbilities(Hero hero)
        {
            if (hero.SkillList[0] is PassiveSkill)
                (hero.SkillList[0] as PassiveSkill).refreshEffect();
        }

        public void DecreaseStatusDuration(int heroId, Consts.EffectDurationType durationType)
        {
            foreach (var hero in GameData._heroes)
            {
                List<Effect> removeEffects = new List<Effect>();
                foreach (var effect in hero.EffectList.FindAll(x => x.durationType == durationType))
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
                foreach (var obst in GameData._obstacles)
                {
                    if (obst.CasterId == hero.Id)
                    {
                        if (--obst.LifeTime <= 0)
                        {
                            Hex? hex = GameData._hexes.FirstOrDefault(x => x.ID == obst.HexId);
                            if (hex != null)
                            {
                                removeHexObst.Add(obst);
                                hex.RemoveObstacle();
                            }
                            obst.HexId = -1;
                            AttackService.ContinuousAuraAction();
                        }
                    }
                }
                // Удалем то что уже протухло
                foreach (var obst in removeHexObst)
                    GameData._obstacles.Remove(obst);

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
            }

            // Теперь по блокирующим перемещение
            List<SolidObstacle> removeObst = new List<SolidObstacle>();
            foreach (var obst in GameData._solidObstacles)
            {
                if (obst.casterId == hero.Id && obst.obstacleLifeTimeDecrease == obstacleLifeTime)
                {
                    if (--obst.lifeTime <= 0)
                    {
                        Hex? hex = GameData._hexes.FirstOrDefault(x => x.ID == obst.HexId);
                        if (hex != null)
                        {
                            hex.RemoveHero();
                            obst.endLifeEffect(hex);
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
                List<Effect> appliedEffects = new List<Effect>();

                hero.EffectList = hero.EffectList.OrderBy(x => x.type).ToList();
                Effect[] effects = hero.EffectList.ToArray();
                for (int i = 0; i < effects.Count(); i++)
                {
                    int effectsCount = effects.Count();
                    if (effects[i].effectType == Consts.EffectType.EndTurn && !appliedEffects.Contains(effects[i])) // Дважды один эффект не отрабатываем
                    {
                        appliedEffects.Add(effects[i]);
                        effects[i].ApplyEffect(hero);
                    }

                    if (effectsCount != effects.Count()) // Если из коллекции удалили элемент, пробегаемся по новой
                        i = 0;
                }
            }
            if (hero.HP <= 0)
            {
                foreach (var effect in hero.EffectList)
                    effect.RemoveEffect(hero);
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
                        effects[i].ApplyEffect(hero);
                    }

                    if (effectsCount != effects.Count()) // Если из коллекции удалили элемент, пробегаемся по новой
                        i = 0;
                }
            }
        }

        public void AddUpgradePoints(int turn)
        {
            if (turn % 8 == 1)
                foreach (var hero in GameData._heroes)
                    if (hero is not Summon)
                        hero.UpgradePoints++;
        }
    }
}
