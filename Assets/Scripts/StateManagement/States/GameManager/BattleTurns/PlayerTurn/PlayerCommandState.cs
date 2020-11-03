using UnityEngine;

namespace RPG_Project
{
    public class PlayerCommandState : IState
    {
        BattleManager battle;
        PlayerTurn turn;
        StateMachine sm;

        UIManager ui;

        BattleChar combatant;

        public PlayerCommandState(PlayerTurn turn)
        {
            this.turn = turn;
            battle = turn._battle;
            sm = turn._sm;

            ui = turn._ui;

            combatant = turn._combatant;
        }

        #region InterfaceMethods
        public void Enter(params object[] args)
        {
            if (!battle._autoMode)
            {
                ui._commandPanel.SetActive(true);
                ui.LogMessage("What will " + combatant._charName + " do?");
            }
        }

        public void ExecutePerFrame()
        {
            battle.ToggleAutoMode();

            if (battle._autoMode) AutoCommand();
            else AcceptCommand();
        }

        public void Exit()
        {
            ui._commandPanel.SetActive(false);
        }
        #endregion

        void AutoCommand()
        {
            turn._command = battle._regularAttack.GetCommand(turn._combatant);

            BattleChar target = battle._activeEnemies[Random.Range(0, battle._activeEnemies.Length)];
            turn._command._targets = new BattleChar[] { target };

            sm.ChangeState(StateID.PlayerTurnAction);
        }

        void AcceptCommand()
        {
            if (battle._attack)
            {
                // Set attack action
                turn._command = battle._regularAttack.GetCommand(turn._combatant);
                turn._targets = battle._activeEnemies;

                sm.ChangeState(StateID.PlayerTurnTarget);
            }
            else if (battle._skill)
            {
                sm.ChangeState(StateID.PlayerTurnSkillSelect);
            }
            else if (battle._switch)
            {
                sm.ChangeState(StateID.PlayerTurnSwitchSelect);
            }
            else if (battle._item)
            {
                sm.ChangeState(StateID.PlayerTurnItemSelect);
            }
            else if (battle._analyse)
            {
                sm.ChangeState(StateID.PlayerTurnAnalyse);
            }
            else if (battle._run)
            {
                battle._sm.ChangeState(StateID.BattleRun);
            }
        }
    }
}