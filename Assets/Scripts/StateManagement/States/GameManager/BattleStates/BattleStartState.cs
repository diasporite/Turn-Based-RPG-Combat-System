using System.Collections;
using UnityEngine;

namespace RPG_Project
{
    public class BattleStartState : IState
    {
        BattleManager battle;
        StateMachine bsm;

        UIManager ui;

        float delay = 0.75f;
        float fadeTime = 0.25f;

        public BattleStartState(BattleManager battle)
        {
            this.battle = battle;
            bsm = battle._sm;

            ui = GameManager.instance._ui;

            delay = battle._delay;
            fadeTime = battle._fadeTime;
        }

        #region InterfaceMethods
        public void Enter(params object[] args)
        {
            ui._blackScreenCg.alpha = 1;

            SetUIEnter();

            battle.StartCoroutine(StartCo());
        }

        public void ExecutePerFrame()
        {

        }

        public void Exit()
        {
            battle._autoMode = false;

            SetUILeave();
        }
        #endregion

        void SetUIEnter()
        {
            ui._loseScreen.SetActive(false);
            ui._winScreen.gameObject.SetActive(false);

            ui._messageLog.gameObject.SetActive(true);

            ui._partyStats.gameObject.SetActive(false);
            ui._enemyStats.gameObject.SetActive(false);

            ui._commandPanel.SetActive(false);
            ui._targetPanel.SetActive(false);

            ui._skillPanel.gameObject.SetActive(false);
            ui._itemPanel.gameObject.SetActive(false);

            ui._switchPanel.gameObject.SetActive(false);
            ui._partyAnalysisPanel.gameObject.SetActive(false);

            ui._battlerAnalysisPanel.gameObject.SetActive(false);

            ui._backPanel.SetActive(false);
            ui._autoPanel.SetActive(false);
            ui._scrollPanel.SetActive(false);
            ui._confirmPanel.SetActive(false);
        }

        void SetUILeave()
        {
            ui._autoPanel.SetActive(true);
            ui.SetAutoText(false);

            ui._partyStats.gameObject.SetActive(true);
            ui._partyStats.InitPanels(battle._activeParty);

            ui._enemyStats.gameObject.SetActive(true);
            ui._enemyStats.InitPanels(battle._activeEnemies);
        }

        IEnumerator StartCo()
        {
            // Instantiate players and enemies, populate turn management state machine,
            //   determine turn order
            battle.InitBattle();

            yield return new WaitForSeconds(delay);

            ui.LogMessage("Enemies have appeared!");

            yield return ui.StartCoroutine(ui.FadeFromBlackScreen(0, fadeTime));

            yield return new WaitForSeconds(delay);

            bsm.ChangeState(battle._currentBattler);
        }
    }
}