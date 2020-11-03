using System.Collections;
using UnityEngine;

namespace RPG_Project
{
    public class PlayerTargetState : IState
    {
        BattleManager battle;
        PlayerTurn turn;
        StateMachine sm;

        BattleChar combatant;

        UIManager ui;

        int reticleIndex = 0;
        Vector3[] targetPositions;
        BattlePortrait[] targetPortraits;

        bool movingReticle = false;

        float cursorDelay = 0.25f;

        public PlayerTargetState(PlayerTurn turn)
        {
            battle = turn._battle;
            ui = turn._ui;

            this.turn = turn;
            sm = turn._sm;

            combatant = turn._combatant;
        }

        #region InterfaceMethods
        public void Enter(params object[] args)
        {
            reticleIndex = 0;

            ui._backPanel.SetActive(true);
            ui._scrollPanel.SetActive(true);
            ui._confirmPanel.SetActive(true);

            ui.LogMessage("Target who?");

            targetPortraits = battle.GetPortraits(turn._targets);
            targetPositions = GetTargetPositions(targetPortraits);

            battle._reticleObj.SetActive(true);
            battle._reticleObj.transform.position = targetPositions[reticleIndex] + Vector3.down;
        }

        public void ExecutePerFrame()
        {
            NextTarget();
            PreviousTarget();
            ConfirmTarget();
            Back();
        }

        public void Exit()
        {
            battle._reticleObj.SetActive(false);

            ui._backPanel.SetActive(false);
            ui._scrollPanel.SetActive(false);
            ui._confirmPanel.SetActive(false);
        }
        #endregion

        Vector3[] GetTargetPositions(BattlePortrait[] portraits)
        {
            Vector3[] pos = new Vector3[portraits.Length];

            for (int i = 0; i < portraits.Length; i++)
            {
                pos[i] = portraits[i].transform.position;
            }

            return pos;
        }

        void NextTarget()
        {
            if (battle._next)
            {
                reticleIndex++;

                reticleIndex = reticleIndex % targetPositions.Length;

                battle._reticleObj.transform.position = targetPositions[reticleIndex] + Vector3.down;
            }
        }

        void PreviousTarget()
        {
            if (battle._previous)
            {
                reticleIndex--;

                if (reticleIndex < 0) reticleIndex = targetPositions.Length - 1;

                battle._reticleObj.transform.position = targetPositions[reticleIndex] + Vector3.down;
            }
        }

        void ConfirmTarget()
        {
            if (battle._confirm)
            {
                // Set the rest of the action variables
                BattleChar target = turn._targets[reticleIndex];

                turn._command._targets = new BattleChar[1] { target };

                sm.ChangeState(StateID.PlayerTurnAction);
            }
        }

        void Back()
        {
            if (battle._backOut)
            {
                sm.ChangeState(StateID.PlayerTurnCommand);
            }
        }

        IEnumerator ReticleWaitCo()
        {
            movingReticle = true;

            yield return new WaitForSeconds(cursorDelay);

            movingReticle = false;
        }

        IEnumerator MoveReticleCo(int increment)
        {
            movingReticle = true;

            //ui._enemyStats.HighlightPanel(reticleIndex, false);

            reticleIndex += increment;

            if (reticleIndex < 0) reticleIndex = targetPositions.Length - 1;
            reticleIndex = reticleIndex % targetPositions.Length;

            //ui._enemyStats.HighlightPanel(reticleIndex, true);

            battle._reticleObj.transform.position = targetPositions[reticleIndex] + Vector3.down;

            yield return new WaitForSeconds(cursorDelay);

            movingReticle = false;
        }
    }
}