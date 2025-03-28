﻿using BattleArenaServer.Models;
using BattleArenaServer.Models.Heroes;
using BattleArenaServer.Models.Obstacles;
using BattleArenaServer.Services;
using BattleArenaServer.Skills.WitchDoctorSkills;
using Microsoft.AspNetCore.SignalR;
using System;

namespace BattleArenaServer.Hubs
{
    public class FieldHub : Hub
    {
        TimingService timingService = new TimingService();
        HttpContext httpContext;
        public FieldHub()
        {
            CreateGameField();
            CreateHeroList();
        }

        private static void CreateHeroList()
        {
            SetRandomCommands();
            // Применяем эффекты постоянных аур сразу
            AttackService.ContinuousAuraAction();

            // Применим уникальные эффекты после сбора команд и до начала боя
            ApplyUniqueEffectsBeforeFight();
        }

        private static void SetRandomCommands()
        {
            List<Hero> heroes = new List<Hero>();
            List<int> redCoords = [7, 22, 37];
            List<int> blueCoords = [14, 29, 44];

            heroes.Add(new WitchDoctorHero(0, ""));
            heroes.Add(new AssassinHero(0, ""));
            heroes.Add(new KnightHero(0, ""));
            heroes.Add(new ArcherHero(0, ""));
            heroes.Add(new BerserkerHero(0, ""));
            heroes.Add(new PriestHero(0, ""));
            heroes.Add(new AeroturgHero(0, ""));
            heroes.Add(new GeomantHero(0, ""));
            heroes.Add(new AbominationHero(0, ""));
            heroes.Add(new ChaosHero(0, ""));
            heroes.Add(new ElementalistHero(0, ""));
            heroes.Add(new CultistHero(0, ""));
            heroes.Add(new NecromancerHero(0, ""));
            heroes.Add(new ShadowHero(0, ""));
            heroes.Add(new TinkerHero(0, ""));
            heroes.Add(new GuardianHero(0, ""));
            heroes.Add(new FairyHero(0, ""));
            heroes.Add(new InvokerHero(0, ""));

            string team = "red";
            Random rnd = new Random();
            int rndCoords = 0;

            for (int i = 0; i < 6; i++)
            {
                Hero hero = GetRandomHero(heroes);
                heroes.Remove(hero);
                hero.Id = i;
                hero.Team = team;

                if (team == "red")
                {
                    rndCoords = rnd.Next(redCoords.Count);
                    hero.HexId = redCoords[rndCoords];
                    redCoords.Remove(redCoords[rndCoords]);
                    team = "blue";
                }
                else
                {
                    rndCoords = rnd.Next(blueCoords.Count);
                    hero.HexId = blueCoords[rndCoords];
                    blueCoords.Remove(blueCoords[rndCoords]);
                    team = "red";
                }
                GameData._hexes[hero.HexId].SetHero(hero);
                GameData._heroes.Add(hero);
            }
        }

        private static Hero GetRandomHero(List<Hero> heroes)
        {
            Random rnd = new Random();
            Hero hero = heroes[rnd.Next(0, heroes.Count)];
            heroes.Remove(hero);

            return hero;
        }

        private static void ApplyUniqueEffectsBeforeFight()
        {
            // Witch Doctor
            WitchDoctorHero? wdh = (WitchDoctorHero)GameData._heroes.FirstOrDefault(x => x is WitchDoctorHero);
            if (wdh != null)
                (wdh.SkillList[0] as PerfectHealthPSkill).ApplyModifier();
        }

        private void CreateGameField()
        {
            // Создание поля
            int[] _start = [-1, -3, 4];
            for (int i = 0; i < 52; i++)
            {
                Hex hex = new Hex(_start[0], _start[1], _start[2], i);
                GameData._hexes.Add(hex);
                switch (i)
                {
                    case 6: _start = [-2, -2, 4]; break;
                    case 14: _start = [-2, -1, 3]; break;
                    case 21: _start = [-3, 0, 3]; break;
                    case 29: _start = [-3, 1, 2]; break;
                    case 36: _start = [-4, 2, 2]; break;
                    case 44: _start = [-4, 3, 1]; break;
                    default:
                        {
                            _start[0] += Consts.directions[1].COORD[0];
                            _start[1] += Consts.directions[1].COORD[1];
                            _start[2] += Consts.directions[1].COORD[2];
                            break;
                        }
                }
            }
        }














        public async Task RecreateGame()
        {
            try
            {
                GameData.ClearAllObjects();
                GameData.activeTeam = "red";
                CreateGameField();
                CreateHeroList();
                timingService = new TimingService();
                GameData.userTeamBindings.ClearBindings();
                await SetActiveHero(GameData.idActiveHero);
                await GetUsersBindings();

                await this.Clients.All.SendAsync("GetField", GameData._hexes);
            }
            catch
            {
                Console.WriteLine("Error in RecreateGame()");
            }
        }

        public async Task GetField()
        {
            try { await this.Clients.All.SendAsync("GetField", GameData._hexes); }
            catch { Console.WriteLine("Error in GetField()"); }
        }

        public async Task UpgradeSkill(int cur_pos, int skill)
        {
            Hero? hero = GameData._hexes.FirstOrDefault(x => x.ID == cur_pos)?.HERO;
            if (hero != null)
            {
                // Пытаемся прокачать скилл
                if (hero.SkillList[skill].UpgradeSkill())
                {
                    hero.UpgradePoints--;
                    try { await this.Clients.All.SendAsync("GetField", GameData._hexes); }
                    catch { Console.WriteLine("Error in UpgradeSkill()"); }
                }
            }
        }

        public async Task StepHero(int cur_pos, int targer_pos)
        {
            RequestData requestData = new RequestData(cur_pos, targer_pos);
            if (requestData.Caster != null)
            {
                if (requestData.Target == null && requestData.TargetHex != null && requestData.TargetHex.OBSTACLE != null
                    && requestData.TargetHex.OBSTACLE.Name == "AstralPortal")
                    AttackService.MoveHero(requestData.Caster, requestData.CasterHex, requestData.TargetHex);
                else
                    requestData.Caster.MoveSkill.Cast(requestData);
            }

            try { await this.Clients.All.SendAsync("GetField", GameData._hexes); }
            catch { Console.WriteLine("Error in StepHero()"); }
        }

        public async Task SpellCast(int targer_pos, int cur_pos, int skill)
        {
            RequestData requestData = new RequestData(cur_pos, targer_pos);
            if (requestData.Caster != null && requestData.Caster.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.Silence)) == null)
            {
                requestData.Caster.beforeSpellCast(requestData.Caster, requestData.Target, requestData.Caster.SkillList[skill]);
                // Кастуем))
                requestData.Caster.SkillList[skill].Cast(requestData);
                try { await this.Clients.All.SendAsync("GetField", GameData._hexes); }
                catch { Console.WriteLine("Error in SpellCast()"); }
            }
        }

        public async Task AttackHero(int cur_pos, int targer_pos)
        {
            RequestData requestData = new RequestData(cur_pos, targer_pos);

            if (requestData.CasterHex != null && requestData.TargetHex != null && requestData.Caster != null && requestData.Target != null &&
                requestData.Target.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.NonTargetable)) == null && // Цель должна быть доступна для выбора
                requestData.Caster.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.Disarm)) == null) // Атакующий не должен быть обезоружен
            {
                Hero attacker = requestData.Caster;
                Hero defender = requestData.Target;

                int range = attacker.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.Blind)) == null ? attacker.AttackRadius + attacker.StatsEffect.AttackRadius : 1;

                if (requestData.CasterHex.Distance(requestData.TargetHex) > range || attacker.AP < attacker.APtoAttack)
                    return;

                attacker.AP -= attacker.APtoAttack;
                // К урону добавляем дополнительный от пассивок и эффектов
                int dmg = (int)((attacker.Dmg + attacker.GetPassiveAttackDamage(attacker, defender) + attacker.StatsEffect.Dmg) * attacker.StatsEffect.DmgMultiplier);

                // Эффекты перед атакой
                attacker.beforeAttack(attacker, defender, dmg);
                defender.beforeReceivedAttack(attacker, defender, dmg);
                // Сама атака с нанесением урона
                AttackService.SetDamage(attacker, defender, dmg, Consts.DamageType.Physical);
                // Эффекты после атаки
                attacker.afterAttack(attacker, defender, dmg);
                defender.afterReceivedAttack(attacker, defender, dmg);
            }

            try { await this.Clients.All.SendAsync("GetField", GameData._hexes); }
            catch { Console.WriteLine("Error in AttackHero()"); }
        }

        public async Task SendSpellArea(int target, int caster, int spell)
        {
            try { await this.Clients.All.SendAsync("DrawSpellArea", GetSpellArea(target, caster, spell)); }
            catch { Console.WriteLine("Error in SendSpellArea()"); }
        }

        public async Task SetActiveHero(int idActiveHero)
        {
            Hero? hero = GameData._heroes.FirstOrDefault(x => x.Id == idActiveHero);
            if (hero != null && hero is not SolidObstacle)
                GameData.idActiveHero = idActiveHero;

            try { await this.Clients.All.SendAsync("GetActiveHero", GameData.idActiveHero); }
            catch { Console.WriteLine("Error in SetActiveHero()"); }
        }

        public async Task SendActiveHero()
        {
            try { await this.Clients.All.SendAsync("GetActiveHero", timingService.GetActiveHero()); }
            catch { Console.WriteLine("Error in SendActiveHero()"); }
        }

        public async void EndTurn()
        {
            try
            {
                timingService.EndTurn();
                await GetUsersBindings();
                await this.Clients.All.SendAsync("GetField", GameData._hexes);
            }
            catch { Console.WriteLine("Error in EndTurn()"); }
        }

        public async Task BindingUserToTeam(string userId, string team)
        {
            if (team == "red")
                GameData.userTeamBindings.RedTeam = userId;
            else
                GameData.userTeamBindings.BlueTeam = userId;

            if (GameData.userTeamBindings.RedTeam != "" && GameData.userTeamBindings.BlueTeam != "") // Выбраны обе команды, можно начинать сражение
                GameData.userTeamBindings.ActiveTeam = GameData.userTeamBindings.RedTeam;

            await GetUsersBindings();
        }

        public async Task GetUsersBindings()
        {
            try {
                await this.Clients.All.SendAsync("GetUsersBindings", GameData.userTeamBindings);
            }
            catch { Console.WriteLine("Error in SendActiveHero()"); }
        }

        private int[] GetSpellArea(int target, int caster, int spell)
        {
            Hero? casterHero = GameData._hexes[caster].HERO;
            Skill skill = casterHero.SkillList[spell];
            List<int> spellArea = new List<int>();

            int spellRange = skill.range;
            if (casterHero.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.Blind)) != null && spellRange > 1)
                spellRange = 1;

            int dist = GameData._hexes[target].Distance(GameData._hexes[caster]);

            switch (skill.area)
            {
                case Consts.SpellArea.AllyTarget:
                    if (dist <= spellRange)
                    {
                        if (GameData._hexes[target].HERO != null && GameData._hexes[target].HERO.Team == casterHero.Team)
                            spellArea.Add(target);
                    }
                    break;
                case Consts.SpellArea.EnemyTarget:
                    if (dist <= spellRange)
                    {
                        if (GameData._hexes[target].HERO != null && GameData._hexes[target].HERO.Team != casterHero.Team &&
                            GameData._hexes[target].HERO.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.NonTargetable)) == null)
                            spellArea.Add(target);
                    }
                    break;
                case Consts.SpellArea.FriendTarget:
                    if (dist <= spellRange && dist > 0)
                    {
                        if (GameData._hexes[target].HERO != null && GameData._hexes[target].HERO.Team == casterHero.Team)
                            spellArea.Add(target);
                    }
                    break;
                case Consts.SpellArea.Radius:
                    if (dist <= spellRange)
                    {
                        foreach (var hex in UtilityService.GetHexesRadius(GameData._hexes[target], skill.radius))
                            spellArea.Add(hex.ID);
                    }
                    break;
                case Consts.SpellArea.Line:
                    foreach (var hex in UtilityService.GetHexesOneLine(GameData._hexes[caster], GameData._hexes[target], skill.radius))
                        spellArea.Add(hex.ID);
                    break;
                case Consts.SpellArea.HeroTarget:
                    if (dist <= spellRange)
                    {
                        if (GameData._hexes[target].HERO != null &&
                            (GameData._hexes[target].HERO.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.NonTargetable)) == null || GameData._hexes[target].HERO.Team == casterHero.Team))
                            spellArea.Add(target);
                    }
                    break;
                case Consts.SpellArea.HeroNotSelfTarget:
                    if (dist <= spellRange)
                    {
                        if (GameData._hexes[target].HERO != null && GameData._hexes[target].HERO.Id != casterHero.Id &&
                            (GameData._hexes[target].HERO.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.NonTargetable)) == null || GameData._hexes[target].HERO.Team == casterHero.Team))
                            spellArea.Add(target);
                    }
                    break;
                case Consts.SpellArea.Conus:
                    if (dist <= spellRange)
                    {
                        foreach (var hex in UtilityService.GetHexesCone(GameData._hexes[caster], GameData._hexes[target], skill.radius))
                            spellArea.Add(hex.ID);
                    }
                    break;
                case Consts.SpellArea.SmallConus:
                    if (dist <= spellRange)
                    {
                        foreach (var hex in UtilityService.GetHexesSmallCone(GameData._hexes[caster], GameData._hexes[target], skill.radius))
                            spellArea.Add(hex.ID);
                    }
                    break;
                case Consts.SpellArea.WideLine:
                    if (dist <= spellRange)
                    {
                        foreach (var hex in UtilityService.GetHexesWideLine(GameData._hexes[caster], GameData._hexes[target], skill.radius))
                            spellArea.Add(hex.ID);
                    }
                    break;
            }

            return spellArea.ToArray();
        }
    }
}
