using UnityEngine;
using UnityEngine.UI;

namespace RPG_Project
{
    public class PlayerStats : BattlerStats
    {
        [SerializeField] Text mpText;

        public override void InitPanel(BattleChar bc)
        {
            base.InitPanel(bc);

            if (bc != null)
            {
                mpText.text = "MP " + bc._magic._pointValue + "/" + bc._magic._statValue;
            }
            else mpText.text = "";
        }

        public override void UpdateStats()
        {
            base.UpdateStats();

            mpText.text = "MP " + battler._magic._pointValue + "/" + battler._magic._statValue;
        }
    }
}