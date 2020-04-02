using System;

[Serializable]
public class Save
{
    public long GameTime;
    public float GameTimeMilliseconds;
    public float GameDeltaTime;
    public float GravitationalConstant;

    public PlanetInfoSaveData[] Planets;
    public ProjectileSaveData[] Projectiles;
}

[Serializable]
public class PlanetInfoSaveData
{
    public string Name;
    public int Id;
    public string ImagePath;
    public float Size;

    
    public MassSaveData MassSave;
    public BotAiSaveData BotAiSave;
    public HealthSaveData HealthSave;
    public PlanetTrajectorySaveData PlanetTrajectorySave;
    public ObjectPositionSaveData ObjectPositionSave;
    public ProjectileCannonSaveData ProjectileCannonSave;
    public ProjectileTypeSaveData ProjectileTypeSave;
    public StartVelocitySaveData StartVelocitySave;
    public VelocitySaveData VelocitySave;
    public CooldownSaveData CooldownSave;
    public ProjectilesTotalShootSaveData ProjectilesTotalShootSave;
    public PlayerSaveData PlayerSave;
}

[Serializable]
public class BotAiSaveData
{
    public int ReactionTicks;
    public int CurrentTick;
}

[Serializable]
public class CooldownSaveData
{
    public float CooldownValue;
    public float TimeLeft;
    public bool IsCoolingDown;
}

[Serializable]
public class HealthSaveData
{
    public int CurrentHp;
    public int TotalHp;
}

[Serializable]
public class MassSaveData
{
    public int Value;
}

[Serializable]
public class PlanetTrajectorySaveData
{
    public int OrbitWidthA; 
    public int OrbitHeightB;
    public int OrbitCenterX; 
    public int OrbitCenterY;
    public float Speed;
}

[Serializable]
public class ObjectPositionSaveData
{
    public Vector2Ser Position;
}

[Serializable]
public class ProjectileCannonSaveData
{
    public int CannonId;
    public int ProjectileMass;
    public int ProjectileDamage;
    public float ProjectileSpeed;
    public float ProjectileCooldown;
    public float ProjectileMaxLifeTime; 
    public float ProjectileSize;
    public ColourSer ProjectileColour;
}

[Serializable]
public class ProjectileTypeSaveData
{
    public ProjectileCannonType Type;
}

[Serializable]
public class ProjectilesTotalShootSaveData
{
    public int Count;
}

[Serializable]
public class StartVelocitySaveData
{
    public Vector2Ser Velocity;
}

[Serializable]
public class VelocitySaveData
{
    public Vector2Ser VelocityValue; 
}

[Serializable]
public class SpeedSaveData
{
    public float SpeedValue; 
}

[Serializable]
public class ForceSaveData
{
    public Vector2Ser ForceValue;
}

[Serializable]
public class GravityVelocitySaveData
{
    public Vector2Ser Velocity; 
}

[Serializable]
public class ProjectileSaveData
{
    public int PlanetId;
    public int ProjectileId;
    public int Damage;

    public ForceSaveData ForceSave;
    public GravityVelocitySaveData GravityVelocitySave;
    public LifeTimeSaveData LifeTimeSave;
    public MassSaveData MassSave;
    public ObjectPositionSaveData PositionSave;
    public StartVelocitySaveData StartVelocitySave;
    public VelocitySaveData VelocitySave;
}

[Serializable]
public class LifeTimeSaveData
{
    public float TimeLeft;
}

[Serializable]
public class PlayerSaveData
{
    public int PlanetId;
    public string PlayerName;
}

[Serializable]
public class BotsClockSaveData
{
    public float LastTick;
    public float TickDuration;
}

