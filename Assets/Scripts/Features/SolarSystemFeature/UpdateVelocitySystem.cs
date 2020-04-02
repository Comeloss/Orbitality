using System.Collections.Generic;
using Entitas;

namespace Features.SolarSystemFeature
{
    public class UpdateVelocitySystem : ReactiveSystem<GameEntity>
    {
        private readonly Contexts _contexts;
    
        public UpdateVelocitySystem(Contexts context) : base(context.game)
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
            var velocityObjects = _contexts.game.GetGroup(GameMatcher.StartVelocity);
            
            foreach (var gameEntity in velocityObjects)
            {
                var resultVelocity =  gameEntity.startVelocity.Velocity;

                if (gameEntity.hasGravityVelocity)
                {
                    resultVelocity += gameEntity.gravityVelocity.Velocity;
                }

                gameEntity.ReplaceVelocity(resultVelocity);
            }
        }
    }
}
