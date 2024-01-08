namespace RSR.World
{
    public interface ISpeedMultiplyer
    {
        float Default { get; }
        float Current { get; }
        void Boost(float multiplyer, float duration);
    }
}