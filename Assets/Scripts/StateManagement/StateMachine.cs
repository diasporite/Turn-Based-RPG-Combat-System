using System.Collections.Generic;

namespace RPG_Project
{
    public class StateMachine
    {
        IState currentState = new EmptyState();
        Dictionary<object, IState> states = new Dictionary<object, IState>();

        public IState _currentState => currentState;
        public Dictionary<object, IState> _states => states;
        public int _stateCount => states.Count;

        public IState GetState(object id)
        {
            if (states.ContainsKey(id))
            {
                return states[id];
            }
            
            return null;
        }

        public void Update()
        {
            if (currentState != null)
            {
                currentState.ExecutePerFrame();
            }
        }

        public void AddState(object id, IState state)
        {
            if (!states.ContainsKey(id))
            {
                states.Add(id, state);
            }
        }

        public void ChangeState(object id, params object[] args)
        {
            if (currentState != null) currentState.Exit();

            if (states.ContainsKey(id))
            {
                currentState = states[id];
                currentState.Enter(args);
            }
        }

        public void ClearStates()
        {
            states.Clear();
        }

        public void RemoveState(object id)
        {
            if (states.ContainsKey(id))
            {
                states.Remove(id);
            }
        }
    }
}