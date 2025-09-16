using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Models;
using BattleArenaServer.Models.Heroes;
using BattleArenaServer.Models.Lobbies;
using BattleArenaServer.Models.Obstacles;
using BattleArenaServer.Services;
using Microsoft.AspNetCore.SignalR;

namespace BattleArenaServer.Hubs
{
    public class FieldHub : Hub
    {
        public FieldHub()
        {

        }

        public override async Task OnConnectedAsync()
        {
            try
            {
                Lobby? lobby = GetLobby();
                if (lobby == null)
                    return;

                if (lobby.gameData.userTeamBindings.BlueTeam == GetCookieByName("user_id"))
                    lobby.countUsers++;
                if (lobby.gameData.userTeamBindings.RedTeam == GetCookieByName("user_id"))
                    lobby.countUsers++;

                lobby.connectionIds.Add(Context.ConnectionId);
            }
            catch { }

            await base.OnConnectedAsync();
        }

        public override async Task OnDisconnectedAsync(Exception? ex)
        {
            try
            {
                Lobby? lobby = GetLobby();
                if (lobby == null)
                    return;

                if (lobby.gameData.userTeamBindings.BlueTeam == GetCookieByName("user_id"))
                {
                    lobby.countUsers--;
                    if (lobby.isStarted == false)
                        lobby.gameData.userTeamBindings.BlueTeam = "";
                }
                if (lobby.gameData.userTeamBindings.RedTeam == GetCookieByName("user_id"))
                {
                    lobby.countUsers--;
                    if (lobby.isStarted == false)
                        lobby.gameData.userTeamBindings.RedTeam = "";
                }

                lobby.connectionIds.Remove(Context.ConnectionId);
                await this.Clients.Clients(lobby.connectionIds).SendAsync("GetUsersBindings", lobby.gameData.userTeamBindings);
            }
            catch { }

            await base.OnDisconnectedAsync(ex);
        }

        private string GetCookieByName(string key)
        {
            var context = Context.GetHttpContext();

            if (context is not null)
                if (context.Request.Cookies.ContainsKey(key))
                    if (context.Request.Cookies.TryGetValue(key, out var coockie))
                        return coockie;
            return "";
        }

        private Lobby? GetLobby()
        {
            try
            {
                Guid.TryParse(GetCookieByName("lobbyId"), out Guid id);
                return LobbyMaster.lobbies.FirstOrDefault(x => x.Id == id);
            }
            catch
            {

            }
            return null;
        }

        private GameData? GetGameData()
        {
            try
            {
                return GetLobby()?.gameData;
            }
            catch
            {

            }
            return null;
        }

        private void CreateHeroList()
        {
            SetRandomCommands();
            // Применяем эффекты постоянных аур сразу
            AttackService.ContinuousAuraAction(GetGameData());
        }

        private void SetRandomCommands()
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
            heroes.Add(new ArtistHero(0, ""));
            heroes.Add(new LizardmanHero(0, ""));
            



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
                GameData? gameData = GetGameData();
                gameData._hexes[hero.HexId].SetHero(hero);
                gameData._heroes.Add(hero);
            }
        }

        private Hero GetRandomHero(List<Hero> heroes)
        {
            Random rnd = new Random();
            Hero hero = heroes[rnd.Next(0, heroes.Count)];
            heroes.Remove(hero);

            return hero;
        }













        public async Task IsGameStarted(int mode)
        {
            int result = -1;
            Lobby? lobby = GetLobby();
            if (lobby != null)
            {
                result = lobby.isStarted ? 1 : 0;
                if (mode != result)
                    await Clients.Clients(lobby.connectionIds).SendAsync("TransferTo", result);
                else
                    await Clients.Clients(lobby.connectionIds).SendAsync("SetGameMode", result);
                return;
            }
            await Clients.Client(Context.ConnectionId).SendAsync("TransferTo", result);
        }


        public async Task RecreateGame()
        {
            Lobby? lobby = GetLobby();
            if (lobby != null)
            {
                try
                {
                    lobby.gameData.RecreateGame();

                    lobby.isStarted = false;
                    lobby.countUsers = 0;

                    await SetActiveHero(lobby.gameData.idActiveHero);
                    await GetUsersBindings();

                    await this.Clients.Clients(lobby.connectionIds).SendAsync("RecreateGame");
                }
                catch
                {
                    Console.WriteLine("Error in RecreateGame()");
                }
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
            GameData? gameData = GetGameData();
            Hero? hero = gameData._hexes.FirstOrDefault(x => x.ID == cur_pos)?.HERO;
            if (hero != null)
            {
                // Пытаемся прокачать скилл
                if (hero.SkillList[skill].UpgradeSkill(gameData))
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
            GameData? gameData = GetGameData();
            RequestData requestData = new RequestData(cur_pos, targer_pos, gameData);
            if (requestData.Caster != null)
            {
                if (requestData.Target == null && requestData.TargetHex != null && requestData.TargetHex.OBSTACLE != null
                    && requestData.TargetHex.OBSTACLE.Name == "AstralPortal")
                    AttackService.MoveHero(requestData.Caster, requestData.CasterHex, requestData.TargetHex, gameData);
                else
                    requestData.Caster.MoveSkill.Cast(requestData, gameData);
            }

            try { 
                await SendAllInfo();
            }
            catch { Console.WriteLine("Error in StepHero()"); }
        }

        public async Task SpellCast(int targer_pos, int cur_pos, int skill)
        {
            GameData? gameData = GetGameData();
            RequestData requestData = new RequestData(cur_pos, targer_pos, gameData);
            if (requestData.Caster != null && requestData.Caster.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.Silence)) == null)
            {
                requestData.Caster.beforeSpellCast(requestData.Caster, requestData.Target, requestData.Caster.SkillList[skill], gameData);
                // Кастуем))
                if (requestData.Caster.SkillList[skill].Cast(requestData, gameData))
                    requestData.Caster.afterSpellCast(requestData.Caster, requestData.Target, requestData.Caster.SkillList[skill], gameData);

                try {
                    await SendAllInfo();
                }
                catch { Console.WriteLine("Error in SpellCast()"); }
            }
        }

        public async Task ItemCast(int targer_pos, int cur_pos, string itemName)
        {
            GameData? gameData = GetGameData();
            RequestData requestData = new RequestData(cur_pos, targer_pos, gameData);
            if (requestData.Caster != null)
            {
                Item? item = requestData.Caster.Items.FirstOrDefault(x => x.Name == itemName && x.ItemType == Consts.ItemType.Active);
                if (item != null)
                    item.Skill.Cast(requestData, gameData);
                try
                {
                    await SendAllInfo();
                }
                catch { Console.WriteLine("Error in SpellCast()"); }
            }
        }

        public async Task AttackHero(int cur_pos, int targer_pos)
        {
            GameData? gameData = GetGameData();
            RequestData requestData = new RequestData(cur_pos, targer_pos, gameData);

            AttackService.AttackHero(requestData, gameData);

            try {
                await SendAllInfo();
            }
            catch { Console.WriteLine("Error in AttackHero()"); }
        }

        public async Task SendSpellArea(int target, int caster, int spell, string itemName = "")
        {
            Lobby? lobby = GetLobby();
            if (lobby != null)
            {
                try { 
                    await this.Clients.Clients(lobby.connectionIds).SendAsync("DrawSpellArea", GetSpellArea(target, caster, spell, itemName));
                }
                catch { Console.WriteLine("Error in SendSpellArea()"); }
            }
        }

        public async Task SendRespawnArea(int heroId)
        {
            Lobby? lobby = GetLobby();
            if (lobby != null)
            {
                try { await this.Clients.Clients(lobby.connectionIds).SendAsync("DrawSpellArea", GetRespawnArea(heroId)); }
                catch { Console.WriteLine("Error in SendRespawnArea()"); }
            }
        }

        public async Task SetActiveHero(int idActiveHero)
        {
            Lobby? lobby = GetLobby();
            if (lobby != null)
            {
                Hero? hero = lobby.gameData._heroes.FirstOrDefault(x => x.Id == idActiveHero);
                if (hero != null && hero is not SolidObstacle)
                    lobby.gameData.idActiveHero = idActiveHero;

                try { await this.Clients.Clients(lobby.connectionIds).SendAsync("GetActiveHero", lobby.gameData.idActiveHero); }
                catch { Console.WriteLine("Error in SetActiveHero()"); }
            }
        }

        public async Task SendActiveHero()
        {
            try {
                Lobby? lobby = GetLobby();
                if (lobby != null)
                {
                    await this.Clients.Clients(lobby.connectionIds).SendAsync("GetActiveHero", lobby.gameData.timingService.GetActiveHero());
                    await GetShopItems(lobby);
                }
            }
            catch { Console.WriteLine("Error in SendActiveHero()"); }
        }

        public async void EndTurn()
        {
            try
            {
                GetGameData().timingService.EndTurn();
                await GetUsersBindings();
                await SendAllInfo();
            }
            catch { Console.WriteLine("Error in EndTurn()"); }
        }

        public async Task SendAllInfo()
        {
            Lobby? lobby = GetLobby();
            if (lobby != null)
            {
                await this.Clients.Clients(lobby.connectionIds).SendAsync("GetField", lobby.gameData._hexes);
                await this.Clients.Clients(lobby.connectionIds).SendAsync("GetVP", lobby.gameData.userTeamBindings);
                await this.Clients.Clients(lobby.connectionIds).SendAsync("GetBanchHeroes", UtilityService.GetBanchHeroes(lobby.gameData));
                await GetShopItems(lobby);
            }
        }

        public async Task BindingUserToTeam(string userId, string team)
        {
            GameData? gameData = GetGameData();
            if (team == "red")
                gameData.userTeamBindings.RedTeam = userId;
            else
                gameData.userTeamBindings.BlueTeam = userId;

            if (gameData.userTeamBindings.RedTeam != "" && gameData.userTeamBindings.BlueTeam != "") // Выбраны обе команды, можно начинать сражение
                gameData.userTeamBindings.ActiveTeam = gameData.userTeamBindings.RedTeam;

            Lobby? lobby = GetLobby();
            if (lobby != null)
            {
                lobby.countUsers = 0;
                if (lobby.gameData.userTeamBindings.BlueTeam == GetCookieByName("user_id"))
                    lobby.countUsers++;
                if (lobby.gameData.userTeamBindings.RedTeam == GetCookieByName("user_id"))
                    lobby.countUsers++;
            }

            await GetUsersBindings();
        }

        public async Task RespawnHero(int heroId, int hexId)
        {
            GameData? gameData = GetGameData();
            Hero? hero = gameData._heroes.FirstOrDefault(x => x.Id == heroId);
            Hex? hex = gameData._hexes.FirstOrDefault(x => x.ID == hexId && x.IsFree());

            if (hero != null && hex != null && hex.TeamRespawn == hero.Team)
            {
                if (hero.HP < 0)
                    hero.HP += 0;

                hero.Respawn(gameData);

                hex.SetHero(hero);
                hero.HexId = hexId;
                hero.RespawnTime = 0;

                HasteBuff hasteBuff = new HasteBuff(hero.Id, 0, 1);
                hero.AddEffect(hasteBuff, gameData);
            }

            await SendAllInfo();
        }

        public async Task GetUsersBindings()
        {
            Lobby? lobby = GetLobby();
            if (lobby != null)
            {
                try {
                    GameData? gameData = GetGameData();
                    gameData.userTeamBindings.ActiveTeamStr = gameData.activeTeam;
                    await this.Clients.Clients(lobby.connectionIds).SendAsync("GetUsersBindings", gameData.userTeamBindings);
                }
                catch { Console.WriteLine("Error in GetUsersBindings()"); }
            }
        }

        public async Task GetShopItems(Lobby lobby)
        {
            try
            {
                await this.Clients.Clients(lobby.connectionIds).SendAsync("SetShopItems", lobby.gameData.shopService.GetShopItems());
            }
            catch { Console.WriteLine("Error in GetShopItems()"); }
        }

        public async Task BuyItem(int idHero, string itemName)
        {
            try
            {
                GameData? gameData = GetGameData();
                Hero? hero = gameData._heroes.FirstOrDefault(h => h.Id == idHero && h.IsMainHero);
                if (hero == null)
                    return;

                Item? item;
                if (hero.Team == "red")
                    item = gameData._redShop.FirstOrDefault(x => x.Name == itemName);
                else
                    item = gameData._blueShop.FirstOrDefault(x => x.Name == itemName);

                if (item == null)
                    return;

                Hex? hex = gameData._hexes.FirstOrDefault(x => x.HERO != null && x.HERO.Id == idHero);
                if (hex == null || hex.TeamShop != hero.Team)
                    return;

                gameData.shopService.BuyItem(hero, item);

                await SendAllInfo();
            }
            catch { Console.WriteLine("Error in BuyItem()"); }
        }

        public async Task SellItem(int idHero, string itemName)
        {
            try
            {
                GameData? gameData = GetGameData();
                Hero? hero = gameData._heroes.FirstOrDefault(h => h.Id == idHero);
                if (hero == null)
                    return;

                Item? item = hero.Items.FirstOrDefault(x => x.Name == itemName);

                if (item == null)
                    return;

                Hex? hex = gameData._hexes.FirstOrDefault(x => x.HERO != null && x.HERO.Id == idHero);
                if (hex == null || hex.TeamShop != hero.Team)
                    return;

                gameData.shopService.SellItem(hero, item);

                await SendAllInfo();
            }
            catch { Console.WriteLine("Error in BuyItem()"); }
        }

        public async Task GetHeroList()
        {
            Lobby? lobby = GetLobby();
            if (lobby != null)
            {
                try
                {
                    await this.Clients.Clients(lobby.connectionIds).SendAsync("DrawPickedHeroes", lobby.gameData.pickService.GetHeroes(), lobby.gameData.pickService.pickBanTurn);
                }
                catch { Console.WriteLine("Error in GetHeroList()"); }
            }
        }

        public async Task BanHero(string heroName)
        {
            Lobby? lobby = GetLobby();
            if (lobby != null)
            {
                try
                {
                    lobby.gameData.pickService.BanHero(heroName);
                    await this.Clients.Clients(lobby.connectionIds).SendAsync("DrawPickedHeroes", lobby.gameData.pickService.GetHeroes(), lobby.gameData.pickService.pickBanTurn);
                    await GetUsersBindings();
                }
                catch { Console.WriteLine("Error in BanHero()"); }
            }
        }

        public async Task PickHero(string heroName)
        {
            Lobby? lobby = GetLobby();
            if (lobby != null)
            {
                try
                {
                    lobby.gameData.pickService.PickHero(heroName);
                    await this.Clients.Clients(lobby.connectionIds).SendAsync("DrawPickedHeroes", lobby.gameData.pickService.GetHeroes(), lobby.gameData.pickService.pickBanTurn);
                    await GetUsersBindings();
                }
                catch { Console.WriteLine("Error in PickHero()"); }
            }
        }

        public async Task BeginBattle()
        {
            Lobby? lobby = GetLobby();
            if (lobby != null)
            {
                try
                {
                    if (lobby.gameData._heroes.Count() == 6)
                    {
                        lobby.isStarted = true;

                        await this.Clients.Clients(lobby.connectionIds).SendAsync("BeginBattle");
                        await GetUsersBindings();
                    }
                }
                catch { Console.WriteLine("Error in BeginBattle()"); }
            }
        }

        public async Task BeginRandomBattle()
        {
            Lobby? lobby = GetLobby();
            if (lobby != null)
            {
                try
                {
                    CreateHeroList();
                    foreach (Hero hero in lobby.gameData._heroes.Where(x => x.Team == "blue"))
                    {
                        HasteBuff hasteBuff = new HasteBuff(hero.Id, 0, 1);
                        hero.AddEffect(hasteBuff, lobby.gameData);
                    }

                    lobby.isStarted = true;

                    lobby.gameData.activeTeam = "red";

                    await this.Clients.Clients(lobby.connectionIds).SendAsync("BeginBattle");
                    await GetUsersBindings();
                }
                catch { Console.WriteLine("Error in BeginBattle()"); }
            }
        }

        public async Task SendChatMessage(string message)
        {
            Lobby? lobby = GetLobby();
            if (lobby != null)
            {
                try
                {
                    if (message != "")
                    {
                        ChatMessage chatMessage = new ChatMessage(GetCookieByName("user_id"), GetCookieByName("nickname"), message);
                        lobby.gameData.messages.Add(chatMessage);
                    }
                    await this.Clients.Clients(lobby.connectionIds).SendAsync("ReceiveChatMessages", lobby.gameData.messages);
                }
                catch { Console.WriteLine("Error in SendChatMessage()"); }
            }
        }

        private int[] GetSpellArea(int target, int caster, int spell, string itemName = "")
        {
            GameData? gameData = GetGameData();
            Hero? casterHero = gameData._hexes[caster].HERO;
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

            int dist = gameData._hexes[target].Distance(gameData._hexes[caster]);

            switch (skill.area)
            {
                case Consts.SpellArea.AllyTarget:
                    if (dist <= spellRange)
                    {
                        if (gameData._hexes[target].HERO != null && gameData._hexes[target].HERO.Team == casterHero.Team)
                            spellArea.Add(target);
                    }
                    break;
                case Consts.SpellArea.EnemyTarget:
                    if (dist <= spellRange)
                    {
                        if (gameData._hexes[target].HERO != null && gameData._hexes[target].HERO.Team != casterHero.Team &&
                            gameData._hexes[target].HERO.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.NonTargetable)) == null)
                            spellArea.Add(target);
                    }
                    break;
                case Consts.SpellArea.FriendTarget:
                    if (dist <= spellRange && dist > 0)
                    {
                        if (gameData._hexes[target].HERO != null && gameData._hexes[target].HERO.Team == casterHero.Team)
                            spellArea.Add(target);
                    }
                    break;
                case Consts.SpellArea.Radius:
                    if (dist <= spellRange)
                    {
                        foreach (var hex in UtilityService.GetHexesRadius(gameData._hexes[target], skill.radius, gameData))
                            spellArea.Add(hex.ID);
                    }
                    break;
                case Consts.SpellArea.Line:
                    foreach (var hex in UtilityService.GetHexesOneLine(gameData._hexes[caster], gameData._hexes[target], gameData, skill.radius))
                        spellArea.Add(hex.ID);
                    break;
                case Consts.SpellArea.HeroTarget:
                    if (dist <= spellRange)
                    {
                        if (gameData._hexes[target].HERO != null &&
                            (gameData._hexes[target].HERO.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.NonTargetable)) == null || gameData._hexes[target].HERO.Team == casterHero.Team))
                            spellArea.Add(target);
                    }
                    break;
                case Consts.SpellArea.HeroNotSelfTarget:
                    if (dist <= spellRange)
                    {
                        if (gameData._hexes[target].HERO != null && gameData._hexes[target].HERO.Id != casterHero.Id &&
                            (gameData._hexes[target].HERO.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.NonTargetable)) == null || gameData._hexes[target].HERO.Team == casterHero.Team))
                            spellArea.Add(target);
                    }
                    break;
                case Consts.SpellArea.Conus:
                    if (dist <= spellRange)
                    {
                        foreach (var hex in UtilityService.GetHexesCone(gameData._hexes[caster], gameData._hexes[target], skill.radius, gameData))
                            spellArea.Add(hex.ID);
                    }
                    break;
                case Consts.SpellArea.SmallConus:
                    if (dist <= spellRange)
                    {
                        foreach (var hex in UtilityService.GetHexesSmallCone(gameData._hexes[caster], gameData._hexes[target], skill.radius, gameData))
                            spellArea.Add(hex.ID);
                    }
                    break;
                case Consts.SpellArea.WideLine:
                    if (dist <= spellRange)
                    {
                        foreach (var hex in UtilityService.GetHexesWideLine(gameData._hexes[caster], gameData._hexes[target], skill.radius, gameData))
                            spellArea.Add(hex.ID);
                    }
                    break;
                case Consts.SpellArea.Circle:
                    if (dist <= spellRange)
                    {
                        foreach (var hex in UtilityService.GetHexesCircle(gameData._hexes[caster], gameData._hexes[target], skill.radius, gameData))
                            spellArea.Add(hex.ID);
                    }
                    break;
                case Consts.SpellArea.Lines:
                    if (dist <= spellRange)
                    {
                        foreach (var hex in UtilityService.GetHexesLines(gameData._hexes[caster], gameData, skill.radius))
                            spellArea.Add(hex.ID);
                    }
                    break;
            }

            return spellArea.ToArray();
        }

        private int[] GetRespawnArea(int heroId)
        {
            GameData? gameData = GetGameData();
            Hero? hero = gameData._heroes.FirstOrDefault(x => x.Id == heroId);
            List<int> respawnArea = new List<int>();
            List<Hex> respawnAreaHexes = new List<Hex>();

            if (hero != null)
            {
                respawnAreaHexes = gameData._hexes.FindAll(x => x.TeamRespawn == hero.Team);
                foreach (var hex in respawnAreaHexes)
                    respawnArea.Add(hex.ID);
            }

            return respawnArea.ToArray();
        }
    }
}
