using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Formatters.Binary;
using Entitas;
using UnityEngine;

namespace Features.SaveFeature
{
    public class SaveSystem : ReactiveSystem<InputEntity>
    {
        private readonly Contexts _contexts;

        public SaveSystem(Contexts context) : base(context.input)
        {
            _contexts = context;
        }

        protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
        {
            return context.CreateCollector(InputMatcher.NewSave);
        }

        protected override bool Filter(InputEntity entity)
        {
            return entity.hasNewSave;
        }

        protected override void Execute(List<InputEntity> entities)
        {
            var name = "";

            foreach (var inputEntity in entities)
            {
                name = inputEntity.newSave.SaveName;
            }

            if (string.IsNullOrEmpty(name))
            {
                Debug.LogWarning("Save name is empty!");
                return;
            }

            var save = new Save();

            SaveGameTime(save);
            SaveGameTimeMilliseconds(save);
            SaveGameDeltaTime(save);
            SaveGravitationalConstant(save);
            SavePlanets(save);
            SaveProjectiles(save);
           
            var bf = new BinaryFormatter();
            var file = File.Create($"{Application.persistentDataPath}/{name}.save");
            bf.Serialize(file, save);
            file.Close();
        }

        private void SaveGameTime(Save save)
        {
            if (_contexts.game.hasGameTime)
            {
                save.GameTime = _contexts.game.gameTime.Time;
            }
        }

        private void SaveGameTimeMilliseconds(Save save)
        {
            if (_contexts.game.hasGameTimeMilliseconds)
            {
                save.GameTimeMilliseconds = _contexts.game.gameTimeMilliseconds.Time;
            }
        }

        private void SaveGameDeltaTime(Save save)
        {
            if (_contexts.game.hasGameDeltaTime)
            {
                save.GameDeltaTime = _contexts.game.gameDeltaTime.TimeDelta;
            }
        }
        
        private void SaveGravitationalConstant(Save save)
        {
            if (_contexts.game.hasGravitationalConstant)
            {
                save.GravitationalConstant = _contexts.game.gravitationalConstant.GRAV;
            }
        }

        private void SavePlanets(Save save)
        {
            var planets = _contexts.game.GetGroup(GameMatcher.PlanetInfo).ToArray();

            save.Planets = new PlanetInfoSaveData[planets.Length];

            for (var i = 0; i < planets.Length; i++)
            {
                var planetEntity = planets[i];
                var planetInfo = (PlanetInfoSaveData) planetEntity.planetInfo;

                planetInfo.MassSave = planetEntity.hasMass ? (MassSaveData) planetEntity.mass : null;
                planetInfo.BotAiSave = planetEntity.hasBotAi ? (BotAiSaveData) planetEntity.botAi : null;
                planetInfo.HealthSave = planetEntity.hasHealth ? (HealthSaveData) planetEntity.health : null;
                planetInfo.PlanetTrajectorySave = planetEntity.hasPlanetTrajectory
                    ? (PlanetTrajectorySaveData) planetEntity.planetTrajectory
                    : null;
                planetInfo.ObjectPositionSave =
                    planetEntity.hasPosition ? (ObjectPositionSaveData) planetEntity.position : null;
                planetInfo.ProjectileCannonSave = planetEntity.hasProjectileCannon
                    ? (ProjectileCannonSaveData) planetEntity.projectileCannon
                    : null;
                planetInfo.ProjectileTypeSave = planetEntity.hasProjectileCannonType
                    ? (ProjectileTypeSaveData) planetEntity.projectileCannonType
                    : null;
                planetInfo.StartVelocitySave = planetEntity.hasStartVelocity
                    ? (StartVelocitySaveData) planetEntity.startVelocity
                    : null;
                planetInfo.VelocitySave = planetEntity.hasVelocity ? (VelocitySaveData) planetEntity.velocity : null;
                planetInfo.CooldownSave = planetEntity.hasCooldown ? (CooldownSaveData) planetEntity.cooldown : null;
                planetInfo.ProjectilesTotalShootSave = planetEntity.hasProjectilesTotalShoot
                    ? (ProjectilesTotalShootSaveData) planetEntity.projectilesTotalShoot
                    : null;
                planetInfo.PlayerSave = planetEntity.hasPlayer ? (PlayerSaveData) planetEntity.player : null;

                save.Planets[i] = planetInfo;
            }
        }

        private void SaveProjectiles(Save save)
        {
            var projectiles = _contexts.game.GetGroup(GameMatcher.Projectile).ToArray();

            save.Projectiles = new ProjectileSaveData[projectiles.Length];

            for (var i = 0; i < projectiles.Length; i++)
            {
                var projectileEntity = projectiles[i];
                var projectile = (ProjectileSaveData) projectileEntity.projectile;

                projectile.MassSave = projectileEntity.hasMass ? (MassSaveData) projectileEntity.mass : null;
                projectile.GravityVelocitySave = projectileEntity.hasGravityVelocity ? (GravityVelocitySaveData) projectileEntity.gravityVelocity : null;
                projectile.LifeTimeSave = projectileEntity.hasLifeTime ? (LifeTimeSaveData) projectileEntity.lifeTime : null;
                projectile.PositionSave = projectileEntity.hasPosition ? (ObjectPositionSaveData) projectileEntity.position : null;
                projectile.StartVelocitySave = projectileEntity.hasStartVelocity  ? (StartVelocitySaveData) projectileEntity.startVelocity : null;
                projectile.VelocitySave = projectileEntity.hasVelocity ? (VelocitySaveData) projectileEntity.velocity : null;
                projectile.ForceSave = projectileEntity.hasForce ? (ForceSaveData) projectileEntity.force : null;

                save.Projectiles[i] = projectile;
            }
        }
    }
}