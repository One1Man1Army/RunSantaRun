using UnityEngine;

namespace RSR.InternalLogic
{
    /// <summary>
    /// Entry point to the game. 
    /// </summary>
    public sealed class GameEntryPoint : MonoBehaviour
    {
        private static GameEntryPoint _instance;
        private GameStateMachine _gameStateMachine;

        private void Awake()
        {
            IntsantiateAsSingle();
            InitGameStateMachine();
        }

        private void InitGameStateMachine()
        {
            _gameStateMachine = new GameStateMachine();
            _gameStateMachine.Enter<InitState>();
        }

        private void IntsantiateAsSingle()
        {
            if (_instance != null && _instance != this)
            {
                Destroy(this);
            }
            else
            {
                _instance = this;
                DontDestroyOnLoad(this);
            }
        }
    }
}
