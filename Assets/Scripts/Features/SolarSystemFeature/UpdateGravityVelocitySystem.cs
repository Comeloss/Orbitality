using System.Collections.Generic;
using Entitas;

namespace Features.SolarSystemFeature
{
    public class UpdateGravityVelocitySystem : ReactiveSystem<GameEntity>
    {
        private readonly Contexts _contexts;
    
        public UpdateGravityVelocitySystem(Contexts context) : base(context.game)
        {
            _contexts = context;
        }

        protected override ICollector<GameEntity> GetTrigger(IContext<GameEntity> context)
        {
            return context.CreateCollector(GameMatcher.Force);
        }

        protected override bool Filter(GameEntity entity)
        {
            return entity.hasForce &&
                   entity.hasMass;
        }

        protected override void Execute(List<GameEntity> entities)
        {
            if (!_contexts.game.hasGameDeltaTime)
            {
                return;
            }
            
            foreach (var gameEntity in entities)
            {
                var gravityVelocity = (gameEntity.force.Force / gameEntity.mass.Value) * _contexts.game.gameDeltaTime.TimeDelta;

                gameEntity.ReplaceGravityVelocity(gravityVelocity);
            }
        }
    }
}
