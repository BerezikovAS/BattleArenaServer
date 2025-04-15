namespace BattleArenaServer.Models
{
    public class UserTeamBindings
    {
        public string RedTeam { get; set; }
        public string BlueTeam { get; set; }
        public string ActiveTeam { get; set; }
        public string ActiveTeamStr { get; set; }

        public int RedVP { get; set; }
        public int BlueVP { get; set; }
        public int RedCoins { get; set; }
        public int BlueCoins { get; set; }

        public UserTeamBindings() 
        {
            RedTeam = BlueTeam = ActiveTeam = "";
            RedVP = 0;
            BlueVP = 5;
            RedCoins = BlueCoins = 30;
        }

        public void ClearBindings()
        {
            RedTeam = BlueTeam = ActiveTeam = "";
            RedVP = 0;
            BlueVP = 5;
            RedCoins = BlueCoins = 30;
        }
    }
}
