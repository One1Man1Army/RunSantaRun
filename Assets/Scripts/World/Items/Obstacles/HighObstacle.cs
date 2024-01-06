namespace RSR.World
{
    public sealed class HighObstacle : Obstacle, IInteractable
    {
        public override ObstacleType Type => ObstacleType.High;
    }
}