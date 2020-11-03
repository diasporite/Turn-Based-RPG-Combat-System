using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

namespace RPG_Project
{
    public class BattlerStats : MonoBehaviour
    {
        protected Image background;
        protected Color highlightColour = new Color(0.45f, 0.45f, 0.45f);
        protected Color standbyColour = new Color(0f, 0f, 0f);

        [SerializeField] protected Text nameText;
        [SerializeField] protected Text lvText;
        [SerializeField] protected Text hpText;

        [SerializeField] protected BattleChar battler;

        public Image _background => background;

        public Text _nameText => nameText;
        public Text _lvText => lvText;
        public Text _hpText => hpText;

        public BattleChar _battler
        {
            get => battler;
            set => battler = value;
        }

        public virtual void InitPanel(BattleChar bc)
        {
            background = GetComponent<Image>();

            battler = bc;

            if (bc != null)
            {
                nameText.text = battler._charName;

                hpText.text = "HP " + battler._health._pointValue + "/" + battler._health._statValue;

                lvText.text = "LV " + battler._lv;
            }
            else
            {
                nameText.text = "";

                hpText.text = "";

                lvText.text = "";
            }
        }

        public virtual void UpdateStats()
        {
            hpText.text = "HP " + battler._health._pointValue + "/" + battler._health._statValue;
        }

        public void HighlightPanel(bool enteringTurn)
        {
            if (enteringTurn) background.color = highlightColour;
            else background.color = standbyColour;
        }
    }
}