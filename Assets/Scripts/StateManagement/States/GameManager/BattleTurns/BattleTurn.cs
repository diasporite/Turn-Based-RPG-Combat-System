namespace RPG_Project
{
    public class BattleTurn
    {
        protected BattleManager battle;
        protected StateMachine turn_sm;

        protected UIManager ui;

        protected BattleChar combatant;

        protected Command command;

        protected StateMachine sm = new StateMachine();

        public BattleManager _battle => battle;
        public UIManager _ui => ui;

        public BattleChar _combatant => combatant;

        public Command _command
        {
            get => command;
            set => command = value;
        }

        public StateMachine _sm => sm;

        public BattleTurn(BattleManager battle, BattleChar combatant)
        {
            this.battle = battle;
            turn_sm = battle._sm;

            ui = GameManager.instance._ui;

            this.combatant = combatant;

            InitSM();
        }

        protected virtual void InitSM()
        {

        }
    }
}