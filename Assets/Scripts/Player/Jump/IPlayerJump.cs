namespace RSR.Player
{
    public interface IPlayerJump
    {
        void Fly(float duration, float height, float amplitude);
        void Jump();
    }
}