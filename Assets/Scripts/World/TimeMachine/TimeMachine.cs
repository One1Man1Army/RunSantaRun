using RSR.ServicesLogic;
using RSR.World;
using UnityEngine;

public sealed class TimeMachine : MonoBehaviour, ITimeMachine
{
    public float Timge { get; private set; }

    private IWorldStarter _worldStarter;

    private float _addPerFrame;
    private bool _isSpeedingUp;

    public void Construct(IGameSettingsProvider gameSettingsProvider, IWorldStarter worldStarter)
    {
        _worldStarter = worldStarter;
        _addPerFrame  = gameSettingsProvider.GameSettings.timeScaleAddPerFrame;

        _worldStarter.OnReady += ResetTime;
        _worldStarter.OnStart += StartSpeedingUp;
    }

    private void Update()
    {
        if (_isSpeedingUp)
        {
            Time.timeScale += _addPerFrame * Time.deltaTime;
        }
    }

    private void ResetTime()
    {
        _isSpeedingUp = false;
        Time.timeScale = 1f;
    }

    private void StartSpeedingUp()
    {
        _isSpeedingUp = true;
    }

    private void OnDestroy()
    {
        _worldStarter.OnReady -= ResetTime;
        _worldStarter.OnStart -= StartSpeedingUp;
    }
}
