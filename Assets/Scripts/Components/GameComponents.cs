using Entitas;
using Entitas.CodeGeneration.Attributes;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

[Game]
public class MassComponent : IComponent
{
    public int Value;
    
    public static explicit operator MassSaveData(MassComponent massComponent)
    {
        return new MassSaveData{Value = massComponent.Value};
    }
}

[System.Serializable]
 public class Vector2Ser
 {
     public float x,y;
     public static explicit operator Vector2Ser(Vector2 vector)
     {
         return new Vector2Ser
         {
             x = vector.x,
             y = vector.y
         };
     }
     
     public static explicit operator Vector2(Vector2Ser vector)
     {
         return new Vector2
         {
             x = vector.x,
             y = vector.y
         };
     }
 }

[System.Serializable]
public class ColourSer
{
    public float r,g,b,a;
    public static explicit operator ColourSer(Color col)
    {
        return new ColourSer
        {
            r = col.r,
            g = col.g,
            b = col.b,
            a = col.a
        };
    }
    
    public static explicit operator Color(ColourSer col)
    {
        return new Color
        {
            r = col.r,
            g = col.g,
            b = col.b,
            a = col.a
        };
    }
}

[Game]
public class PositionComponent : IComponent
{
    public Vector2 Position;
    public static explicit operator ObjectPositionSaveData(PositionComponent positionComponent)
    {
        return new ObjectPositionSaveData{Position = (Vector2Ser)positionComponent.Position};
    }
}

[Game]
public class SpeedComponent : IComponent
{
    public float Speed;
    
    public static explicit operator SpeedSaveData(SpeedComponent speedComponent)
    {
        return new SpeedSaveData{SpeedValue = speedComponent.Speed};
    }
}

[Game]
public class PlanetInfoComponent : IComponent
{
    public string Name;
    public int Id;
    public string SpriteName;
    public float Size;
    
    public static explicit operator PlanetInfoSaveData(PlanetInfoComponent planetInfoComponent)
    {
        return new PlanetInfoSaveData
        {
            Name = planetInfoComponent.Name,
            Id = planetInfoComponent.Id,
            ImagePath = planetInfoComponent.SpriteName,
            Size = planetInfoComponent.Size
        };
    }
}

[Game]
public class ForceComponent : IComponent
{
    public Vector2 Force;
    
    public static explicit operator ForceSaveData(ForceComponent forceComponent)
    {
        return new ForceSaveData{ForceValue = (Vector2Ser)forceComponent.Force};
    }
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
public class HealthComponent : IComponent
{
    public int CurrentHp;
    public int TotalHp;
    public float Proportion => (float)CurrentHp / TotalHp;
    
    public static explicit operator HealthSaveData(HealthComponent healthComponent)
    {
        return new HealthSaveData
        {
            CurrentHp = healthComponent.CurrentHp,
            TotalHp = healthComponent.TotalHp
        };
    }
}

[Game]
public class VelocityComponent : IComponent
{
    public Vector2 Velocity; 
    
    public static explicit operator VelocitySaveData(VelocityComponent velocityComponent)
    {
        return new VelocitySaveData{VelocityValue = (Vector2Ser)velocityComponent.Velocity};
    }
}

[Game]
public class StartVelocityComponent : IComponent
{
    public Vector2 Velocity; 
    
    public static explicit operator StartVelocitySaveData(StartVelocityComponent startVelocityComponent)
    {
        return new StartVelocitySaveData{Velocity = (Vector2Ser)startVelocityComponent.Velocity};
    }
}

[Game]
public class GravityVelocityComponent : IComponent
{
    public Vector2 Velocity; 
    
    public static explicit operator GravityVelocitySaveData(GravityVelocityComponent gravityVelocityComponent)
    {
        return new GravityVelocitySaveData{Velocity = (Vector2Ser)gravityVelocityComponent.Velocity};
    }
}

[Game]
public class PlanetTrajectoryComponent : IComponent
{
    public int OrbitWidthA; 
    public int OrbitHeightB;
    public int OrbitCenterX; 
    public int OrbitCenterY;
    public float Speed;
    
    public static explicit operator PlanetTrajectorySaveData(PlanetTrajectoryComponent planetTrajectoryComponent)
    {
        return new PlanetTrajectorySaveData
        {
            OrbitWidthA = planetTrajectoryComponent.OrbitWidthA,
            OrbitHeightB = planetTrajectoryComponent.OrbitHeightB,
            OrbitCenterX = planetTrajectoryComponent.OrbitCenterX,
            OrbitCenterY = planetTrajectoryComponent.OrbitCenterY,
            Speed = planetTrajectoryComponent.Speed
        };
    }
}

public enum ProjectileCannonType
{
    Heavy,
    Frequent,
    Fast
}

[Game]
public class ProjectileCannonTypeComponent : IComponent
{
    public ProjectileCannonType Type;
    
    public static explicit operator ProjectileTypeSaveData(ProjectileCannonTypeComponent projectileCannonTypeComponent)
    {
        return new ProjectileTypeSaveData{Type = projectileCannonTypeComponent.Type};
    }
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
    
    public static explicit operator ProjectileCannonSaveData(ProjectileCannonComponent projectileCannonTypeComponent)
    {
        return new ProjectileCannonSaveData
        {
            CannonId = projectileCannonTypeComponent.CannonId,
            ProjectileMass = projectileCannonTypeComponent.ProjectileMass,
            ProjectileDamage = projectileCannonTypeComponent.ProjectileDamage,
            ProjectileSpeed = projectileCannonTypeComponent.ProjectileSpeed,
            ProjectileCooldown = projectileCannonTypeComponent.ProjectileCooldown,
            ProjectileMaxLifeTime = projectileCannonTypeComponent.ProjectileMaxLifeTime,
            ProjectileSize = projectileCannonTypeComponent.ProjectileSize,
            ProjectileColour = (ColourSer)projectileCannonTypeComponent.ProjectileColour
        };
    }
}

[Game]
public class ProjectileComponent : IComponent
{
    public int PlanetId;
    public int ProjectileId;
    public int Damage;
    
    public static explicit operator ProjectileSaveData(ProjectileComponent projectileComponent)
    {
        return new ProjectileSaveData
        {
            PlanetId = projectileComponent.PlanetId,
            ProjectileId = projectileComponent.ProjectileId,
            Damage = projectileComponent.Damage
        };
    }
}

[Game]
public class ProjectilesTotalShootComponent : IComponent
{
    public int Count;
    
    public static explicit operator ProjectilesTotalShootSaveData(ProjectilesTotalShootComponent projectilesTotalShootComponent)
    {
        return new ProjectilesTotalShootSaveData
        {
            Count = projectilesTotalShootComponent.Count
        };
    }
}

[Game]
public class LifeTimeComponent : IComponent
{
    public float TimeLeft;
    public bool TimeEnded => TimeLeft <= 0;
    
    public static explicit operator LifeTimeSaveData(LifeTimeComponent lifeTimeComponent)
    {
        return new LifeTimeSaveData
        {
            TimeLeft = lifeTimeComponent.TimeLeft
        };
    }
}

[Game, Unique]
public class PlayerComponent : IComponent
{
    public int PlanetId;
    public string PlayerName;
    
    public static explicit operator PlayerSaveData(PlayerComponent playerComponent)
    {
        return new PlayerSaveData
        {
            PlanetId = playerComponent.PlanetId,
            PlayerName = playerComponent.PlayerName,
        };
    }
}

[Game]
public class CooldownComponent : IComponent
{
    public float Cooldown;
    public float TimeLeft;
    public bool IsCoolingDown;
    public float Proportion => TimeLeft / Cooldown;
    
    public static explicit operator CooldownSaveData(CooldownComponent cooldownComponent)
    {
        return new CooldownSaveData
        {
            CooldownValue = cooldownComponent.Cooldown,
            TimeLeft = cooldownComponent.TimeLeft,
            IsCoolingDown = cooldownComponent.IsCoolingDown
        };
    }
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
public class BotAiComponent : IComponent
{
    public int ReactionTicks;
    public int CurrentTick;

    public bool CanAct => ReactionTicks == CurrentTick;
    
    public static explicit operator BotAiSaveData(BotAiComponent botAiComponent)
    {
        return new BotAiSaveData
        {
            ReactionTicks = botAiComponent.ReactionTicks,
            CurrentTick = botAiComponent.CurrentTick,
        };
    }
}

[Game, Unique]
public class BotsClockComponent : IComponent
{
    public float LastTick;
    public float TickDuration;
    
    public static explicit operator BotsClockSaveData(BotsClockComponent botsClockComponent)
    {
        return new BotsClockSaveData
        {
            LastTick = botsClockComponent.LastTick,
            TickDuration = botsClockComponent.TickDuration,
        };
    }
}

[Game]
public class ReadyToDestroyComponent : IComponent
{
}
