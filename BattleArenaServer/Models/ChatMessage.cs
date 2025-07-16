namespace BattleArenaServer.Models
{
    public class ChatMessage
    {
        public ChatMessage(string ChatterId, string ChatterName, string Message)
        {
            this.ChatterId = ChatterId;
            this.ChatterName = ChatterName;
            this.Message = Message;
        }
        public string ChatterId { get; set; } = "";
        public string ChatterName { get; set; } = "";
        public string Message { get; set; } = "";
    }
}
