using BattleArenaServer.Models;
using static BattleArenaServer.Models.Consts;

namespace BattleArenaServer.Services
{
    public static class AttackService
    {
        public static bool SetDamage(Hero attacker, Hero defender, int dmg, DamageType damageType)
        {
            if (defender != null)
            {
                double totalDmg = 0;
                switch (damageType)
                {
                    case DamageType.Physical:
                        {
                            int armor = defender.Armor + defender.StatsEffect.Armor + defender.passiveArmor(attacker, defender);
                            totalDmg = dmg * (1 - (0.1 * armor) / (1 + 0.1 * armor));
                        }
                        break;
                    case DamageType.Magic:
                        {
                            int resist = defender.Resist + defender.StatsEffect.Resist + defender.passiveResistance(attacker, defender);
                            totalDmg = dmg * (1 - (0.1 * resist) / (1 + 0.1 * resist));
                        }
                        break;
                    case DamageType.Pure:
                        {
                            totalDmg = dmg;
                        }
                        break;
                }

                return defender.applyDamage(attacker, defender, (int)Math.Round(totalDmg));
            }
            return false;
        }

        public static bool ApplyDamage(Hero attacker, Hero defender, int dmg)
        {
            // Добавляем модификатор получаемого урона
            dmg += defender.modifierAppliedDamage(attacker, defender, dmg);

            defender.HP -= dmg;

            if (defender.HP <= 0)
            {
                Hex? hex = GameData._hexes.FirstOrDefault(x => x.ID == defender.HexId);
                if (hex != null)
                    hex.RemoveHero();
                //GameData._heroes.Remove(defender);
                return true;
            }
            return false;
        }

        public static void ContinuousAuraAction()
        {
            foreach (var hero in GameData._heroes)
            {
                foreach (var aura in hero.AuraList)
                {
                    if (aura.type == Consts.AuraType.Continuous)
                    {
                        aura.CancelEffect();
                        Hex? hexSource = GameData._hexes.FirstOrDefault(x => x.ID == hero.HexId);
                        if (hexSource != null)
                            aura.SetEffect(hero, hexSource);
                    }
                }                
            }
        }

        public static void EndTurnAuraAction(Hero sourceAura)
        {
            foreach (var aura in sourceAura.AuraList)
            {
                if (aura.type == Consts.AuraType.EndTurn)
                {
                    Hex? hexSource = GameData._hexes.FirstOrDefault(x => x.ID == sourceAura.HexId);
                    if (hexSource != null)
                        aura.SetEffect(sourceAura, hexSource);
                }
            }
        }

        public static void MoveHero(Hero hero, Hex currentHex, Hex targetHex)
        {
            targetHex.SetHero(hero);
            currentHex.RemoveHero();
            ContinuousAuraAction();
        }
    }
}
