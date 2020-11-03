using System.Collections;
using UnityEngine;

namespace RPG_Project
{
    public class BattleSwitchState : IState
    {
        BattleManager battle;
        StateMachine bsm;

        UIManager ui;

        float delay = 0.75f;
        float fadeTime = 0.25f;

        public BattleSwitchState(BattleManager battle)
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
            ui._switchPanel.gameObject.SetActive(false);
            ui._partyAnalysisPanel.gameObject.SetActive(false);

            battle.StartCoroutine(SwitchBattler(battle._currentBattler, battle._switchNewChar));
        }

        public void ExecutePerFrame()
        {

        }

        public void Exit()
        {

        }
        #endregion

        IEnumerator SwitchBattler(BattleChar currentChar, BattleChar newChar)
        {
            battle.SwitchBattler(newChar);

            ui.LogMessage(currentChar._charName + " switched places with " + newChar._charName + "!");

            yield return new WaitForSeconds(delay);

            if (newChar._speed._currentStatValue <= currentChar._speed._currentStatValue)
                battle._sm.ChangeState(battle._currentBattler);
            else
            {
                battle._currentBattlerIndex++;
                battle._sm.ChangeState(battle._currentBattler);
            }
        }
    }
}