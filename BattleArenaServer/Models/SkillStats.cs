namespace BattleArenaServer.Models
{
    public class SkillStats
    {
        public SkillStats(int _coolDown, int _requireAP, int _range, int _radius)
        {
            coolDown = _coolDown;
            requireAP = _requireAP;
            range = _range;
            radius = _radius;
        }

        public int coolDown { get; set; }
        public int requireAP { get; set; }
        public int range { get; set; } = 0;
        public int radius { get; set; } = 0;
    }
}
