namespace RSR.World
{
    public sealed class LowObstacle : Obstacle, IInteractable
    {
        public override ObstacleType Type => ObstacleType.Low;
    }
}