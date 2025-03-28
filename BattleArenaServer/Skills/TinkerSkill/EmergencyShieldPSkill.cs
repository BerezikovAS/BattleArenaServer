using BattleArenaServer.Models;
using BattleArenaServer.Effects.Buffs;
using BattleArenaServer.Effects;

namespace BattleArenaServer.Skills.TinkerSkill
{
    public class EmergencyShieldPSkill : PassiveSkill
    {
        int dmgReceived = 0;
        int dmgTreshhold = 200;
        int shieldDurability = 80;
        public EmergencyShieldPSkill(Hero hero) : base(hero)
        {
            name = "Emergency Shield";
            title = $"Потеряв {dmgTreshhold} ХП в рамках одного хода, срабатывает механизм экстренной защиты, даруя щит, поглощающий {shieldDurability} урона.";
            titleUpg = "-25 к порогу потери ХП, +20 прочности щита";
            skillType = Consts.SkillType.Passive;
            hero.afterReceiveDmg += AfterReceiveDmgDelegate;
        }

        public override bool Cast(RequestData requestData)
        {
            return false;
        }

        public override void refreshEffect()
        {
            dmgReceived = 0;
        }

        public override bool UpgradeSkill()
        {
            if (!upgraded)
            {
                upgraded = true;
                hero.afterReceiveDmg -= AfterReceiveDmgDelegate;
                dmgTreshhold -= 25;
                shieldDurability += 20;
                hero.afterReceiveDmg += AfterReceiveDmgDelegate;
                title = $"Потеряв {dmgTreshhold} ХП в рамках одного хода, срабатывает механизм экстренной защиты, даруя щит, поглощающий {shieldDurability} урона.";
                return true;
            }
            return false;
        }

        private void AfterReceiveDmgDelegate(Hero defender, Hero? attacker, int dmg)
        {
            dmgReceived += dmg;
            if (dmgReceived >= dmgTreshhold)
            {
                dmgReceived = 0;
                Effect? oldShield = defender.EffectList.FirstOrDefault(x => x.idCaster == defender.Id && x.effectTags.Contains(Consts.EffectTag.DmgShield));
                if (oldShield != null)
                    defender.EffectList.Remove(oldShield); // Если уже висел старый щит, уберем его

                DmgShieldBuff dmgShieldBuff = new DmgShieldBuff(defender.Id, shieldDurability, 3);
                defender.EffectList.Add(dmgShieldBuff);
            }
        }
    }
}
