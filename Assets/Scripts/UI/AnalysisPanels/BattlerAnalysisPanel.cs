using UnityEngine;
using UnityEngine.UI;

namespace RPG_Project
{
    public class BattlerAnalysisPanel : AnalysisPanel
    {
        [SerializeField] Text sideText;

        BattleManager battle = null;

        public Text _sideText => sideText;

        public override void AnalyseBattler(BattleChar battler)
        {
            base.AnalyseBattler(battler);

            if (battle == null) battle = GameManager.instance._battle;

            if (GameManager.instance._battle.InActiveParty(battler)) sideText.text = "Party";
            else if (GameManager.instance._battle.InActiveEnemies(battler)) sideText.text = "Enemy";
        }
    }
}