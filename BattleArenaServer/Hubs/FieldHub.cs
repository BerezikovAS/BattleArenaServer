using BattleArenaServer.Effects;
using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Models;
using BattleArenaServer.Models.Heroes;
using BattleArenaServer.Models.Obstacles;
using BattleArenaServer.Services;
using Microsoft.AspNetCore.SignalR;

namespace BattleArenaServer.Hubs
{
    public class FieldHub : Hub
    {
        TimingService timingService = new TimingService();
        PickService pickService = new PickService();

        public FieldHub()
        {
            CreateGameField();
            //CreateHeroList();
            ShopService.FillItems();
            pickService.FillHeroesList();
        }

        private static void CreateHeroList()
        {
            SetRandomCommands();
            // Применяем эффекты постоянных аур сразу
            AttackService.ContinuousAuraAction();
        }

        private static void SetRandomCommands()
        {
            List<Hero> heroes = new List<Hero>();
            List<int> redCoords = [7, 22, 37];
            List<int> blueCoords = [14, 29, 44];

            heroes.Add(new ChaosHero(0, ""));
            heroes.Add(new WitchDoctorHero(0, ""));
            heroes.Add(new AssassinHero(0, ""));
            heroes.Add(new KnightHero(0, ""));
            heroes.Add(new ArcherHero(0, ""));
            heroes.Add(new BerserkerHero(0, ""));
            heroes.Add(new PriestHero(0, ""));
            heroes.Add(new AeroturgHero(0, ""));
            heroes.Add(new GeomantHero(0, ""));
            heroes.Add(new ElementalistHero(0, ""));
            heroes.Add(new CultistHero(0, ""));
            heroes.Add(new NecromancerHero(0, ""));
            heroes.Add(new ShadowHero(0, ""));
            heroes.Add(new TinkerHero(0, ""));
            heroes.Add(new GuardianHero(0, ""));
            heroes.Add(new FairyHero(0, ""));
            heroes.Add(new InvokerHero(0, ""));
            heroes.Add(new SeraphimHero(0, ""));
            heroes.Add(new DruidHero(0, ""));
            heroes.Add(new GhostHero(0, ""));
            heroes.Add(new MusketeerHero(0, ""));
            heroes.Add(new AbominationHero(0, ""));
            heroes.Add(new DwarfHero(0, ""));
            heroes.Add(new FallenKingHero(0, ""));
            heroes.Add(new GolemHero(0, ""));
            heroes.Add(new PlagueDoctorHero(0, ""));
            heroes.Add(new VampireHero(0, ""));
            heroes.Add(new SnowQueenHero(0, ""));
            

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

        private void CreateGameField()
        {
            // Создание поля
            int[] _start = [-1, -3, 4];
            for (int i = 0; i < 52; i++)
            {
                //Устанавливаем победные очки за контроль гекса
                int vp = 0;
                switch (i)
                {
                    case 10: case 11: case 17: case 19: case 24: case 27: case 32: case 34: case 40: case 41: vp = 1; break;
                    case 18: case 25: case 26: case 33: vp = 2; break;
                    default: vp = 0; break;
                }

                //Устанавливаем точки респавна для каждой из команд
                string teamRespawn = "";
                switch (i)
                {
                    case 0: case 7: case 37: case 45: teamRespawn = "red"; break;
                    case 6: case 14: case 44: case 51: teamRespawn = "blue"; break;
                    default: teamRespawn = ""; break;
                }

                //Устанавливаем зоны покупок предметов для команд
                string teamShop = "";
                switch (i)
                {
                    case 0: case 1: case 7: case 8: case 15: case 22: case 30: case 37: case 38: case 45: case 46: teamShop = "red"; break;
                    case 5: case 6: case 13: case 14: case 21: case 29: case 36: case 43: case 44: case 50: case 51: teamShop = "blue"; break;
                    default: teamShop = ""; break;
                }

                Hex hex = new Hex(_start[0], _start[1], _start[2], i, vp, teamRespawn, teamShop);
                GameData._hexes.Add(hex);

                //Поскольку мы добавляем гексы слева направо, то при достижении определенных значений, нужно "спуститься" на новую строку
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
                //CreateHeroList();
                ShopService.FillItems();
                timingService = new TimingService();
                GameData.userTeamBindings.ClearBindings();
                pickService.FillHeroesList(); // Обновляется список героев для пика
                await SetActiveHero(GameData.idActiveHero);
                await GetUsersBindings();

                await this.Clients.All.SendAsync("RecreateGame");
                //await SendAllInfo();
            }
            catch
            {
                Console.WriteLine("Error in RecreateGame()");
            }
        }

        public async Task GetField()
        {
            try { 
                await SendAllInfo();
            }
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
                    try {
                        await SendAllInfo();
                    }
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

            try { 
                await SendAllInfo();
            }
            catch { Console.WriteLine("Error in StepHero()"); }
        }

        public async Task SpellCast(int targer_pos, int cur_pos, int skill)
        {
            RequestData requestData = new RequestData(cur_pos, targer_pos);
            if (requestData.Caster != null && requestData.Caster.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.Silence)) == null)
            {
                requestData.Caster.beforeSpellCast(requestData.Caster, requestData.Target, requestData.Caster.SkillList[skill]);
                // Кастуем))
                if (requestData.Caster.SkillList[skill].Cast(requestData))
                    requestData.Caster.afterSpellCast(requestData.Caster, requestData.Target, requestData.Caster.SkillList[skill]);

                try {
                    await SendAllInfo();
                }
                catch { Console.WriteLine("Error in SpellCast()"); }
            }
        }

        public async Task ItemCast(int targer_pos, int cur_pos, string itemName)
        {
            RequestData requestData = new RequestData(cur_pos, targer_pos);
            if (requestData.Caster != null)
            {
                Item? item = requestData.Caster.Items.FirstOrDefault(x => x.Name == itemName && x.ItemType == Consts.ItemType.Active);
                if (item != null)
                    item.Skill.Cast(requestData);
                try
                {
                    await SendAllInfo();
                }
                catch { Console.WriteLine("Error in SpellCast()"); }
            }
        }

        public async Task AttackHero(int cur_pos, int targer_pos)
        {
            RequestData requestData = new RequestData(cur_pos, targer_pos);

            AttackService.AttackHero(requestData);

            try {
                await SendAllInfo();
            }
            catch { Console.WriteLine("Error in AttackHero()"); }
        }

        public async Task SendSpellArea(int target, int caster, int spell, string itemName = "")
        {
            try { 
                await this.Clients.All.SendAsync("DrawSpellArea", GetSpellArea(target, caster, spell, itemName));
            }
            catch { Console.WriteLine("Error in SendSpellArea()"); }
        }

        public async Task SendRespawnArea(int heroId)
        {
            try { await this.Clients.All.SendAsync("DrawSpellArea", GetRespawnArea(heroId)); }
            catch { Console.WriteLine("Error in SendRespawnArea()"); }
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
            try { 
                await this.Clients.All.SendAsync("GetActiveHero", timingService.GetActiveHero());
                await GetShopItems();
            }
            catch { Console.WriteLine("Error in SendActiveHero()"); }
        }

        public async void EndTurn()
        {
            try
            {
                timingService.EndTurn();
                await GetUsersBindings();
                await SendAllInfo();
            }
            catch { Console.WriteLine("Error in EndTurn()"); }
        }

        public async Task SendAllInfo()
        {
            await this.Clients.All.SendAsync("GetField", GameData._hexes);
            await this.Clients.All.SendAsync("GetVP", GameData.userTeamBindings);
            await this.Clients.All.SendAsync("GetBanchHeroes", UtilityService.GetBanchHeroes());
            await GetShopItems();
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

        public async Task RespawnHero(int heroId, int hexId)
        {
            Hero? hero = GameData._heroes.FirstOrDefault(x => x.Id == heroId);
            Hex? hex = GameData._hexes.FirstOrDefault(x => x.ID == hexId && x.IsFree());

            if (hero != null && hex != null && hex.TeamRespawn == hero.Team)
            {
                hex.SetHero(hero);
                hero.HexId = hexId;
                hero.RespawnTime = 0;

                HasteBuff hasteBuff = new HasteBuff(hero.Id, 0, 1);
                hero.AddEffect(hasteBuff);
            }

            await SendAllInfo();
        }

        public async Task GetUsersBindings()
        {
            try {
                GameData.userTeamBindings.ActiveTeamStr = GameData.activeTeam;
                await this.Clients.All.SendAsync("GetUsersBindings", GameData.userTeamBindings);
            }
            catch { Console.WriteLine("Error in GetUsersBindings()"); }
        }

        public async Task GetShopItems()
        {
            try
            {
                await this.Clients.All.SendAsync("SetShopItems", ShopService.GetShopItems());
            }
            catch { Console.WriteLine("Error in GetShopItems()"); }
        }

        public async Task BuyItem(int idHero, string itemName)
        {
            try
            {
                Hero? hero = GameData._heroes.FirstOrDefault(h => h.Id == idHero && h.IsMainHero);
                if (hero == null)
                    return;

                Item? item;
                if (hero.Team == "red")
                    item = GameData._redShop.FirstOrDefault(x => x.Name == itemName);
                else
                    item = GameData._blueShop.FirstOrDefault(x => x.Name == itemName);

                if (item == null)
                    return;

                Hex? hex = GameData._hexes.FirstOrDefault(x => x.HERO != null && x.HERO.Id == idHero);
                if (hex == null || hex.TeamShop != hero.Team)
                    return;

                ShopService.BuyItem(hero, item);

                await SendAllInfo();
            }
            catch { Console.WriteLine("Error in BuyItem()"); }
        }

        public async Task SellItem(int idHero, string itemName)
        {
            try
            {
                Hero? hero = GameData._heroes.FirstOrDefault(h => h.Id == idHero);
                if (hero == null)
                    return;

                Item? item = hero.Items.FirstOrDefault(x => x.Name == itemName);

                if (item == null)
                    return;

                Hex? hex = GameData._hexes.FirstOrDefault(x => x.HERO != null && x.HERO.Id == idHero);
                if (hex == null || hex.TeamShop != hero.Team)
                    return;

                ShopService.SellItem(hero, item);

                await SendAllInfo();
            }
            catch { Console.WriteLine("Error in BuyItem()"); }
        }

        public async Task GetHeroList()
        {
            try
            {
                await this.Clients.All.SendAsync("DrawPickedHeroes", pickService.GetHeroes(), pickService.pickBanTurn);
            }
            catch { Console.WriteLine("Error in GetHeroList()"); }
        }

        public async Task BanHero(string heroName)
        {
            try
            {
                pickService.BanHero(heroName);
                await this.Clients.All.SendAsync("DrawPickedHeroes", pickService.GetHeroes(), pickService.pickBanTurn);
                await GetUsersBindings();
            }
            catch { Console.WriteLine("Error in BanHero()"); }
        }

        public async Task PickHero(string heroName)
        {
            try
            {
                pickService.PickHero(heroName);
                await this.Clients.All.SendAsync("DrawPickedHeroes", pickService.GetHeroes(), pickService.pickBanTurn);
                await GetUsersBindings();
            }
            catch { Console.WriteLine("Error in PickHero()"); }
        }

        public async Task BeginBattle()
        {
            try
            {
                if (GameData._heroes.Count() == 6)
                {
                    await this.Clients.All.SendAsync("BeginBattle");
                    await GetUsersBindings();
                }
            }
            catch { Console.WriteLine("Error in BeginBattle()"); }
        }

        public async Task BeginRandomBattle()
        {
            try
            {
                CreateHeroList();
                foreach (Hero hero in GameData._heroes.Where(x => x.Team == "blue"))
                {
                    HasteBuff hasteBuff = new HasteBuff(hero.Id, 0, 1);
                    hero.AddEffect(hasteBuff);
                }

                await this.Clients.All.SendAsync("BeginBattle");
                await GetUsersBindings();
            }
            catch { Console.WriteLine("Error in BeginBattle()"); }
        }

        private int[] GetSpellArea(int target, int caster, int spell, string itemName = "")
        {
            Hero? casterHero = GameData._hexes[caster].HERO;
            Skill skill;
            List<int> spellArea = new List<int>();

            if (casterHero != null && itemName != "")
                skill = casterHero.Items.FirstOrDefault(x => x.Name == itemName).Skill;
            else if (casterHero != null && spell >= 0)
                skill = casterHero.SkillList[spell];
            else
                return spellArea.ToArray();


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
                case Consts.SpellArea.Circle:
                    if (dist <= spellRange)
                    {
                        foreach (var hex in UtilityService.GetHexesCircle(GameData._hexes[caster], GameData._hexes[target], skill.radius))
                            spellArea.Add(hex.ID);
                    }
                    break;
                case Consts.SpellArea.Lines:
                    if (dist <= spellRange)
                    {
                        foreach (var hex in UtilityService.GetHexesLines(GameData._hexes[caster], skill.radius))
                            spellArea.Add(hex.ID);
                    }
                    break;
            }

            return spellArea.ToArray();
        }

        private int[] GetRespawnArea(int heroId)
        {
            Hero? hero = GameData._heroes.FirstOrDefault(x => x.Id == heroId);
            List<int> respawnArea = new List<int>();
            List<Hex> respawnAreaHexes = new List<Hex>();

            if (hero != null)
            {
                respawnAreaHexes = GameData._hexes.FindAll(x => x.TeamRespawn == hero.Team);
                foreach (var hex in respawnAreaHexes)
                    respawnArea.Add(hex.ID);
            }

            return respawnArea.ToArray();
        }
    }
}
