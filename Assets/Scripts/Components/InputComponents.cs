using Entitas;

[Input]
public class TriggerFireComponent : IComponent
{
    public int PlaneId;
}

[Input]
public class CollisionComponent : IComponent
{
    public int ProjectileId;
    public int ProjectileLauncherId;
    public int TargetId;
}