namespace RPG_Project
{
    public class EnemyTurn : BattleTurn, IState
    {
        public EnemyTurn(BattleManager battle, BattleChar combatant) : base(battle, combatant)
        {

        }

        #region InterfaceMethods
        public void Enter(params object[] args)
        {
            ui._enemyStats.HighlightPanel(combatant, true);

            sm.ChangeState(StateID.EnemyTurnDecide);
        }

        public void ExecutePerFrame()
        {
            sm.Update();
        }

        public void Exit()
        {
            ui._enemyStats.HighlightPanel(combatant, false);
        }
        #endregion

        protected override void InitSM()
        {
            sm.AddState(StateID.EnemyTurnDecide, new EnemyDecideState(this));
            sm.AddState(StateID.EnemyTurnAction, new EnemyActionState(this));
        }
    }
}