namespace BattleArenaServer.Models.Lobbies
{
    public class Lobby
    {
        public Lobby(string CreatorId, string name)
        {
            this.CreatorId = CreatorId;
            Name = name;
            Id = Guid.NewGuid();
            gameData = new GameData();
        }
        public Guid Id { get; set; }
        public string CreatorId { get; set; }
        public string Name { get; set; }
        public GameData gameData { get; set; }
        public int countUsers { get; set; } = 0;
        public bool isStarted { get; set; } = false;
        public List<string> connectionIds { get; set; } = new List<string>();
    }
}
