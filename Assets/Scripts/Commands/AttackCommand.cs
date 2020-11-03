using System.Collections;
using UnityEngine;

namespace RPG_Project
{
    public class AttackCommand : Command
    {
        CommandType commandType;

        AttackSkill skill;
        DamageItem item;

        BattleManager battle;
        CombatManager combat;
        UIManager ui;

        TypeDatabase typeDatabase;

        float smallDelay = 0.25f;
        float normalDelay = 0.75f;

        float shakeTime = 0.25f;

        Color normalColour = new Color(1, 0, 0);
        Color weakColour = new Color(1, 1, 0);
        Color resistColour = new Color(0.5f, 0, 0.85f);

        Color outlineColour = new Color(0, 0, 0);

        #region Constructors
        public AttackCommand(BattleChar instigator, AttackSkill skill) : base(instigator)
        {
            this.skill = skill;

            battle = game._battle;
            combat = game._combat;
            ui = game._ui;

            typeDatabase = game._typeDatabase;

            normalDelay = battle._delay;
            smallDelay = 0.33f * normalDelay;

            commandType = CommandType.Skill;
        }

        public AttackCommand(BattleChar instigator, DamageItem item) : base(instigator)
        {
            this.item = item;

            battle = game._battle;
            combat = game._combat;
            ui = game._ui;

            typeDatabase = game._typeDatabase;

            normalDelay = battle._delay;
            smallDelay = 0.33f * normalDelay;

            commandType = CommandType.Item;
        }
        #endregion

        public override IEnumerator ExecuteCo()
        {
            switch (commandType)
            {
                case CommandType.Skill:
                    yield return SkillAttackCo();
                    break;
                case CommandType.Item:
                    yield return ItemAttackCo();
                    break;
                default:
                    yield return null;
                    break;
            }
        }

        IEnumerator SkillAttackCo()
        {
            ui.LogMessage(instigator._charName + " used " + skill._skillName + "!");

            instigator._magic.ChangeCurrentPoints(-skill._mpCost);

            ui.UpdateStats(instigator);

            bool hit = combat.SuccessfulHit(instigator, targets, skill);

            yield return new WaitForSeconds(normalDelay);

            if (hit)
            {
                foreach (var target in targets)
                {
                    float effectiveness;    // >1 for weak, 0<x<1 for resist, 0 for immune
                    float critical;
                    int damage;

                    bool dead;

                    BattlePortrait targetPortrait = battle.GetPortrait(target);

                    critical = combat.Critical(instigator);

                    effectiveness = combat.Effectiveness(skill, target);

                    damage = Damage(instigator, target, skill, effectiveness, critical);

                    dead = target.TakeDamage(damage);

                    LogMessageSkill(target, effectiveness, critical, damage);

                    ui.UpdateStats(target);

                    CreateDamageNumber(targetPortrait, effectiveness, damage);

                    yield return battle.StartCoroutine(targetPortrait.ShakeCo(effectiveness, shakeTime));

                    if (dead)
                    {
                        if (battle.InActiveEnemies(target))
                        {
                            battle.AddExpEarned(target._expReward);
                            battle.AddMoneyEarned(target._moneyReward);
                        }
                        yield return battle.StartCoroutine(battle.RemoveBattlerCo(target, smallDelay));
                    }
                }
            }
            else
            {
                ui.LogMessage("The attack missed!");
            }

            yield return new WaitForSeconds(normalDelay);
        }

        IEnumerator ItemAttackCo()
        {
            float delay;
            TypeID damageType = item._damageType;

            if (battle._autoMode) delay = smallDelay;
            else delay = normalDelay;

            if (StartsWithVowel(item._itemName))
                ui.LogMessage(instigator._charName + " used an " + item._itemName + "!");
            else ui.LogMessage(instigator._charName + " used a " + item._itemName + "!");

            yield return new WaitForSeconds(2f * smallDelay);

            foreach (var target in targets)
            {
                float effectiveness;
                int damage;

                bool dead;

                BattlePortrait targetPortrait = battle.GetPortrait(target);

                effectiveness = combat.Effectiveness(item._damageType, target);

                damage = Damage(item, effectiveness);

                dead = target.TakeDamage(damage);

                LogMessageItem(target, effectiveness, damage);

                ui.UpdateStats(target);

                CreateDamageNumber(targetPortrait, effectiveness, damage);

                yield return battle.StartCoroutine(targetPortrait.ShakeCo(effectiveness, shakeTime));

                if (dead)
                {
                    if (battle.InActiveEnemies(target))
                    {
                        battle.AddExpEarned(target._expReward);
                        battle.AddMoneyEarned(target._moneyReward);
                    }
                    yield return battle.StartCoroutine(battle.RemoveBattlerCo(target, delay));
                }
            }

            game._inventory.RemoveItem(item);

            yield return new WaitForSeconds(delay);
        }

        #region DamageCalculation
        int Damage(BattleChar instigator, BattleChar target, AttackSkill skill, 
            float effectiveness, float critical)
        {
            if (effectiveness == 0) return 0;

            int damageCap = game._pointsCap;

            int damage = Mathf.RoundToInt(effectiveness * critical * combat.RawDamage(instigator, target, skill));

            if (damage <= 0) damage = 1;
            if (damage > damageCap) damage = damageCap;

            return damage;
        }

        int Damage(DamageItem item, float effectiveness)
        {
            int damageCap = game._pointsCap;

            int damage = Mathf.RoundToInt(effectiveness * item._damage);

            if (damage <= 0) damage = 1;
            if (damage > damageCap) damage = damageCap;

            return damage;
        }
        #endregion

        #region MessageLog
        void LogMessageSkill(BattleChar target, float effectiveness, float critical, int damage)
        {
            string damageMessage;

            if (critical > 1)
            {
                if (effectiveness > 1) damageMessage = "Hit weakness! " + instigator._charName +
                        " dealt a critical " + damage + " damage against " + target._charName + "!";
                else if (effectiveness < 1 && effectiveness > 0) damageMessage = "Hit resistance - " +
                        instigator._charName + " dealt a critical " + damage + " damage against " +
                        target._charName + "!";
                else if (effectiveness == 0) damageMessage = "The attack did not affect " +
                        target._charName + ".";
                else damageMessage = instigator._charName + " dealt a critical " + damage +
                        " damage against " + target._charName + "!";
            }
            else
            {
                if (effectiveness > 1) damageMessage = "Hit weakness! " + instigator._charName +
                        " dealt " + damage + " damage against " + target._charName + "!";
                else if (effectiveness < 1 && effectiveness > 0) damageMessage = "Hit resistance - " +
                        instigator._charName + " dealt " + damage + " damage against " +
                        target._charName + "!";
                else if (effectiveness == 0) damageMessage = "The attack did not affect " +
                        target._charName + ".";
                else damageMessage = instigator._charName + " dealt " + damage + " damage against " +
                    target._charName + "!";
            }

            ui.LogMessage(damageMessage);
        }

        void LogMessageItem(BattleChar target, float effectiveness, int damage)
        {
            string damageMessage;

            if (effectiveness > 1) damageMessage = "Hit weakness! The " + item._itemName + 
                    " did " + damage + " damage against " + target._charName + "!";
            else if (effectiveness < 1 && effectiveness > 0) damageMessage = "Hit resistance - The " +
                    item._itemName + " did " + damage + " damage against " + target._charName + "!";
            else if (effectiveness == 0) damageMessage = "The item did not affect " + target._charName + ".";
            else damageMessage = "The " + item._itemName + " dealt " + damage + " damage against " +
                target._charName + "!";

            ui.LogMessage(damageMessage);
        }

        void CreateDamageNumber(BattlePortrait targetPortrait, float effectiveness, int damage)
        {
            GameObject damageNumberObj = GameObject.Instantiate(ui._floatingTextPrefab, 
                targetPortrait.transform.position + 0.5f * Vector3.one, Quaternion.identity) as GameObject;

            FloatingText damageNum = damageNumberObj.GetComponent<FloatingText>();

            string message = "-" + damage;

            if (effectiveness > 1)
                damageNum.StartCoroutine(damageNum.TextFloatCo(message, weakColour, outlineColour));
            else if (effectiveness < 1)
                damageNum.StartCoroutine(damageNum.TextFloatCo(message, resistColour, outlineColour));
            else damageNum.StartCoroutine(damageNum.TextFloatCo(message, normalColour, outlineColour));
        }
        #endregion

        public bool StartsWithVowel(string str)
        {
            var initial = str.Trim().ToLower()[0];

            return initial == 'a' || initial == 'e' || initial == 'i' || initial == 'o' || initial == 'u';
        }
    }
}