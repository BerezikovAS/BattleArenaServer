using BattleArenaServer.Effects;
using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Models;

namespace BattleArenaServer.Skills.PlagueDoctorSkills.Auras
{
    public class PlagueAura : Aura
    {
        int resistReduction = 2;
        int appliedTurn = 0;
        public PlagueAura(int _resistReduction, int _appliedTurn)
        {
            Name = "Plague";
            radius = 1;
            type = Consts.AuraType.EndTurn;
            resistReduction = _resistReduction;
            appliedTurn = _appliedTurn;
        }

        public override void ApplyEffect(Hero source, Hero target, GameData gameData)
        {
            if (source.Team == target.Team && source.Id != target.Id && target.type != Consts.HeroType.Obstacle && gameData.turn > appliedTurn)
            {
                Effect? effect = target.EffectList.FirstOrDefault(x => x.Name == "Plague");
                if (effect == null)
                {
                    PlagueDebuff plagueDebuff = new PlagueDebuff(source.Id, resistReduction, 2, gameData.turn);
                    target.AddEffect(plagueDebuff, gameData);
                }
            }
        }

        public override void CancelEffect(Hero source, GameData gameData)
        {

        }
    }
}
