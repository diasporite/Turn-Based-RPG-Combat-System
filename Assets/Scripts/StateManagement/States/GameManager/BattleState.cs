namespace RPG_Project
{
    public class BattleState : IState
    {
        GameManager game;
        StateMachine gsm;

        BattleManager battle;

        public BattleManager _battle => battle;

        public BattleState(GameManager game)
        {
            this.game = game;
            gsm = game._sm;

            battle = game._battle;

            battle.InitSM();
        }

        #region InterfaceMethods
        public void Enter(params object[] args)
        {
            battle._sm.ChangeState(StateID.BattleStart);
        }

        public void ExecutePerFrame()
        {
            battle.UpdateSM();
        }

        public void Exit()
        {

        }
        #endregion
    }
}