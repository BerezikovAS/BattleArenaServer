using BattleArenaServer.Effects;
using BattleArenaServer.Effects.Debuffs;
using BattleArenaServer.Models;

namespace BattleArenaServer.Skills.PlagueDoctorSkills.Auras
{
    public class PlagueAura : Aura
    {
        int resistReduction = 2;
        public PlagueAura(int _resistReduction)
        {
            Name = "Plague";
            radius = 1;
            type = Consts.AuraType.EndTurn;
            resistReduction = _resistReduction;
        }

        public override void ApplyEffect(Hero source, Hero target)
        {
            if (source.Team == target.Team && source.Id != target.Id && target.type != Consts.HeroType.Obstacle)
            {
                Effect? effect = target.EffectList.FirstOrDefault(x => x.Name == "Plague");
                if (effect == null)
                {
                    PlagueDebuff plagueDebuff = new PlagueDebuff(source.Id, resistReduction, 2);
                    target.AddEffect(plagueDebuff);
                }
            }
        }

        public override void CancelEffect(Hero source)
        {

        }
    }
}
