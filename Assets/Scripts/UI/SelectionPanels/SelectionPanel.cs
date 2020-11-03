using UnityEngine;
using UnityEngine.UI;

namespace RPG_Project
{
    public class SelectionPanel : MonoBehaviour
    {
        protected int currentIndex = 0;
        [SerializeField] protected Image[] subPanels;
        protected Text[] subPanelTexts;

        Color highlightColour = new Color(0.45f, 0.45f, 0.45f);
        Color normalColour = new Color(0, 0, 0);

        protected UIManager ui;

        public void InitPanel(UIManager ui)
        {
            subPanelTexts = new Text[subPanels.Length];

            this.ui = ui;

            for (int i = 0; i < subPanels.Length; i++)
            {
                subPanelTexts[i] = subPanels[i].GetComponentInChildren<Text>();
            }
        }

        public virtual void NextSelection(int length)
        {
            if (length > 0)
            {
                UnhighlightPanel(currentIndex);

                currentIndex++;
                currentIndex = currentIndex % length;

                HighlightPanel(currentIndex);
            }
        }

        public virtual void PreviousSelection(int length)
        {
            if (length > 0)
            {
                UnhighlightPanel(currentIndex);

                currentIndex--;
                if (currentIndex < 0) currentIndex = length - 1;

                HighlightPanel(currentIndex);
            }
        }

        protected void HighlightPanel(int index)
        {
            subPanels[index].color = highlightColour;
        }

        protected void UnhighlightPanel(int index)
        {
            subPanels[index].color = normalColour;
        }
    }
}