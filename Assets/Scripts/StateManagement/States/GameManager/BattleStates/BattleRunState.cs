using System.Collections;
using UnityEngine;

namespace RPG_Project
{
    public class BattleRunState : IState
    {
        BattleManager battle;
        StateMachine bsm;

        UIManager ui;

        float delay = 0.75f;
        float fadeTime = 0.25f;

        public BattleRunState(BattleManager battle)
        {
            this.battle = battle;
            bsm = battle._sm;

            ui = GameManager.instance._ui;

            fadeTime = battle._fadeTime;
        }

        #region InterfaceMethods
        public void Enter(params object[] args)
        {
            Run();
        }

        public void ExecutePerFrame()
        {

        }

        public void Exit()
        {

        }
        #endregion

        bool RunSuccessful(float speedRatio)
        {
            if (speedRatio <= 0) return false;
            if (speedRatio >= 1) return true;

            float i = Random.Range(0f, 1f);

            return i < speedRatio;
        }

        float AverageSpeed(BattleChar[] group)
        {
            int avgSpeed = 0;

            foreach(var member in group)
            {
                avgSpeed += member._speed._currentStatValue;
            }

            return avgSpeed / group.Length;
        }

        void Run()
        {
            var players = battle._activeParty;
            var enemies = battle._activeEnemies;

            float speedRatio = AverageSpeed(players) / AverageSpeed(enemies);

            if (RunSuccessful(speedRatio)) battle.StartCoroutine(RunCo());
            else battle.StartCoroutine(TrappedCo());
        }

        IEnumerator RunCo()
        {
            ui.LogMessage("Tried to flee!");

            yield return new WaitForSeconds(delay);

            ui.LogMessage("Successfully fled!");

            yield return new WaitForSeconds(delay);

            yield return ui.StartCoroutine(ui.FadeToBlackScreen(fadeTime));

            Application.Quit();
        }

        IEnumerator TrappedCo()
        {
            ui.LogMessage("Tried to flee!");

            yield return new WaitForSeconds(delay);

            ui.LogMessage("Couldn't escape!");

            yield return new WaitForSeconds(delay);

            battle.AdvanceTurn();
        }
    }
}