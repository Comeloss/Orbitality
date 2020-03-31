using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;

[Game]
public class MassComponent : IComponent
{
    public int Value;
}

[Game]
public class PositionComponent : IComponent
{
    public Vector2 Position;
}

[Game]
public class SpeedComponent : IComponent
{
    public float Speed;
}

[Game]
public class PlanetInfoComponent : IComponent
{
    public string Name;
    public int Id;
    public Sprite Image;
    public float Size;
}

[Game]
public class ForceComponent : IComponent
{
    public Vector2 Force;
}

[Game, Unique]
public class GameTimeComponent : IComponent
{
    public long Time;
}

[Game, Unique]
public class GameTimeMillisecondsComponent : IComponent
{
    public float Time;
}

[Game, Unique]
public class GameDeltaTimeComponent : IComponent
{
    public float TimeDelta { get; set; }
}

[Game, Unique]
public class GravitationalConstantComponent : IComponent
{
    public float GRAV; 
}

[Game]
public class OrbitalMagnitudeComponent : IComponent
{
    public float Magnitude; 
}

[Game]
public class HealthComponent : IComponent
{
    public int CurrentHp;
    public int TotalHp;
    public float Proportion => (float)CurrentHp / TotalHp;
}

[Game]
public class VelocityComponent : IComponent
{
    public Vector2 Velocity; 
}

[Game]
public class StartVelocityComponent : IComponent
{
    public Vector2 Velocity; 
}

[Game]
public class GravityVelocityComponent : IComponent
{
    public Vector2 Velocity; 
}

[Game]
public class PlanetTrajectoryComponent : IComponent
{
    public int OrbitWidthA; 
    public int OrbitHeightB;
    public int OrbitCenterX; 
    public int OrbitCenterY;
    public float Speed;
}

public enum ProjectileCannonType
{
    None,
    Heavy,
    Frequent,
    Fast
}

[Game]
public class ProjectileCannonTypeComponent : IComponent
{
    public ProjectileCannonType Type;
}

[Game]
public class ProjectileCannonComponent : IComponent
{
    public int CannonId;
    public int ProjectileMass;
    public int ProjectileDamage;
    public float ProjectileSpeed;
    public float ProjectileCooldown;
    public float ProjectileMaxLifeTime; 
    public float ProjectileSize;
    public Color ProjectileColour;
}

[Game]
public class ProjectileComponent : IComponent
{
    public int PlanetId;
    public int ProjectileId;
    public int Damage;
}

[Game]
public class ProjectilesTotalShootComponent : IComponent
{
    public int Count;
}

[Game]
public class LifeTimeComponent : IComponent
{
    public float TimeLeft;
    public bool TimeEnded => TimeLeft <= 0;
}

[Game, Unique]
public class PlayerComponent : IComponent
{
    public int PlaneId;
}

[Game]
public class CooldownComponent : IComponent
{
    public float Cooldown;
    public float TimeLeft;
    public bool IsCoolingDown;
    public float Proportion => TimeLeft / Cooldown;
}

[Game, Unique]
public class GamePlayStateComponent : IComponent
{
    public enum GamePlayStateType
    {
        Play,
        Pause
    }

    public GamePlayStateType CurrentState;
}

[Game]
public class ReadyToDestroyComponent : IComponent
{
}
