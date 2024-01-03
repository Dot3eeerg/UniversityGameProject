namespace UniversityGameProject.Game;

public abstract class Entity
{
    public abstract float Speed { get; set; }
    public abstract int MaxHealth { get; set; }
    public abstract int CurrentHealth { get; set; }
    public abstract int Damage { get; set; }
    public abstract long InvulTime { get; set; }
}