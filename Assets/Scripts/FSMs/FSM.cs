using System.Collections.Generic;
using UnityEngine;

namespace FSMs
{
    public class FSM
    {
        public enum ClientStates
        {
            Spawn,
            Pidiendo,
            Comiendo
        }

        Dictionary<ClientStates, IState> _states = new Dictionary<ClientStates, IState>();

        IState _currentState;

        public void CreateState(ClientStates newState, IState state)
        {
            if (!_states.ContainsKey(newState))
            {
                _states.Add(newState, state);
            }
        }

        public void ExitState()
        {
            _currentState.OnExit();
        }

        public void ChangeState(ClientStates state)
        {
            if (_states.ContainsKey(state))
            {
                if (_currentState != null)
                {
                    Debug.Log("entre");
                    _currentState.OnExit();
                    _currentState = _states[state];
                    _currentState.OnEnter();
                }
                else
                {
                    _currentState = _states[state];
                    _currentState.OnEnter();
                }
            }
        }

        public void ArtificialUpdate()
        {
            if (_currentState != null)
            {
                _currentState.OnUpdate();
            }
        }
    }
}
