namespace UniversityGameProject.Game;

public abstract class Entity
{
    public abstract float Speed { get; set; }
    public abstract int MaxHealth { get; set; }
    public abstract int CurrentHealth { get; set; }
}