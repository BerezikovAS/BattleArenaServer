using BattleArenaServer.Models;
using BattleArenaServer.Services;

namespace BattleArenaServer.Effects.Debuffs
{
    public class VoodooPuppetDebuff : Effect
    {
        public VoodooPuppetDebuff(int _idCaster, int _value, int _duration)
        {
            Name = "VoodooPuppet";
            type = Consts.StatusEffect.Debuff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            description = "Персонаж поддается воле заклинателя и повторяет его движения.";
        }

        public override void ApplyEffect(Hero _hero)
        {
            Hero? caster = GameData._heroes[idCaster];
            if (caster != null)
                caster.afterMove += AfterMove;
        }

        public override void RemoveEffect(Hero _hero)
        {
            Hero? caster = GameData._heroes[idCaster];
            if (caster != null)
                caster.afterMove -= AfterMove;
        }

        private void AfterMove(Hero hero, Hex? currentHex, Hex targetHex)
        {
            Hex dir = UtilityService.GetDirection(currentHex, targetHex);

            foreach (Hero enemy in GameData._heroes.Where(x => x.EffectList.FirstOrDefault(y => y.Name == "VoodooPuppet" && y.idCaster == hero.Id) != null
                && x.HP > 0))
            {
                Hex? enemyHex = GameData._hexes.FirstOrDefault(x => x.ID == enemy.HexId);
                if (enemyHex != null)
                {
                    Hex? moveHex = UtilityService.GetOneHexOnDirection(enemyHex, dir, 1, 1);
                    if (moveHex != null && moveHex.IsFree())
                        AttackService.MoveHero(enemy, enemyHex, moveHex);
                }
            }
        }
    }
}
