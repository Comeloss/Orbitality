using System.Collections.Generic;
using System.Linq;
using Entitas;

namespace Features.CannonsFeature
{
    public class HandleCollisionSystem : ReactiveSystem<InputEntity>
    {
        private readonly Contexts _contexts;

        public HandleCollisionSystem(Contexts context) : base(context.input)
        {
            _contexts = context;
        }

        protected override ICollector<InputEntity> GetTrigger(IContext<InputEntity> context)
        {
            return context.CreateCollector(InputMatcher.Collision);
        }

        protected override bool Filter(InputEntity entity)
        {
            return entity.hasCollision;
        }

        protected override void Execute(List<InputEntity> entities)
        {
            foreach (var gameEntity in entities)
            {
                var collision = gameEntity.collision;
                
                var projectile = _contexts.game.GetGroup(GameMatcher.Projectile)
                    .FirstOrDefault(p => p.projectile.PlanetId == collision.ProjectileLauncherId &&
                                         p.projectile.ProjectileId == collision.ProjectileId);

                if (projectile == null)
                {
                    continue;
                }

                var targetPlanet = _contexts.game.GetGroup(GameMatcher.PlanetInfo)
                    .FirstOrDefault(p => p.planetInfo.Id == collision.TargetId);

                if (targetPlanet == null)
                {
                    continue;
                }

                if (targetPlanet.planetInfo.Id == projectile.projectile.PlanetId)
                {
                    continue;
                }

                projectile.isReadyToDestroy = true;


                if (!targetPlanet.hasHealth)
                {
                    continue;
                }

                var newHealth = targetPlanet.health.CurrentHp - projectile.projectile.Damage;

                if (newHealth < 0)
                {
                    newHealth = 0;
                }

                targetPlanet.ReplaceHealth(newHealth,  targetPlanet.health.TotalHp);
            }
        }
    }
}