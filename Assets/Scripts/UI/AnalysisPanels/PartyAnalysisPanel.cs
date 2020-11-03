using UnityEngine;
using UnityEngine.UI;

namespace RPG_Project
{
    public class PartyAnalysisPanel : AnalysisPanel
    {
        [SerializeField] Text battleStatusText;

        public Text _battleStatusText => battleStatusText;

        public override void AnalyseBattler(BattleChar battler)
        {
            base.AnalyseBattler(battler);

            if (battler._battleStatus == BattleStatus.InBattle)
            {
                // If statement so that space shows in text
                battleStatusText.text = "In Battle";
            }
            else battleStatusText.text = battler._battleStatus.ToString();
        }
    }
}