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
public class NewSaveComponent : IComponent
{
    public string SaveName;
}

[Input]
public class LoadComponent : IComponent
{
    public string LoadName;
}
 
[Input]
public class SwitchGamePlayStateComponent : IComponent
{
}