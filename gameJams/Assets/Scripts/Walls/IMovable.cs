namespace MyGame.Walls
{
    /// <summary>
    /// Duvar veya platform gibi hareket eden objeler i�in temel aray�z.
    /// </summary>
    public interface IMovable
    {
        void MoveToTarget();
        void MoveToInitial();
    }
}
