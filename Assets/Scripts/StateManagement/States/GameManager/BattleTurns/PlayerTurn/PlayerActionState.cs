using System.Collections;

namespace RPG_Project
{
    public class PlayerActionState : IState
    {
        BattleManager battle;
        PlayerTurn turn;
        StateMachine sm;

        BattleChar combatant;

        float delay = 0.5f;

        public PlayerActionState(PlayerTurn turn)
        {
            this.turn = turn;
            battle = turn._battle;
            sm = turn._sm;

            combatant = turn._combatant;

            delay = battle._delay;
        }

        #region InterfaceMethods
        public void Enter(params object[] args)
        {
            battle.StartCoroutine(ActCo());
        }

        public void ExecutePerFrame()
        {
            battle.ToggleAutoMode();
        }

        public void Exit()
        {

        }
        #endregion

        IEnumerator ActCo()
        {
            yield return battle.StartCoroutine(turn._command.ExecuteCo());

            battle.AdvanceTurn();
        }
    }
}