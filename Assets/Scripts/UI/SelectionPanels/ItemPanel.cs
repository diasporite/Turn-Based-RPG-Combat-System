namespace RPG_Project
{
    public class ItemPanel : SelectionPanel
    {
        Item[] items;

        public Item _currentItem => items[currentIndex];

        public void EnterPanel(Item[] inventory)
        {
            items = inventory;

            currentIndex = 0;

            if (inventory.Length > 0)
            {
                for (int i = 0; i < subPanelTexts.Length; i++)
                {
                    if (i == currentIndex)
                    {
                        HighlightPanel(i);
                    }
                    else UnhighlightPanel(i);

                    if (i < inventory.Length)
                    {
                        subPanelTexts[i].text = inventory[i]._itemName;
                    }
                    else subPanelTexts[i].text = "";
                }

                ui.LogMessage(_currentItem._description);
            }
        }

        public override void NextSelection(int length)
        {
            base.NextSelection(length);

            ui.LogMessage(_currentItem._description);
        }

        public override void PreviousSelection(int length)
        {
            base.PreviousSelection(length);

            ui.LogMessage(_currentItem._description);
        }
    }
}