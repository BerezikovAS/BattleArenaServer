using BattleArenaServer.Models;
using BattleArenaServer.Services;

namespace BattleArenaServer.Effects.Unique
{
    public class DisappearUnique : Effect
    {
        List<Hex> list = new List<Hex>();
        Hex casterHex;
        Hex targetHex;
        bool isDisappear = false;
        public DisappearUnique(int _idCaster, int _value, int _duration, Hex _casterHex, Hex _targetHex, List<Hex> _list)
        {
            Name = "Disappear";
            type = Consts.StatusEffect.Unique;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            effectType = Consts.EffectType.StartTurn;
            description = "Вы появились на поле боя. С возвращением =)";
            list = _list;
            casterHex = _casterHex;
            targetHex = _targetHex;
        }

        public override void ApplyEffect(Hero hero)
        {
            if (isDisappear)
            {
                isDisappear = !isDisappear;
                List<Hex> freeHexes = new List<Hex>();
                foreach (var h in list)
                {
                    if (h.IsFree())
                        freeHexes.Add(h);
                }

                Hex? hex = null;
                Random rnd = new Random();

                if (freeHexes.Count() > 0)
                {
                    while (hex == null)
                        hex = freeHexes.FirstOrDefault(x => x.ID == freeHexes[rnd.Next(0, freeHexes.Count)].ID && x.IsFree());
                }
                else // На случай когда все клетки в выбранной области оказались заняты. Ищем любой ближайший и появляемся там
                {
                    int radius = 2;
                    while (hex == null)
                    {
                        freeHexes = GameData._hexes.FindAll(x => x.Distance(targetHex) == radius && x.IsFree());
                        if (freeHexes.Count() > 0)
                        {
                            while (hex == null)
                                hex = freeHexes.FirstOrDefault(x => x.ID == freeHexes[rnd.Next(0, freeHexes.Count)].ID && x.IsFree());
                        }
                        radius++;
                    }
                }

                hex.SetHero(hero);
                //Наносим урон
                foreach (var hexR in UtilityService.GetHexesRadius(hex, 1))
                {
                    if (hexR.HERO != null && hexR.HERO.Team != hero.Team)
                        AttackService.SetDamage(hero, hexR.HERO, value, Consts.DamageType.Magic);
                }
            }
            else
            {
                isDisappear = true;
                casterHex.RemoveHero();
                hero.HexId = -1;
            }
        }

        public override void RemoveEffect(Hero hero)
        {

        }
    }
}
