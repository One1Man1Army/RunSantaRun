namespace RSR.Player
{
    public interface IPlayerSpeedMultiplyer
    {
        float Default { get; }
        float Current { get; }
        float SpeedUp { get; }
        void Boost(float multiplyer, float duration);
    }
}