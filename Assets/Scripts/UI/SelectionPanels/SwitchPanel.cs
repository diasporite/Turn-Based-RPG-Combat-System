using UnityEngine;

namespace RPG_Project
{
    public class SwitchPanel : SelectionPanel
    {
        BattleChar[] party;

        Color inBattleColour;
        Color standByColour;

        public BattleChar _currentPartyMember => party[currentIndex];

        public void EnterPanel(BattleChar[] party)
        {
            this.party = party;

            currentIndex = 0;

            if (party.Length > 0)
            {
                for (int i = 0; i < subPanelTexts.Length; i++)
                {
                    if (i == currentIndex)
                    {
                        HighlightPanel(i);
                    }
                    else UnhighlightPanel(i);

                    if (i < party.Length)
                    {
                        subPanelTexts[i].text = party[i]._charName;
                    }
                    else subPanelTexts[i].text = "";
                }
            }

            ui._partyAnalysisPanel.AnalyseBattler(_currentPartyMember);
        }

        public override void NextSelection(int length)
        {
            base.NextSelection(length);

            ui._partyAnalysisPanel.AnalyseBattler(_currentPartyMember);
        }

        public override void PreviousSelection(int length)
        {
            base.PreviousSelection(length);

            ui._partyAnalysisPanel.AnalyseBattler(_currentPartyMember);
        }
    }
}