using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization.Formatters.Binary;
using Entitas;
using UniRx;
using UnityEngine;

namespace Features.SaveFeature
{
    public class LoadSystem : ReactiveSystem<InputEntity>
    {
        private readonly Contexts _contexts;

        public LoadSystem(Contexts context) : base(context.input)
        {
            _contexts = context;
        }

        protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
        {
            return context.CreateCollector(InputMatcher.Load);
        }

        protected override bool Filter(InputEntity entity)
        {
            return entity.hasLoad;
        }

        protected override void Execute(List<InputEntity> entities)
        {
            var name = "";

            foreach (var inputEntity in entities)
            {
                name = inputEntity.load.LoadName;
            }

            if (string.IsNullOrEmpty(name))
            {
                Debug.LogWarning("Load name is empty!");
                return;
            }

            var filePath = $"{Application.persistentDataPath}/{name}.save";
            
            if (!File.Exists(filePath))
            {
                Debug.LogWarningFormat("Save {0} was not found!", name);
                return;
            }
            
            _contexts.game.CleanUp();

            Observable.Timer(TimeSpan.FromTicks(1)).Subscribe((_) =>
            {
                var bf = new BinaryFormatter();
                var file = File.Open(filePath, FileMode.Open);
                var save = (Save)bf.Deserialize(file);
                file.Close();
            
                LoadData(save);
            });
        }
        
        private void LoadData(Save save)
        {
            _contexts.game.ReplaceGamePlayState(GamePlayStateComponent.GamePlayStateType.Pause);
            _contexts.game.ReplaceGameTime(save.GameTime);
            _contexts.game.ReplaceGameTimeMilliseconds(save.GameTimeMilliseconds);
            _contexts.game.ReplaceGameDeltaTime(save.GameDeltaTime);
            _contexts.game.ReplaceGravitationalConstant(save.GravitationalConstant);

            LoadPlanets(save);
            
            LoadProjectiles(save);
        }

        private void LoadProjectiles(Save save)
        {
            if (save.Projectiles == null)
            {
                return;
            }

            foreach (var projectileSaveData in save.Projectiles)
            {
                var projectileEntity = _contexts.game.CreateEntity();
                projectileEntity.ReplaceProjectile(projectileSaveData.PlanetId, projectileSaveData.ProjectileId,
                    projectileSaveData.Damage);

                if (projectileSaveData.MassSave != null)
                {
                    projectileEntity.ReplaceMass(projectileSaveData.MassSave.Value);
                }

                if (projectileSaveData.GravityVelocitySave != null)
                {
                    projectileEntity.ReplaceGravityVelocity((UnityEngine.Vector2)projectileSaveData.GravityVelocitySave.Velocity);
                }

                if (projectileSaveData.LifeTimeSave != null)
                {
                    projectileEntity.ReplaceLifeTime(projectileSaveData.LifeTimeSave.TimeLeft);
                }

                if (projectileSaveData.PositionSave != null)
                {
                    projectileEntity.ReplacePosition((UnityEngine.Vector2)projectileSaveData.PositionSave.Position);
                }

                if (projectileSaveData.StartVelocitySave != null)
                {
                    projectileEntity.ReplaceStartVelocity((UnityEngine.Vector2) projectileSaveData.StartVelocitySave.Velocity);
                }
                
                if (projectileSaveData.VelocitySave != null)
                {
                    projectileEntity.ReplaceVelocity((UnityEngine.Vector2) projectileSaveData.VelocitySave.VelocityValue);
                }
                
                if (projectileSaveData.ForceSave != null)
                {
                    projectileEntity.ReplaceForce((UnityEngine.Vector2) projectileSaveData.ForceSave.ForceValue);
                }
            }
        }

        private void LoadPlanets(Save save)
        {
            if (save.Planets == null)
            {
                return;
            }

            foreach (var planetInfoSaveData in save.Planets)
            {
                var planetEntity = _contexts.game.CreateEntity();
                planetEntity.ReplacePlanetInfo(planetInfoSaveData.Name, planetInfoSaveData.Id, planetInfoSaveData.ImagePath, planetInfoSaveData.Size);

                if (planetInfoSaveData.MassSave != null)
                {
                    planetEntity.ReplaceMass(planetInfoSaveData.MassSave.Value);
                }
                
                if (planetInfoSaveData.BotAiSave != null)
                {
                    planetEntity.ReplaceBotAi(planetInfoSaveData.BotAiSave.CurrentTick, planetInfoSaveData.BotAiSave.ReactionTicks);
                }
                
                if (planetInfoSaveData.HealthSave != null)
                {
                    planetEntity.ReplaceHealth(planetInfoSaveData.HealthSave.CurrentHp, planetInfoSaveData.HealthSave.TotalHp);
                }
                
                if (planetInfoSaveData.PlanetTrajectorySave != null)
                {
                    planetEntity.ReplacePlanetTrajectory(
                        planetInfoSaveData.PlanetTrajectorySave.OrbitWidthA, 
                        planetInfoSaveData.PlanetTrajectorySave.OrbitHeightB, 
                        planetInfoSaveData.PlanetTrajectorySave.OrbitCenterX, 
                        planetInfoSaveData.PlanetTrajectorySave.OrbitCenterY, 
                        planetInfoSaveData.PlanetTrajectorySave.Speed);
                }
                
                if (planetInfoSaveData.ObjectPositionSave != null)
                {
                    planetEntity.ReplacePosition((UnityEngine.Vector2)planetInfoSaveData.ObjectPositionSave.Position);
                }
                
                if (planetInfoSaveData.ProjectileCannonSave != null)
                {
                    planetEntity.ReplaceProjectileCannon(
                        planetInfoSaveData.ProjectileCannonSave.CannonId, 
                        planetInfoSaveData.ProjectileCannonSave.ProjectileMass,
                        planetInfoSaveData.ProjectileCannonSave.ProjectileDamage,
                        planetInfoSaveData.ProjectileCannonSave.ProjectileSpeed,
                        planetInfoSaveData.ProjectileCannonSave.ProjectileCooldown,
                        planetInfoSaveData.ProjectileCannonSave.ProjectileMaxLifeTime,
                        planetInfoSaveData.ProjectileCannonSave.ProjectileSize,
                        (Color)planetInfoSaveData.ProjectileCannonSave.ProjectileColour);
                }
                
                if (planetInfoSaveData.ProjectileTypeSave != null)
                {
                    planetEntity.ReplaceProjectileCannonType(planetInfoSaveData.ProjectileTypeSave.Type);
                }
                
                if (planetInfoSaveData.StartVelocitySave != null)
                {
                    planetEntity.ReplaceStartVelocity((UnityEngine.Vector2)planetInfoSaveData.StartVelocitySave.Velocity);
                }
                
                if (planetInfoSaveData.VelocitySave != null)
                {
                    planetEntity.ReplaceVelocity((UnityEngine.Vector2)planetInfoSaveData.VelocitySave.VelocityValue);
                }
                
                if (planetInfoSaveData.CooldownSave != null)
                {
                    planetEntity.ReplaceCooldown(planetInfoSaveData.CooldownSave.CooldownValue, planetInfoSaveData.CooldownSave.TimeLeft, planetInfoSaveData.CooldownSave.IsCoolingDown);
                }
                
                if (planetInfoSaveData.ProjectilesTotalShootSave != null)
                {
                    planetEntity.ReplaceProjectilesTotalShoot(planetInfoSaveData.ProjectilesTotalShootSave.Count);
                }
                
                if (planetInfoSaveData.PlayerSave != null)
                {
                    planetEntity.ReplacePlayer(planetInfoSaveData.PlayerSave.PlanetId, planetInfoSaveData.PlayerSave.PlayerName);
                }
            }
        }
    }
}

