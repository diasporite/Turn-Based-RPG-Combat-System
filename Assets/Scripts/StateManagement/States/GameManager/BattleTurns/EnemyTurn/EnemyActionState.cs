using System.Collections;

namespace RPG_Project
{
    public class EnemyActionState : IState
    {
        BattleManager battle;
        EnemyTurn turn;
        StateMachine sm;

        BattleChar combatant;

        float autoDelay = 0.25f;
        float delay = 0.5f;

        public EnemyActionState(EnemyTurn turn)
        {
            this.turn = turn;
            battle = turn._battle;
            sm = turn._sm;

            combatant = turn._combatant;

            delay = battle._delay;
            autoDelay = 0.5f * delay;
        }

        #region InterfaceMethods
        public void Enter(params object[] args)
        {
            battle.ToggleAutoMode();

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