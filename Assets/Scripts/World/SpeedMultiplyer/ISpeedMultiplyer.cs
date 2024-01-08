namespace RSR.World
{
    public interface ISpeedMultiplyer
    {
        float Default { get; }
        float Current { get; }
        float Acceleration { get; }
        void Boost(float multiplyer, float duration);
    }
}