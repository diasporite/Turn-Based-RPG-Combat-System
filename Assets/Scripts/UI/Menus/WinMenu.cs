using UnityEngine;
using UnityEngine.UI;

namespace RPG_Project
{
    public class WinMenu : MonoBehaviour
    {
        [SerializeField] Text headerText;
        [SerializeField] GameObject panelHolder;
        [SerializeField] ResultCharPanel[] panels;

        int expEarned;
        int moneyEarned;

        Party party;
        BattleChar[] partyMembers;

        Inventory inventory;

        public Text _headerText => headerText;
        public GameObject _panelHolder => panelHolder;
        public ResultCharPanel[] _panels => panels;

        public void InitMenu()
        {
            panels = panelHolder.GetComponentsInChildren<ResultCharPanel>();

            party = GameManager.instance._party;
            inventory = GameManager.instance._inventory;
        }

        public void EnterMenu(int expEarned, int moneyEarned)
        {
            this.expEarned = expEarned;
            this.moneyEarned = moneyEarned;

            headerText.text = "Obtained " + expEarned + "EXP and " + moneyEarned + "G.";

            PopulateMenu();
        }

        public void UpdateMenu()
        {
            UpdateParty();
        }

        void PopulateMenu()
        {
            partyMembers = party._party;

            for(int i = 0; i < panels.Length; i++)
            {
                if (i < partyMembers.Length)
                {
                    panels[i].PopulateCharPanel(partyMembers[i]);
                }
                else
                {
                    panels[i].SetElementsActive(false);
                }
            }
        }

        void UpdateParty()
        {
            party.PartyGainExperience(expEarned);

            inventory._money += moneyEarned;

            for (int i = 0; i < panels.Length; i++)
            {
                if (i < partyMembers.Length)
                {
                    panels[i].UpdateCharPanel(partyMembers[i]);
                }
            }
        }
    }
}