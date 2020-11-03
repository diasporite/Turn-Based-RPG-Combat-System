using System.Collections;
using UnityEngine;

namespace RPG_Project
{
    public class BattleWinState : IState
    {
        BattleManager battle;
        StateMachine bsm;

        UIManager ui;

        bool updatedStats = false;

        float fadeTime = 0.2f;
        bool fading = false;

        public BattleWinState(BattleManager battle)
        {
            this.battle = battle;
            bsm = battle._sm;

            ui = GameManager.instance._ui;

            ui._winScreen.InitMenu();

            fadeTime = battle._fadeTime;
        }

        #region InterfaceMethods
        public void Enter(params object[] args)
        {
            ui._winScreen.gameObject.SetActive(true);

            updatedStats = false;

            ui._winScreen.EnterMenu(battle._currentExpEarned, battle._currentMoneyEarned);

            battle.StartCoroutine(EnterCo());
        }

        public void ExecutePerFrame()
        {
            Advance();
        }

        public void Exit()
        {
            ui._winScreen.gameObject.SetActive(false);
        }
        #endregion

        void Advance()
        {
            if (!fading && Input.anyKeyDown)
            {
                if (!updatedStats) UpdateStats();
                else battle.StartCoroutine(LeaveCo());
            }
        }

        void UpdateStats()
        {
            updatedStats = true;

            // Update stats
            ui._winScreen.UpdateMenu();
        }

        IEnumerator EnterCo()
        {
            fading = true;

            yield return ui.StartCoroutine(ui.FadeFromBlackScreen(0, fadeTime));

            fading = false;
        }

        IEnumerator LeaveCo()
        {
            fading = true;

            yield return ui.StartCoroutine(ui.FadeToBlackScreen(fadeTime));

            fading = false;

            Application.Quit();
        }
    }
}