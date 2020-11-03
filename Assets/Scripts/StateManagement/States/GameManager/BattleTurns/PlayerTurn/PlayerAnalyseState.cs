namespace RPG_Project
{
    public class PlayerAnalyseState : IState
    {
        PlayerTurn turn;
        StateMachine sm;

        BattleManager battle;
        UIManager ui;

        int currentAnalysis = 0;

        public PlayerAnalyseState(PlayerTurn turn)
        {
            this.turn = turn;
            sm = turn._sm;

            battle = GameManager.instance._battle;
            ui = GameManager.instance._ui;
        }

        #region InterfaceMethods
        public void Enter(params object[] args)
        {
            ui._battlerAnalysisPanel.gameObject.SetActive(true);

            currentAnalysis = battle.GetBattlerIndex(battle._currentBattler);
            ui._battlerAnalysisPanel.AnalyseBattler(battle._currentBattler);
        }

        public void ExecutePerFrame()
        {
            NextBattler();
            PreviousBattler();
            Back();
        }

        public void Exit()
        {
            ui._battlerAnalysisPanel.gameObject.SetActive(false);
        }
        #endregion

        void NextBattler()
        {
            if (battle._next)
            {
                currentAnalysis++;
                ui._battlerAnalysisPanel.AnalyseBattler(battle.GetActiveBattler(currentAnalysis));
            }
        }

        void PreviousBattler()
        {
            if (battle._previous)
            {
                currentAnalysis--;
                ui._battlerAnalysisPanel.AnalyseBattler(battle.GetActiveBattler(currentAnalysis));
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