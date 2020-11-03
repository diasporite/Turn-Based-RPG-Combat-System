using UnityEngine;
using UnityEngine.UI;

namespace RPG_Project
{
    public class ResultCharPanel : CharPanel
    {
        [SerializeField] Text newSkillText;
        [SerializeField] Text expToNextHeaderText;
        [SerializeField] Text expToNextValueText;

        public Text _newSkillText => newSkillText;
        public Text _expToNextHeaderText => expToNextHeaderText;
        public Text _expToNextValueText => expToNextValueText;

        public override void PopulateCharPanel(BattleChar character)
        {
            base.PopulateCharPanel(character);

            newSkillText.text = "";

            expToNextValueText.text = character.GetExpToNext().ToString();
        }

        public override void UpdateCharPanel(BattleChar character)
        {
            lvText.text = "LV " + character._lv;

            if (character._learntNewSkill) newSkillText.text = "New skill learnt.";
            else newSkillText.text = "";

            expToNextValueText.text = character.GetExpToNext().ToString();
        }

        public override void SetElementsActive(bool value)
        {
            base.SetElementsActive(value);

            newSkillText.gameObject.SetActive(value);
            expToNextHeaderText.gameObject.SetActive(value);
            expToNextValueText.gameObject.SetActive(value);
        }
    }
}