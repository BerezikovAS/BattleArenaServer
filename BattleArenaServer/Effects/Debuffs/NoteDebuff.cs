using BattleArenaServer.Models;

namespace BattleArenaServer.Effects.Debuffs
{
    public class NoteDebuff : Effect
    {
        string casterName = "";
        public NoteDebuff(int _idCaster, int _value, int _duration, string _casterName)
        {
            Name = "Note";
            type = Consts.StatusEffect.Debuff;
            idCaster = _idCaster;
            value = _value;
            duration = _duration;
            casterName = _casterName;
            description = $"Наложено нот: {value}. Сила некоторых способностей {casterName} увеличена.";

            effectTags.Add(Consts.EffectTag.Note);
        }

        public override void RefreshDescr()
        {
            description = $"Наложено нот: {value}. Сила некоторых способностей {casterName} увеличена.";
        }

        public override void ApplyEffect(Hero hero, GameData gameData)
        {
            value++;
            duration++;
            RefreshDescr();
        }

        public override void RemoveEffect(Hero hero, GameData gameData)
        {

        }
    }
}
