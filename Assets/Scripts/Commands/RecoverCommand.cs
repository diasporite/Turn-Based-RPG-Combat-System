using System.Collections;
using UnityEngine;

namespace RPG_Project
{
    public class RecoverCommand : Command
    {
        CommandType recoverType;

        HealSkill skill;
        RecoveryItem item;

        BattleManager battle;
        UIManager ui;

        float delay = 0.75f;

        Color hpColour = new Color(0, 1, 0);
        Color mpColour = new Color(0, 0, 1);
        Color outlineColour = new Color(0, 0, 0);

        public RecoverCommand(BattleChar instigator, HealSkill skill) : base(instigator)
        {
            this.skill = skill;

            battle = GameManager.instance._battle;
            ui = GameManager.instance._ui;

            delay = battle._delay;

            recoverType = CommandType.Skill;
        }

        public RecoverCommand(BattleChar instigator, RecoveryItem item) : base(instigator)
        {
            this.item = item;

            battle = GameManager.instance._battle;
            ui = GameManager.instance._ui;

            delay = battle._delay;

            recoverType = CommandType.Item;
        }

        public override IEnumerator ExecuteCo()
        {
            switch (recoverType)
            {
                case CommandType.Skill:
                    yield return SkillCo();
                    break;

                case CommandType.Item:
                    yield return ItemCo();
                    break;

                default:
                    break;
            }
        }

        IEnumerator SkillCo()
        {
            ui.LogMessage(instigator._charName + " used " + skill._skillName + "!");

            yield return new WaitForSeconds(delay);

            foreach (var target in targets)
            {
                int hp;
                BattlePortrait targetPortrait = battle.GetPortrait(target);

                if (skill._healByAmount) hp = skill._healPower;
                else hp = Mathf.RoundToInt(skill._healProportion * instigator._health._currentStatValue);

                target._health.ChangeCurrentPoints(hp);

                GameObject hpHealNumber = GameObject.Instantiate(ui._floatingTextPrefab,
                    targetPortrait.transform.position + 0.75f * Vector3.one, Quaternion.identity) as GameObject;

                FloatingText hpHealNum = hpHealNumber.GetComponent<FloatingText>();

                hpHealNum.StartCoroutine(hpHealNum.TextFloatCo(hp.ToString(), hpColour, outlineColour));
            }

            yield return new WaitForSeconds(delay);
        }

        IEnumerator ItemCo()
        {
            ui.LogMessage("Used the " + item._itemName + "!");

            yield return new WaitForSeconds(delay);

            foreach (var target in targets)
            {
                BattlePortrait targetPortrait = battle.GetPortrait(target);

                if (item._recoverHp)
                {
                    int hp = item._recoverAmountHp;
                    target._health.ChangeCurrentPoints(hp);

                    GameObject hpHealNumber = GameObject.Instantiate(ui._floatingTextPrefab,
                        targetPortrait.transform.position + 0.75f * Vector3.one, Quaternion.identity) as GameObject;

                    FloatingText hpHealNum = hpHealNumber.GetComponent<FloatingText>();

                    hpHealNum.StartCoroutine(hpHealNum.TextFloatCo(hp.ToString(), hpColour, outlineColour));
                }

                if (item._recoverMp)
                {
                    int mp = item._recoverAmountMp;
                    target._magic.ChangeCurrentPoints(mp);

                    GameObject mpHealNumber = GameObject.Instantiate(ui._floatingTextPrefab,
                        targetPortrait.transform.position + 0.25f * Vector3.one, Quaternion.identity) as GameObject;

                    FloatingText mpHealNum = mpHealNumber.GetComponent<FloatingText>();

                    mpHealNum.StartCoroutine(mpHealNum.TextFloatCo(mp.ToString(), mpColour, outlineColour));
                }

                ui.UpdateStats(target);
            }

            GameManager.instance._inventory.RemoveItem(item);

            yield return new WaitForSeconds(delay);
        }
    }
}