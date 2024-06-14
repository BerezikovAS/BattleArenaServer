namespace BattleArenaServer.Models
{
    public class RequestData
    {
        public RequestData(int? casterPosId, int? targetPosId)
        {
            CasterPositionId = casterPosId;
            TargetPositionId = targetPosId;

            CasterHex = GameData._hexes.FirstOrDefault(x => x.ID == CasterPositionId);
            TargetHex = GameData._hexes.FirstOrDefault(x => x.ID == TargetPositionId);

            Caster = CasterHex?.HERO;
            Target = TargetHex?.HERO;
        }

        public int? CasterPositionId { get; set; }

        public int? TargetPositionId { get; set; }

        public Hero? Caster { get; set; }

        public Hero? Target { get; set; }

        public Hex? CasterHex { get; set; }

        public Hex? TargetHex { get; set; }

        public bool HasCaster() => Caster != null;
        public bool HasTarget() => Target != null;

        public bool HasCasterHex() => CasterHex != null;
        public bool HasTargetHex() => TargetHex != null;
    }
}
