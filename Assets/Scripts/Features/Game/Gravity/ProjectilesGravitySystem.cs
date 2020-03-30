using System;
using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;
using Vector2 = UnityEngine.Vector2;

namespace Features.Game.Gravity
{
    public class ProjectilesGravitySystem : ReactiveSystem<GameEntity>
    {
        private readonly Contexts _contexts;

        public ProjectilesGravitySystem(Contexts context) : base(context.game)
        {
            _contexts = context;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.GameTimeMilliseconds);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasGameTimeMilliseconds;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            var planetEntities = _contexts.game.GetGroup(GameMatcher.PlanetInfo).Where(e => e.hasMass && e.hasPosition);
            var projectileEntities =
                _contexts.game.GetGroup(GameMatcher.Projectile).Where(e => e.hasMass && e.hasPosition);

            foreach (var projectileEntity in projectileEntities)
            {
                var forceVector = Vector2.zero;

                foreach (var planetEntity in planetEntities)
                {
                    if (planetEntity.planetInfo.Id == projectileEntity.projectile.PlanetId)
                    {
                        continue;
                    }

                    var distance = Vector2.Distance(planetEntity.position.Position, projectileEntity.position.Position);

                    var forceMagnitude = GetForce(planetEntity.mass.Value, projectileEntity.mass.Value, distance);

                    var direction = GetDirection(planetEntity.position.Position, projectileEntity.position.Position);

                    forceVector += direction * forceMagnitude;
                }
                
                projectileEntity.ReplaceForce(forceVector);
            }
        }

        private float GetForce(int mass1, int mass2, float r)
        {
            if (!_contexts.game.hasGravitationalConstant)
            {
                Debug.LogError("[GravitySystem] No GravitationalConstant");
                return 0;
            }

            var force = (float) (mass1 * mass2 / Math.Pow(r, 2) * _contexts.game.gravitationalConstant.GRAV);
            return force;
        }

        private Vector2 GetDirection(Vector2 position1, Vector2 position2)
        {
            var heading = position1 - position2;
            var dist = heading.magnitude;
            var direction = heading / dist;
            return direction;
        }
    }
}