using System.Collections.Generic;
using System.Linq;
using Entitas;
using UnityEngine;

namespace Features.Game.Projectile
{
    public class UpdateProjectilesLifeTimeSystem : ReactiveSystem<GameEntity>
    {
        private readonly Contexts _contexts;

        public UpdateProjectilesLifeTimeSystem(Contexts context) : base(context.game)
        {
            _contexts = context;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.LifeTime);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasLifeTime;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            foreach (var gameEntity in entities)
            {
                if (gameEntity.hasProjectile && gameEntity.lifeTime.TimeEnded)
                {
                    gameEntity.isReadyToDestroy = true;
                }
            }
        }
    }
}