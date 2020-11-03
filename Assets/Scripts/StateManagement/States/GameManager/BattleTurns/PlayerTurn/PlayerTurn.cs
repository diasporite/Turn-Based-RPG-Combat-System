namespace RPG_Project
{
    public class PlayerTurn : BattleTurn, IState
    {
        BattleChar[] targets;

        public BattleChar[] _targets
        {
            get => targets;
            set => targets = value;
        }

        public PlayerTurn(BattleManager battle, BattleChar combatant) : base(battle, combatant)
        {

        }

        #region InterfaceMethods
        public void Enter(params object[] args)
        {
            ui._partyStats.HighlightPanel(combatant, true);

            sm.ChangeState(StateID.PlayerTurnCommand);
        }

        public void ExecutePerFrame()
        {
            sm.Update();
        }

        public void Exit()
        {
            ui._partyStats.HighlightPanel(combatant, false);
        }
        #endregion

        protected override void InitSM()
        {
            sm.AddState(StateID.PlayerTurnCommand, new PlayerCommandState(this));
            sm.AddState(StateID.PlayerTurnSkillSelect, new PlayerSkillSelectState(this));
            sm.AddState(StateID.PlayerTurnItemSelect, new PlayerItemSelectState(this));
            sm.AddState(StateID.PlayerTurnSwitchSelect, new PlayerSwitchSelectState(this));
            sm.AddState(StateID.PlayerTurnTarget, new PlayerTargetState(this));
            sm.AddState(StateID.PlayerTurnAnalyse, new PlayerAnalyseState(this));
            sm.AddState(StateID.PlayerTurnAction, new PlayerActionState(this));
        }
    }
}