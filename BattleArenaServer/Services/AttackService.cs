using BattleArenaServer.Effects;
using BattleArenaServer.Models;
using BattleArenaServer.Models.Obstacles;
using static BattleArenaServer.Models.Consts;

namespace BattleArenaServer.Services
{
    public static class AttackService
    {
        public static int dealedDmg = 0;

        private static double ShieldDefence(Hero defender, double dmg, Consts.EffectTag shieldType)
        {
            double totalDmg = dmg;
            List<Effect> shields = defender.EffectList.FindAll(x => x.effectTags.Contains(shieldType));
            foreach (var shield in shields)
            {
                shield.value -= (int)Math.Round(totalDmg);
                if (shield.value <= 0)
                {
                    defender.EffectList.Remove(shield);
                    totalDmg = shield.value * -1;
                }
                else
                {
                    totalDmg = 0;
                    shield.RefreshDescr();
                }
            }
            return totalDmg;
        }

        private static int ArmorPiercedValue(int armor, Hero? attacker, Hero defender)
        {
            if (attacker == null || armor <= 0) // Если нет героя атаки (?) или броня уже ноль или меньше, вернем её
                return armor;

            int armorPiercing = attacker.GetArmorPiercing(attacker, defender);
            if (armor - armorPiercing <= 0) // Если игнорирование брони снижает её до нуля и ниже, то вернем 0 (У нас тут игнорирование брони, а не уменьшение!)
                return 0;
            return armor - armorPiercing; // Иначе возвращаем броню с учетом значение игнора
        }

        private static int ResistPiercedValue(int resist, Hero? attacker, Hero defender)
        {
            if (attacker == null || resist <= 0) // Если нет героя атаки (?) или сопротивление уже ноль или меньше, вернем его
                return resist;

            int resistPiercing = attacker.GetResistPiercing(attacker, defender);
            if (resist - resistPiercing <= 0) // Если игнорирование сопротивления снижает его до нуля и ниже, то вернем 0
                return 0;
            return resist - resistPiercing; // Иначе возвращаем сопротивление с учетом значение игнора
        }

        public static bool AttackHero(RequestData requestData, GameData gameData)
        {
            if (requestData.CasterHex != null && requestData.TargetHex != null && requestData.Caster != null && requestData.Target != null &&
                requestData.Target.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.NonTargetable)) == null && // Цель должна быть доступна для выбора
                requestData.Caster.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.Disarm)) == null) // Атакующий не должен быть обезоружен
            {
                Hero attacker = requestData.Caster;
                Hero defender = requestData.Target;

                int range = attacker.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.Blind)) == null ? attacker.AttackRadius + attacker.StatsEffect.AttackRadius : 1;

                int availableAP = attacker.AP;

                Effect? taunt = attacker.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.Taunt));
                if (taunt != null && taunt.idCaster != defender.Id) // Если атакующий под таунтом, то он может атаковать только заклинателя
                    return false;

                // Спиритическая связь объединяет ОД двух героев, поэтому доступные ОД - это ОД кастера + ОД связанного героя
                Effect? spiritLink = attacker.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.SpiritLink));
                if (spiritLink != null)
                {
                    Hero? anotherHero = gameData._heroes.FirstOrDefault(x => x.Id == spiritLink.idCaster);
                    if (anotherHero != null)
                        availableAP += anotherHero.AP;
                }

                if (requestData.CasterHex.Distance(requestData.TargetHex) > range || availableAP < attacker.APtoAttack)
                    return false;

                attacker.SpendAP(attacker.APtoAttack, gameData);

                // Если у цели есть уворот, то вся атака отменяется, а ОД тратятся
                Effect? dodge = defender.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.Dodge));
                if (dodge != null)
                {
                    defender.EffectList.Remove(dodge);
                    return true;
                }

                // К урону добавляем дополнительный от пассивок и эффектов
                int dmg = (int)((attacker.Dmg + attacker.GetPassiveAttackDamage(attacker, defender, gameData)) * attacker.StatsEffect.DmgMultiplier);

                // Эффекты перед атакой
                attacker.beforeAttack(attacker, defender, dmg, gameData);
                defender.beforeReceivedAttack(attacker, defender, dmg);
                // Сама атака с нанесением урона
                SetDamage(attacker, defender, dmg, Consts.DamageType.Physical, gameData);
                // Эффекты после атаки
                attacker.afterAttack(attacker, defender, dmg, Consts.DamageType.Physical, gameData);
                defender.afterReceivedAttack(attacker, defender, dmg, gameData);
                return true;
            }
            return false;
        }

        public static bool SetDamage(Hero? attacker, Hero defender, int dmg, DamageType damageType, GameData gameData)
        {
            if (defender != null)
            {
                //Бабл защищает от первого урона. Если он есть, не наносим урон, а сам эффект снимаем
                Effect? bubble = defender.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.Bubble));
                if (bubble != null)
                {
                    bubble.RemoveEffect(defender, gameData);
                    defender.EffectList.Remove(bubble);
                    return false;
                }
                //Великий суд делает весь входящий урон чистым
                if (defender.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.GreateJudgement)) != null)
                    damageType = DamageType.Pure;
                //ГА защищает от физ. урона
                if (defender.EffectList.FirstOrDefault(x => x.effectTags.Contains(Consts.EffectTag.GuardianAngel)) != null && damageType == DamageType.Physical)
                    dmg = 0;

                defender.beforeReceiveDmg(defender, attacker, dmg, damageType, gameData);

                double totalDmg = 0;
                switch (damageType)
                {
                    case DamageType.Physical:
                        {
                            int armor = defender.Armor + defender.GetPassiveArmor(attacker, defender, gameData);
                            armor = ArmorPiercedValue(armor, attacker, defender);
                            totalDmg = dmg * (1 - (0.1 * armor) / (1 + 0.1 * armor));

                            totalDmg = ShieldDefence(defender, totalDmg, Consts.EffectTag.PhysShield); // Сначала убираем щиты на физ. урон
                            totalDmg = ShieldDefence(defender, totalDmg, Consts.EffectTag.DmgShield); // Только потом смотрим на универсальные щиты
                        }
                        break;
                    case DamageType.Magic:
                        {
                            int resist = defender.Resist + defender.GetPassiveResistance(attacker, defender);
                            resist = ResistPiercedValue(resist, attacker, defender);
                            totalDmg = dmg * (1 - (0.1 * resist) / (1 + 0.1 * resist));

                            totalDmg = ShieldDefence(defender, totalDmg, Consts.EffectTag.MagicShield); // Сначала убираем щиты на маг. урон
                            totalDmg = ShieldDefence(defender, totalDmg, Consts.EffectTag.DmgShield); // Только потом смотрим на универсальные щиты
                        }
                        break;
                    case DamageType.Pure:
                        {
                            totalDmg = dmg;

                            totalDmg = ShieldDefence(defender, totalDmg, Consts.EffectTag.DmgShield); // Только универсальный щит защищает от чистого урона
                        }
                        break;
                }

                return defender.applyDamage(attacker, defender, (int)Math.Round(totalDmg), damageType, gameData);
            }
            return false;
        }

        public static bool ApplyDamage(Hero? attacker, Hero defender, int dmg, DamageType damageType, GameData gameData)
        {
            // Добавляем модификатор получаемого урона
            dmg += defender.GetModifierAppliedDamage(attacker, defender, dmg, damageType);
            dmg = dmg < 0 ? 0 : dmg;

            defender.HP -= dmg;
            if (attacker != null)
            {
                int dmgReducer = 1;
                if (defender.EffectList.FirstOrDefault(x => x.Name == "ShadowTwin") != null)
                    dmgReducer = 5;
                attacker.DamageDealed += (int)(Convert.ToDouble(dmg) / dmgReducer);
            }

            defender.afterReceiveDmg(defender, attacker, dmg, damageType, gameData);

            foreach (var effect in defender.EffectList)
                effect.RefreshDescr();

            if (defender.HP <= 0)
            {
                KillHero(defender, gameData);
                return true;
            }
            return false;
        }

        public static void KillHero(Hero defender, GameData gameData)
        {
            defender.HP = -1;
            Hex? hex = gameData._hexes.FirstOrDefault(x => x.ID == defender.HexId);
            if (hex != null && hex.HERO != null)
            {
                Hero killedHero = hex.HERO;
                killedHero.HexId = -1;
                hex.RemoveHero();
                ContinuousAuraAction(gameData);

                if (killedHero is SolidObstacle)
                {
                    SolidObstacle obstacle = (SolidObstacle)killedHero;
                    obstacle.endLifeEffect(hex, gameData);
                    gameData._solidObstacles.Remove(obstacle);
                }
            }
            if (defender.Team == "red")
            {
                gameData.userTeamBindings.BlueVP += defender.VP;
                gameData.userTeamBindings.BlueCoins += defender.GoldReward;
            }
            else
            {
                gameData.userTeamBindings.RedVP += defender.VP;
                gameData.userTeamBindings.RedCoins += defender.GoldReward;
            }
            gameData.userTeamBindings.CheckWin();

            if (defender.IsMainHero)
            {
                defender.RespawnTime = 4;

                foreach (var effect in defender.EffectList)
                    effect.RemoveEffect(defender, gameData);
                defender.EffectList.Clear();
            }
        }

        /// <summary>
        /// Обновление эффектов постоянных аур
        /// </summary>
        public static void ContinuousAuraAction(GameData gameData)
        {
            foreach (var hero in gameData._heroes)
            {
                foreach (var aura in hero.AuraList)
                {
                    if (aura.type == Consts.AuraType.Continuous)
                    {
                        aura.CancelEffect(hero, gameData);
                        Hex? hexSource = gameData._hexes.FirstOrDefault(x => x.ID == hero.HexId);
                        if (hexSource != null)
                            aura.SetEffect(hero, hexSource, gameData);
                    }
                }
            }

            foreach (var hero in gameData._solidObstacles)
            {
                foreach (var aura in hero.AuraList)
                {
                    if (aura.type == Consts.AuraType.Continuous)
                    {
                        aura.CancelEffect(hero, gameData);
                        Hex? hexSource = gameData._hexes.FirstOrDefault(x => x.ID == hero.HexId);
                        if (hexSource != null)
                            aura.SetEffect(hero, hexSource, gameData);
                    }
                }
            }
        }

        /// <summary>
        /// Применение эффекта ауры в конце хода текущего героя
        /// </summary>
        /// <param name="sourceAura"></param>
        public static void EndTurnAuraAction(Hero sourceAura, GameData gameData)
        {
            foreach (var aura in sourceAura.AuraList)
            {
                if (aura.type == Consts.AuraType.EndTurn)
                {
                    Hex? hexSource = gameData._hexes.FirstOrDefault(x => x.ID == sourceAura.HexId);
                    if (hexSource != null)
                        aura.SetEffect(sourceAura, hexSource, gameData);
                }
            }
        }

        /// <summary>
        /// Применение эффекта ауры в начале хода текущего героя
        /// </summary>
        /// <param name="sourceAura"></param>
        public static void StartTurnAuraAction(Hero sourceAura, GameData gameData)
        {
            foreach (var aura in sourceAura.AuraList)
            {
                if (aura.type == Consts.AuraType.StartTurn)
                {
                    Hex? hexSource = gameData._hexes.FirstOrDefault(x => x.ID == sourceAura.HexId);
                    if (hexSource != null)
                        aura.SetEffect(sourceAura, hexSource, gameData);
                }
            }
        }

        /// <summary>
        /// Перемещение героя с одного гекса на другой
        /// </summary>
        /// <param name="hero"></param>
        /// <param name="currentHex"></param>
        /// <param name="targetHex"></param>
        public static void MoveHero(Hero hero, Hex? currentHex, Hex targetHex, GameData gameData)
        {
            if (currentHex != null)
                currentHex.RemoveHero();
            targetHex.SetHero(hero);
            ContinuousAuraAction(gameData);

            if (targetHex.OBSTACLE != null && targetHex.OBSTACLE is FillableObstacle)
            {
                (targetHex.OBSTACLE as FillableObstacle).ApplyEffect(hero, targetHex, gameData);
            }

            foreach (var surface in targetHex.SURFACES)
            {
                surface.ApplyEffect(hero, targetHex, gameData);
            }

            // Применяем эффекты после движения (если есть)
            hero.afterMove(hero, currentHex, targetHex, gameData);
        }
    }
}
