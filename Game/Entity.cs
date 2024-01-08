namespace UniversityGameProject.Game;

public abstract class Entity
{
    public abstract float Speed { get; set; }
    public abstract int MaxHealth { get; set; }
    public abstract int CurrentHealth { get; set; }
}

public abstract class EntityPlayer : Entity
{
    public abstract long InvulTime { get; set; }
    public abstract uint ExpToLevel { get; set; }
    public abstract uint CurrentExp { get; set; }
    public abstract float DamageReduction { get; set; }
    public abstract int HpRegen { get; set; }
}

public abstract class EntityEnemy : Entity
{
    public abstract int Damage { get; set; }
    public abstract uint Exp { get; set; }
}