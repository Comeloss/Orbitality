using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;

namespace Features.Game.Gravity
{
    public class UpdateProjectileStartVelocitySystem : ReactiveSystem<GameEntity>
    {
        private readonly Contexts _contexts;
    
        public UpdateProjectileStartVelocitySystem(Contexts context) : base(context.game)
        {
            _contexts = context;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.Velocity);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasVelocity &&
                entity.hasProjectile;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var projectileEntity in entities)
            {
                /*var orbitMagnitude = projectileEntity.gravityVelocity.Velocity.magnitude * projectileEntity.orbitalMagnitude.Magnitude;
                
                
                var orbitalVelocity = orbitMagnitude * 
                                      (!projectileEntity.hasVelocity ? 
                                          Vector2.Perpendicular(projectileEntity.gravityVelocity.Velocity).normalized : 
                                          projectileEntity.velocity.Velocity.normalized);*/

                var newStartVelocity = projectileEntity.velocity.Velocity;

                projectileEntity.ReplaceStartVelocity(newStartVelocity);
            }
        }
    }
}
