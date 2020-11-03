using UnityEngine;
using UnityEngine.UI;

namespace RPG_Project
{
    public class AnalysisPanel : MonoBehaviour
    {
        [SerializeField] protected Image portrait;

        [SerializeField] protected Text nameText;

        [SerializeField] protected Text type1Text;
        [SerializeField] protected Text type2Text;

        [SerializeField] protected Text lvText;
        [SerializeField] protected Text hpText;
        [SerializeField] protected Text mpText;

        [SerializeField] protected Text atkValueText;
        [SerializeField] protected Text defValueText;
        [SerializeField] protected Text spdValueText;
        [SerializeField] protected Text lckValueText;

        [SerializeField] protected Text[] skillTexts;

        Color normalTextColour = new Color(1, 1, 1);
        Color halfHealthColour = new Color(1, 1, 0);
        Color lowHealthColour = new Color(1, 0, 0);

        Color buffColour = new Color(1, 0, 0);
        Color debuffColour = new Color(0, 0, 1);
        Color normalColour = new Color(1, 1, 1);

        public Image _portrait => portrait;

        public Text _nameText => nameText;

        public Text _type1Text => type1Text;
        public Text _type2Text => type2Text;

        public Text _lvText => lvText;
        public Text _hpText => hpText;
        public Text _mpText => mpText;

        public Text _atkValueText => atkValueText;
        public Text _defValueText => defValueText;
        public Text _spdValueText => spdValueText;
        public Text _lckValueText => lckValueText;

        public Text[] _skillTexts => skillTexts;

        public virtual void AnalyseBattler(BattleChar battler)
        {
            portrait.sprite = battler._sprite;

            nameText.text = battler._charName;

            if (battler._type1 != TypeID.Typeless)
                type1Text.text = battler._type1.ToString();
            else type1Text.text = "";

            if (battler._type2 != TypeID.Typeless)
                type2Text.text = battler._type2.ToString();
            else type2Text.text = "";

            lvText.text = "LV " + battler._lv;
            hpText.text = "HP " + battler._health._pointValue + "/" + battler._health._currentStatValue;
            mpText.text = "MP " + battler._magic._pointValue + "/" + battler._magic._currentStatValue;

            if (battler._health._pointsFraction <= 0.2f)
                hpText.color = lowHealthColour;
            else if (battler._health._pointsFraction > 0.2f && battler._health._pointsFraction <= 0.5f)
                hpText.color = halfHealthColour;
            else hpText.color = normalTextColour;

            atkValueText.text = battler._attack._currentStatValue.ToString();
            defValueText.text = battler._defence._currentStatValue.ToString();

            if (battler._attack._currentStatValue > battler._attack._statValue)
                atkValueText.color = buffColour;
            else if (battler._attack._currentStatValue < battler._attack._statValue)
                atkValueText.color = debuffColour;
            else atkValueText.color = normalColour;

            if (battler._defence._currentStatValue > battler._defence._statValue)
                defValueText.color = buffColour;
            else if (battler._defence._currentStatValue < battler._defence._statValue)
                defValueText.color = debuffColour;
            else defValueText.color = normalColour;

            spdValueText.text = battler._speed._currentStatValue.ToString();
            lckValueText.text = battler._luck._currentStatValue.ToString();

            for(int i = 0; i < skillTexts.Length; i++)
            {
                if (i < battler._currentSkillset.Length)
                {
                    skillTexts[i].text = battler._currentSkillset[i]._skillName;
                }
                else skillTexts[i].text = "";
            }
        }
    }
}