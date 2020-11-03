namespace RPG_Project
{
    public class SkillPanel : SelectionPanel
    {
        Skill[] skills;

        public Skill _currentSkill => skills[currentIndex];

        public void EnterPanel(Skill[] skills)
        {
            this.skills = skills;

            currentIndex = 0;

            if (skills.Length > 0)
            {
                for (int i = 0; i < subPanelTexts.Length; i++)
                {
                    if (i == currentIndex)
                    {
                        HighlightPanel(i);
                    }
                    else UnhighlightPanel(i);

                    if (i < skills.Length)
                    {
                        subPanelTexts[i].text = skills[i]._skillName + " (" + skills[i]._mpCost + "MP)";
                    }
                    else subPanelTexts[i].text = "";
                }

                ui.LogMessage(_currentSkill._description);
            }
        }

        public override void NextSelection(int length)
        {
            base.NextSelection(length);

            ui.LogMessage(_currentSkill._description);
        }

        public override void PreviousSelection(int length)
        {
            base.PreviousSelection(length);

            ui.LogMessage(_currentSkill._description);
        }
    }
}