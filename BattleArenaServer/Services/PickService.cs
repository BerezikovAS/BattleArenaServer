using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Models;
using BattleArenaServer.Models.Heroes;

namespace BattleArenaServer.Services
{
    public class PickService
    {
        public List<Hero> heroes = new List<Hero>();
        public int order = 1;
        public PickBan pickBanTurn = PickBan.Ban;

        public enum PickBan
        {
            Pick,
            Ban,
            ReadyToBattle
        }

        public void FillHeroesList()
        {
            heroes.Clear();
            pickBanTurn = PickBan.Ban;
            order = 1;

            heroes.Add(new AbominationHero(0, ""));
            heroes.Add(new AeroturgHero(0, ""));
            heroes.Add(new ArcherHero(0, ""));
            heroes.Add(new AssassinHero(0, ""));
            heroes.Add(new BerserkerHero(0, ""));
            heroes.Add(new ChaosHero(0, ""));
            heroes.Add(new CultistHero(0, ""));
            heroes.Add(new DruidHero(0, ""));
            heroes.Add(new DwarfHero(0, ""));
            heroes.Add(new ElementalistHero(0, ""));
            heroes.Add(new FairyHero(0, ""));
            heroes.Add(new FallenKingHero(0, ""));
            heroes.Add(new GeomantHero(0, ""));
            heroes.Add(new GhostHero(0, ""));
            heroes.Add(new GolemHero(0, ""));
            heroes.Add(new GuardianHero(0, ""));
            heroes.Add(new InvokerHero(0, ""));
            heroes.Add(new KnightHero(0, ""));
            heroes.Add(new MusketeerHero(0, ""));
            heroes.Add(new NecromancerHero(0, ""));
            heroes.Add(new PlagueDoctorHero(0, ""));
            heroes.Add(new PriestHero(0, ""));
            heroes.Add(new SeraphimHero(0, ""));
            heroes.Add(new ShadowHero(0, ""));
            heroes.Add(new SnowQueenHero(0, ""));
            heroes.Add(new TinkerHero(0, ""));
            heroes.Add(new VampireHero(0, ""));
            heroes.Add(new WitchDoctorHero(0, ""));
        }

        public List<Hero> GetHeroes()
        {
            return heroes;
        }

        public bool PickHero(string heroName)
        {
            if (pickBanTurn != PickBan.Pick)
                return false;

            Hero? hero = heroes.FirstOrDefault(h => h.Name == heroName && h.Team == "" && h.Id == 0);
            if (hero == null)
                return false;

            int newId = heroes.FindAll(h => h.Id != 0).Count() + 1;
            hero.Id = newId;
            hero.Team = GameData.activeTeam;

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
            ChangePickBanStage();
            return true;
        }

        public bool BanHero(string heroName)
        {
            if (pickBanTurn != PickBan.Ban)
                return false;

            Hero? hero = heroes.FirstOrDefault(h => h.Name == heroName && h.Team == "" && h.Id == 0);
            if (hero == null)
                return false;

            hero.Id = -1;

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
            ChangePickBanStage();
            return true;
        }

        public void StartBattle()
        {
            List<Hero> pickedHeroes = heroes.FindAll(x => x.Id > 0);
            List<int> redCoords = [7, 22, 37];
            List<int> blueCoords = [14, 29, 44];

            Random rnd = new Random();
            int rndCoords = 0;
            foreach (var hero in pickedHeroes)
            {
                if (hero.Team == "red")
                {
                    rndCoords = rnd.Next(redCoords.Count);
                    hero.HexId = redCoords[rndCoords];
                    redCoords.Remove(redCoords[rndCoords]);
                }
                else
                {
                    rndCoords = rnd.Next(blueCoords.Count);
                    hero.HexId = blueCoords[rndCoords];
                    blueCoords.Remove(blueCoords[rndCoords]);

                    HasteBuff hasteBuff = new HasteBuff(hero.Id, 0, 1);
                    hero.AddEffect(hasteBuff);
                }
                GameData._hexes[hero.HexId].SetHero(hero);
                GameData._heroes.Add(hero);
            }
        }

        public void ChangePickBanStage()
        {
            if (order++ % 2 == 0)
                pickBanTurn = pickBanTurn == PickBan.Pick ? PickBan.Ban : PickBan.Pick;
            if (heroes.FindAll(x => x.Id > 0).Count() == 6)
            {
                pickBanTurn = PickBan.ReadyToBattle;
                StartBattle();
            }
        }
    }
}
