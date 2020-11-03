using UnityEngine;
using UnityEngine.UI;

namespace RPG_Project
{
    public class CharPanel : MonoBehaviour
    {
        [SerializeField] protected Image portrait;

        [SerializeField] protected Text charNameText;
        [SerializeField] protected Text lvText;
        [SerializeField] protected Text type1Text;
        [SerializeField] protected Text type2Text;

        protected BattleChar character;

        public Image _portrait => portrait;

        public Text _charNameText => charNameText;
        public Text _lvText => lvText;
        public Text _type1Text => type1Text;
        public Text _type2Text => type2Text;

        public BattleChar _character => character;

        public virtual void PopulateCharPanel(BattleChar character)
        {
            this.character = character;

            portrait.sprite = character._sprite;

            charNameText.text = character._charName;

            lvText.text = "LV " + character._lv;

            if (character._type1 == TypeID.Typeless) type1Text.text = "";
            else type1Text.text = character._type1.ToString();

            if (character._type2 == TypeID.Typeless) type2Text.text = "";
            else type2Text.text = character._type2.ToString();
        }

        public virtual void UpdateCharPanel(BattleChar character)
        {

        }

        public virtual void SetElementsActive(bool value)
        {
            portrait.gameObject.SetActive(value);

            charNameText.gameObject.SetActive(value);
            lvText.gameObject.SetActive(value);

            type1Text.gameObject.SetActive(value);
            type2Text.gameObject.SetActive(value);
        }
    }
}