namespace RPG_Project
{
    public interface IState
    {
        void Enter(params object[] args);
        void ExecutePerFrame();
        void Exit();
    }
}