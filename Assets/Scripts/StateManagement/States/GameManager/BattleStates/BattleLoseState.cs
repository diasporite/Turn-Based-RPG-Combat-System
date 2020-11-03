using System.Collections;
using UnityEngine;

namespace RPG_Project
{
    public class BattleLoseState : IState
    {
        BattleManager battle;
        StateMachine bsm;

        UIManager ui;

        float fadeTime = 0.25f;
        float delay = 1f;
        bool fading = false;

        public BattleLoseState(BattleManager battle)
        {
            this.battle = battle;
            bsm = battle._sm;

            ui = GameManager.instance._ui;

            fadeTime = battle._fadeTime;
        }

        #region InterfaceMethods
        public void Enter(params object[] args)
        {
            ui._loseScreen.SetActive(true);

            battle.StartCoroutine(EnterCo());
        }

        public void ExecutePerFrame()
        {
            if (!fading && Input.anyKeyDown)
            {
                Advance();
            }
        }

        public void Exit()
        {
            ui._loseScreen.SetActive(false);
        }
        #endregion

        void Advance()
        {
            Application.Quit();
        }

        IEnumerator EnterCo()
        {
            fading = true;

            yield return ui.StartCoroutine(ui.FadeFromBlackScreen(0, fadeTime));

            yield return new WaitForSeconds(delay);

            fading = false;
        }

        IEnumerator LeaveCo()
        {
            fading = true;

            yield return ui.StartCoroutine(ui.FadeToBlackScreen(fadeTime));

            fading = false;
        }
    }
}