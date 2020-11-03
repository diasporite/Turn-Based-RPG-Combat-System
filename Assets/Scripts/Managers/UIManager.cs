using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace RPG_Project
{
    public class UIManager : MonoBehaviour
    {
        [SerializeField] GameObject ui;

        [SerializeField] GameObject battleUi;

        [SerializeField] GameObject blackScreen;
        CanvasGroup blackScreenCg;

        [SerializeField] GameObject reticle;

        [SerializeField] MessageLog messageLog;

        [Header("Player Turn Panels")]
        [SerializeField] GameObject commandPanel;
        [SerializeField] GameObject targetPanel;

        [SerializeField] SkillPanel skillPanel;

        [SerializeField] ItemPanel itemPanel;

        [SerializeField] BattlerAnalysisPanel battlerAnalysisPanel;

        [SerializeField] SwitchPanel switchPanel;
        [SerializeField] AnalysisPanel partyAnalysisPanel;

        [Header("Selection Panels")]
        [SerializeField] GameObject backPanel;
        [SerializeField] GameObject autoPanel;
        [SerializeField] GameObject scrollPanel;
        [SerializeField] GameObject confirmPanel;

        [Header("Stat Panels")]
        [SerializeField] GroupStats partyStats;
        [SerializeField] GroupStats enemyStats;

        [Header("Result Screens")]
        [SerializeField] WinMenu winScreen;
        [SerializeField] GameObject loseScreen;

        [Header("Prefabs")]
        [SerializeField] FloatingText floatingText;

        GameManager game;
        BattleManager battle;

        #region Getters
        public GameObject _ui => ui;

        public GameObject _battleUi => battleUi;
        
        public GameObject _blackScreen => blackScreen;
        public CanvasGroup _blackScreenCg => blackScreenCg;

        public GameObject _reticle => reticle;

        public GameObject _commandPanel => commandPanel;
        public GameObject _targetPanel => targetPanel;

        public SkillPanel _skillPanel => skillPanel;

        public ItemPanel _itemPanel => itemPanel;

        public BattlerAnalysisPanel _battlerAnalysisPanel => battlerAnalysisPanel;

        public SwitchPanel _switchPanel => switchPanel;
        public AnalysisPanel _partyAnalysisPanel => partyAnalysisPanel;

        public GameObject _backPanel => backPanel;
        public GameObject _autoPanel => autoPanel;
        public GameObject _scrollPanel => scrollPanel;
        public GameObject _confirmPanel => confirmPanel;

        public GroupStats _partyStats => partyStats;
        public GroupStats _enemyStats => enemyStats;

        public WinMenu _winScreen => winScreen;
        public GameObject _loseScreen => loseScreen;

        public MessageLog _messageLog => messageLog;

        public GameObject _floatingTextPrefab => floatingText.gameObject;
        #endregion

        private void Awake()
        {
            game = GetComponent<GameManager>();
            battle = GetComponent<BattleManager>();

            blackScreenCg = blackScreen.GetComponent<CanvasGroup>();
        }

        public void InitUI()
        {
            itemPanel.InitPanel(this);
            switchPanel.InitPanel(this);
            skillPanel.InitPanel(this);
        }

        public void LogMessage(string message)
        {
            messageLog.LogMessage(message);
        }

        public void SetAutoText(bool value)
        {
            Text autoText = autoPanel.GetComponentInChildren<Text>();
            KeyCode autoKey = battle._autoKey;

            if (value)
            {
                autoText.text = "(" + autoKey.ToString() + ") Cancel Auto";
            }
            else autoText.text = "(" + autoKey.ToString() + ") Enter Auto";
        }

        public void UpdateStats(BattleChar battler)
        {
            if (partyStats._statsDict.ContainsKey(battler)) partyStats.UpdateStats(battler);

            if (enemyStats._statsDict.ContainsKey(battler)) enemyStats.UpdateStats(battler);
        }

        public IEnumerator FadeToBlackScreen(float dt)
        {
            float t = 0;
            float a0;
            float da;

            a0 = blackScreenCg.alpha;
            da = 1 - a0;

            while (t < 1)
            {
                t += Time.deltaTime / dt;
                blackScreenCg.alpha = a0 + t * da;
                yield return null;
            }

            blackScreenCg.alpha = 1;
        }

        public IEnumerator FadeFromBlackScreen(float alpha, float dt)
        {
            float t = 0;
            float a0;
            float da;

            a0 = blackScreenCg.alpha;
            da = alpha - 1;

            while (t < 1)
            {
                t += Time.deltaTime / dt;
                blackScreenCg.alpha = a0 + t * da;
                yield return null;
            }

            blackScreenCg.alpha = alpha;
        }
    }
}