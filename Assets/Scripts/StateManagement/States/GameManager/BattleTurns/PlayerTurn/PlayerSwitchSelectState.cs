using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace RPG_Project
{
    public class PlayerSwitchSelectState : IState
    {
        PlayerTurn turn;
        StateMachine sm;

        BattleManager battle;
        UIManager ui;

        BattleChar[] party;
        int partySize;

        public PlayerSwitchSelectState(PlayerTurn turn)
        {
            battle = turn._battle;
            ui = turn._ui;

            this.turn = turn;
            sm = turn._sm;
        }

        #region InterfaceMethods
        public void Enter(params object[] args)
        {
            party = GameManager.instance._party._party;
            partySize = party.Length;

            ui._switchPanel.gameObject.SetActive(true);
            ui._partyAnalysisPanel.gameObject.SetActive(true);

            ui._switchPanel.EnterPanel(party);
        }

        public void ExecutePerFrame()
        {
            NextMember();
            PreviousMember();
            ConfirmMember();
            Back();
        }

        public void Exit()
        {
            ui._switchPanel.gameObject.SetActive(false);
            ui._partyAnalysisPanel.gameObject.SetActive(false);
        }
        #endregion

        void NextMember()
        {
            if (battle._next)
            {
                ui._switchPanel.NextSelection(partySize);
            }
        }

        void PreviousMember()
        {
            if (battle._previous)
            {
                ui._switchPanel.PreviousSelection(partySize);
            }
        }

        void ConfirmMember()
        {
            if (battle._confirm)
            {
                BattleChar newChar = ui._switchPanel._currentPartyMember;

                if (newChar._battleStatus == BattleStatus.Dead || 
                    newChar._battleStatus == BattleStatus.InBattle) return;

                battle._switchNewChar = newChar;
                battle._sm.ChangeState(StateID.BattleSwitch);
            }
        }

        void Back()
        {
            if (battle._backOut)
            {
                sm.ChangeState(StateID.PlayerTurnCommand);
            }
        }
    }
}