using Entitas;

[Input]
public class TriggerFireComponent : IComponent
{
    public int PlanetId;
}

[Input]
public class CollisionComponent : IComponent
{
 public int ProjectileId;
 public int ProjectileLauncherId;
 public int TargetId;
}
 
[Input]
public class SwitchGamePlayStateComponent : IComponent
{
}