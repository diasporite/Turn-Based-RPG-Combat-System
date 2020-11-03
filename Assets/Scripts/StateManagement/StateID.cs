namespace RPG_Project
{
    public enum StateID
    {
        Empty = 0,

        GameOverworld = 5,
        GameBattle = 10,

        BattleStart = 50,
        BattleSwitch = 55,
        BattleRun = 60,
        BattleWin = 65,
        BattleLose = 70,

        PlayerTurnCommand = 100,
        PlayerTurnSkillSelect = 105,
        PlayerTurnItemSelect = 110,
        PlayerTurnSwitchSelect = 115,
        PlayerTurnTarget = 120,
        PlayerTurnAnalyse = 125,
        PlayerTurnAction = 130,

        EnemyTurnDecide = 150,
        EnemyTurnAction = 155,
    }
}